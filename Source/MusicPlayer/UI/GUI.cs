using System.Collections.Generic;
using MusicPlayer.UI.UIElements;

namespace MusicPlayer.UI
{
	public static class GUI
	{
		public static readonly Vector2 itemSize = new Vector2(Main.graphics.PreferredBackBufferWidth, 24);
		public static readonly Vector2 offset = new Vector2(16);
		public static readonly Vector2 hoverOffsetUpSpeed = new Vector2(4, 0);
		public static readonly Vector2 hoverOffsetDownSpeed = new Vector2(8, 0);
		public static readonly Vector2 hoverOffsetMax = new Vector2(4, 0);

		public static Vector2 position = Vector2.zero;
		static Vector2 _hoverOffset = Vector2.zero;
		public static Vector2 hoverOffset
		{
			get => _hoverOffset;
			set
			{
				if (Settings.current.staticUI) return;

				_hoverOffset = value;
				_hoverOffset.x = Math.Clamp(_hoverOffset.x, 0, hoverOffsetMax.x);
			}
		}
		public static Vector2 hoverPos;

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
				if (hoverPos == position)
					hoverOffset += new Vector2(hoverOffsetUpSpeed.x * (float)Main.deltaTime * 10, 0);
				else
					hoverOffset = Vector2.zero;

				hoverPos = position;

				Pointer.SetHand();

				AddDraw(new UIBox(rect, Settings.current.accentColor.ChangeAlpha(0.2f)));
				Text(text, Color.whiteBright, hoverOffset);
			}
			else
			{
				if (hoverPos == position)
				{
					hoverOffset -= new Vector2(hoverOffsetDownSpeed.x * (float)Main.deltaTime * 10, 0);
					Text(text, Color.white, hoverOffset);

					Pointer.Reset();
				}
				else
					Text(text, Color.white, Vector2.zero);
			}

			return isHovered && Input.CheckButtonPress(Inputs.MouseLeft);
		}

		public static void Checkbox(string text, ref bool state)
		{
			if (Button((state ? "[X] " : "[ ] ") + text, false)) state = !state;
		}

		public static void DrawCustomGUI()
		{
			foreach (var text in Settings.current.windowTexts)
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