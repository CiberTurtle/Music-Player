using System.Text;
using System.IO;
using System.Diagnostics;

namespace MusicPlayer
{
	public static class Util
	{
		public static string ParsePath(string path)
		{
			ParsePath(ref path);
			return path;
		}

		public static void ParsePath(ref string path)
		{
			// Path is relitive
			if (path.StartsWith('/'))
				path = Main.root + path;
		}

		public static string MakeBar(int value, int max)
		{
			var sb = new StringBuilder("[");

			for (int i = 0; i < max; i++)
				if (i < (float)value - 0.25f)
					sb.Append('-');
				else
					sb.Append(' ');

			sb.Append(']');

			return sb.ToString();
		}

		public static void OpenFile(string path)
		{
			Process.Start(new ProcessStartInfo("explorer", "\"" + path.Replace('/', '\\') + "\""));
		}
	}
}