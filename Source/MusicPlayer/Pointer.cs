using Microsoft.Xna.Framework.Input;

namespace MusicPlayer
{
	public static class Pointer
	{
		public static readonly MouseCursor defaultCursor = MouseCursor.Arrow;

		static MouseCursor _cursor;
		public static MouseCursor cursor
		{
			get => _cursor;
			set
			{
				_cursor = value;
				Mouse.SetCursor(_cursor);
			}
		}

		public static void SetHand()
		{
			cursor = MouseCursor.Hand;
		}

		public static void Reset()
		{
			cursor = defaultCursor;
		}
	}
}