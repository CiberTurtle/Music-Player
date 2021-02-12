namespace MusicPlayer.UI
{
	public interface IDrawable
	{
		bool space { get; }

		void Draw(Vector2 position);
	}
}