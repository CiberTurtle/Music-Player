using System.IO;
using System.Text;

namespace MusicPlayer
{
	public class OutputSys
	{
		public static void UpdateAllOutputs(bool forceOutput = false)
		{
			UpdateOutput(false, forceOutput);
			UpdateOutput(true, forceOutput);
		}

		public static void UpdateOutput(bool isTick, bool forceOutput = false)
		{
			Main.current.Window.Title = ParsePerams(Settings.current.windowTitle);

			if (!Main.enableOutput && !forceOutput) return;

			foreach (var output in isTick ? Settings.current.outputsEveryTick : Settings.current.outputsNewSong)
			{
				var text = output.text;

				ParsePerams(ref text);

				int hashCode = text.GetHashCode();

				if (hashCode != output.lastOutput)
				{
					output.lastOutput = hashCode;

					using (var fs = File.Open(Settings.current.outputFolder + output.path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
					{
						var bytes = Encoding.UTF8.GetBytes(text);
						fs.Write(bytes, 0, bytes.Length);
					}
				}
			}
		}

		public static string ParsePerams(string text)
		{
			ParsePerams(ref text);
			return text;
		}

		public static void ParsePerams(ref string text)
		{
			foreach (var peram in Perams.outputPerams)
			{
				var key = "{" + peram.Key + "}";
				if (text.Contains(key))
				{
					text = text.Replace(key, peram.Value.Invoke());
				}
			}
		}
	}
}