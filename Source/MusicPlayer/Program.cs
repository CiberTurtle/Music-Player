using System;

namespace MusicPlayer
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			using (var game = new Main())
				game.Run();
		}
	}
}