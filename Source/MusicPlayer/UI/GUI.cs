using System.Collections.Generic;

namespace MusicPlayer.UI
{
	public static class GUI
	{
		static Vector2 position = Vector2.zero;
		static Vector2 itemSize = new Vector2(Main.graphics.PreferredBackBufferWidth, 32);

		static Vector2 offset = new Vector2(16);
		static Vector2 textPadding = new Vector2(0, 8);

		static List<IDrawable> draws = new List<IDrawable>();

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

			var isHovered = new Rect(position + new Vector2(0, itemSize.y / 2), itemSize).Contains(Input.windowMousePosition);

			if (isHovered) Pointer.SetHand();
			Text(isHovered ? "#" + text : text);

			return isHovered && Input.CheckButtonPress(Inputs.MouseLeft);
		}

		public static void AddDraw(IDrawable draw)
		{
			draws.Add(draw);
			AddSpace();
		}

		public static void LineBreak() => Text(string.Empty);

		static void AddSpace() => position.y += itemSize.y;
	}
}