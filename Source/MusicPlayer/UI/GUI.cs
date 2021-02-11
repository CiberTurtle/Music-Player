namespace MusicPlayer.UI
{
	public static class GUI
	{
		static Vector2 position;
		static Vector2 itemSize = new Vector2(Main.graphics.PreferredBackBufferWidth, 32);

		static Vector2 offset = new Vector2(32);
		static Vector2 textPadding = new Vector2(0, 8);

		public static void Reset()
		{
			position = Vector2.zero;
		}

		public static void Text(string text)
		{
			if (text.StartsWith('#'))
				Main.sb.DrawString(Main.bold, text.Remove(0, 1), position + offset + textPadding, Color.whiteBright);
			else
				Main.sb.DrawString(Main.font, text, position + offset + textPadding, Color.white);

			AddSpace();
		}

		public static bool Button(string text)
		{
			text = "[ " + text + " ]";

			var isHovered = new Rect(position + new Vector2(0, itemSize.y), itemSize).Contains(Input.windowMousePosition);

			if (isHovered) Pointer.SetHand();
			Text(isHovered ? "#" + text : text);

			return isHovered && Input.CheckButtonPress(Inputs.MouseLeft);
		}

		public static void AddSpace()
		{
			position.y += itemSize.y;
		}
	}
}