using System.Collections.Generic;
using MusicPlayer.UI.UIElements;

namespace MusicPlayer.UI
{
	public static class GUI
	{
		public static readonly Vector2 itemSize = new Vector2(Main.graphics.PreferredBackBufferWidth, 24);
		public static readonly Vector2 offset = new Vector2(16);

		public static Vector2 position = Vector2.zero;

		public static List<IDrawable> draws = new List<IDrawable>();

		public static void Update()
		{
			position = Vector2.zero;
			draws = new List<IDrawable>();
		}

		public static void Draw()
		{
			position = offset;

			foreach (var draw in draws)
			{
				draw.Draw(position);

				if (draw.space)
					AddSpace();
			}
		}

		public static void Text(string text) =>
			AddDraw(new UIText(text, Color.white, Main.font));

		public static void Text(string text, Color color, Vector2 offset) =>
			AddDraw(new UIText(text, color, Main.font, offset));

		public static bool Button(string text, bool makeFancy = true)
		{
			if (makeFancy)
				text = $"[ {text} ]";

			var rect = new Rect(position + new Vector2(0, itemSize.y / 2), itemSize);
			var isHovered = rect.Contains(Input.windowMousePosition);

			if (isHovered)
			{
				Pointer.SetHand();

				AddDraw(new UIBox(rect, new Color("#FF750422")));
				Text(
					text, Color.whiteBright,
					Main.settings.enableShakingText ?
						new Vector2((float)(Main.rng.Next(0, 4) - 1) / 3, (float)(Main.rng.Next(0, 4) - 1) / 3) :
						Vector2.zero);
			}
			else
				Text(text, Color.white, Vector2.zero);

			return isHovered && Input.CheckButtonPress(Inputs.MouseLeft);
		}

		public static void Checkbox(string text, ref bool state)
		{
			if (Button((state ? "[X] " : "[ ] ") + text, false)) state = !state;
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