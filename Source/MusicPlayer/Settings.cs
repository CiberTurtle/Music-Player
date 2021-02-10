using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MusicPlayer
{
	[Serializable]
	public class Settings
	{
		[Description("Defualt volume in increments. [0 - volumeIncrements]"), Range(0, int.MaxValue)]
		public int startingVolume = 10;

		[Description("Volume in increments or \"max volume\". [0 - 2147483647]"), Range(0, int.MaxValue)]
		public int volumeIncrements = 10;
		[Description("Time between Ticks. Set to -1 to tick every frame.")]
		public double tickTime = 1.0;
		public Inputs settingsKey = Inputs.F1;
		public Inputs toggleOutputKey = Inputs.F2;
		public Inputs reloadKey = Inputs.F5;
		public Inputs volumeUpKey = Inputs.OemPlus;
		public Inputs volumeDownKey = Inputs.OemMinus;
		public Inputs pauseKey = Inputs.Space;

		[Description("Path to a folder containing playlists that contains music. (can be absolute)"), Required]
		public string musicPath = @"/Music";
		[Description("Path to a folder where all outputs are relitive to. (can be absolute)"), Required]
		public string outputPath = @"/Output";

		public char disabledChar = '@';

		[Description("Text that appears in the window (will get parsed) (start with '#' to make the line bold).")]
		public string[] windowTexts = new string[]
		{
			"#{song_artist}: {song_name} from {playlist_name}",
			"{song_timeplayed_mm:ss.ff} {song_time_bar_10} {song_duration_mm:ss.ff}",
			"{audio_volume} / {audio_volume_max} ('{input_volume_down}' '{input_volume_up}')",
			"Outputting {output_enabled} ('{input_output}')",
			"Settings ('{input_settings}')",
			"Reload ('{input_reload}')",
		};

		[Description("Outputs tiggered after a new song is being played.")]
		public Output[] outputsNewSong = new Output[]
		{
			new Output(@"/song_name.txt", "{song_name}"),
			new Output(@"/song_artist.txt", "{song_artist}"),
		};

		[Description("Outputs triggered when a tick happens.")]
		public Output[] outputsEveryTick = new Output[]
		{
			new Output(@"/song_time.txt", "{song_time_mm:ss} / {song_duration_mm:ss}"),
		};
	}
}