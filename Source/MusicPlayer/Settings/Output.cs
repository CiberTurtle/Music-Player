using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MusicPlayer
{
	[Serializable]
	public class Output
	{
		[Description("Path relitive to 'outputPath' where the text gets generated (also incudes the file name and extention)."), Required]
		public string path;
		[Description("Text that gets outputed in the file (will get parsed)."), Required]
		public string text;

		[NonSerialized] public int lastOutput = string.Empty.GetHashCode();

		public Output() { }

		public Output(string path, string text)
		{
			this.path = path;
			this.text = text;
		}
	}
}