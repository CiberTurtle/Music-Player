using Microsoft.Xna.Framework.Graphics;
using MusicPlayer;

namespace MusicPlayer.UI.UIElements
{
	public struct UIText : IDrawable
	{
		public bool space => true;

		public string text;
		public Color color;
		public SpriteFont font;
		public Vector2 offset;

		public UIText(string text, Color color, SpriteFont font)
		{
			this.text = text;
			this.color = color;
			this.font = font;
			this.offset = Vector2.zero;
		}

		public UIText(string text, Color color, SpriteFont font, Vector2 offset)
		{
			this.text = text;
			this.color = color;
			this.font = font;
			this.offset = offset;
		}

		public void Draw(Vector2 position)
		{
			Main.sb.DrawString(font, text, position + offset, color);
		}
	}
}