namespace MusicPlayer.UI.UIElements
{
	public class UIBox : IDrawable
	{
		public bool space => false;

		public Rect rect;
		public Color color;

		public UIBox(Rect rect, Color color)
		{
			this.rect = rect;
			this.color = color;
		}

		public void Draw(Vector2 position)
		{
			Graphics.DrawBox(rect, color);
		}
	}
}