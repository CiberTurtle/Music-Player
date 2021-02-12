using System.Collections.Generic;
using MusicPlayer.UI.UIElements;

namespace MusicPlayer.UI
{
	public static class GUI
	{
		public static Vector2 position = Vector2.zero;
		public static Vector2 itemSize = new Vector2(Main.graphics.PreferredBackBufferWidth, 32);

		public static Vector2 offset = new Vector2(16);
		public static Vector2 textPadding = new Vector2(0, 8);

		public static List<IDrawable> draws = new List<IDrawable>();

		public static void Update()
		{
			position = Vector2.zero;
			draws = new List<IDrawable>();
		}

		public static void Draw()
		{
			position = offset + textPadding;

			foreach (var draw in draws)
			{
				draw.Draw(position);

				if (draw.space)
					AddSpace();
			}
		}

		public static void Text(string text)
		{
			if (text.StartsWith('#'))
				AddDraw(new UIText(text.Remove(0, 1), Color.whiteBright, Main.bold));
			else
				AddDraw(new UIText(text, Color.white, Main.font));
		}

		public static void Text(string text, Color color, bool isBold) =>
			AddDraw(new UIText(text, color, isBold ? Main.bold : Main.font));

		public static bool Button(string text)
		{
			text = $"[ {text} ]";

			var rect = new Rect(position + new Vector2(0, itemSize.y / 2), itemSize);
			var isHovered = rect.Contains(Input.windowMousePosition);

			if (isHovered)
			{
				Pointer.SetHand();

				AddDraw(new UIBox(rect, new Color("#FF750444")));

				Text("#" + text);
			}
			else
				Text(text);

			return isHovered && Input.CheckButtonPress(Inputs.MouseLeft);
		}

		public static void DrawCustomGUI()
		{
			foreach (var text in Main.settings.windowTexts)
				GUI.Text(OutputSys.ParsePerams(text));

			GUI.LineBreak();
		}

		public static void AddDraw(IDrawable draw)
		{
			draws.Add(draw);
			if (draw.space)
				AddSpace();
		}

		public static void LineBreak() => Text(string.Empty);

		static void AddSpace() => position.y += itemSize.y;
	}
}