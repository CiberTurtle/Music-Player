using Microsoft.Xna.Framework.Graphics;
using MusicPlayer;

namespace MusicPlayer.UI
{
	public struct UIText : IDrawable
	{
		public string text;
		public Color color;
		public SpriteFont font;

		public UIText(string text, Color color, SpriteFont font)
		{
			this.text = text;
			this.color = color;
			this.font = font;
		}

		public void Draw(Vector2 position)
		{
			Main.sb.DrawString(font, text, position, color);
		}
	}
}