using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;

namespace MusicPlayer
{
	[Serializable]
	public class Settings
	{
		public Settings()
		{
			version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
		}

		#region Values
		public Version version;

		[Description("Defualt volume in increments. [0 - volumeIncrements]"), Range(0, int.MaxValue)]
		public int startingVolume = 10;
		[Description("Volume in increments or \"max volume\". [0 - 2147483647]"), Range(0, int.MaxValue)]
		public int volumeIncrements = 10;

		[Description("Time between Ticks. Set to <= 0 to tick every frame.")]
		public double tickTime = 1.0;
		public bool throttleWhenUnfocused = true;

		public Inputs volumeUpKey = Inputs.OemPlus;
		public Inputs volumeDownKey = Inputs.OemMinus;
		public Inputs pauseKey = Inputs.Space;

		[Description("Should animations be disabled?")]
		public bool staticUI = false;
		[Description("The opaque accent color the ui uses.")]
		public Color accentColor = new Color("#FF7504");

		[Description("Path to a folder containing playlists that contains music. (can be absolute)"), Required]
		public string musicPath = @"/Music";
		[Description("Path to a folder where all outputs are relitive to. (can be absolute)"), Required]
		public string outputPath = @"/Output";

		public char disabledChar = '@';

		[Description("Text that appears in the window (will get parsed) (start with '#' to make the line bold).")]
		public string[] windowTexts = new string[]
		{
			"{song_artist}: {song_name} from {playlist_name}",
			"{song_time_mm:ss.ff} {song_time_bar_10} {song_duration_mm:ss.ff} {song_state_name}",
			"{audio_volume} {audio_volume_bar} {audio_volume_max}",
		};

		public string windowTitle = "Music Player  [{song_time_mm:ss} / {song_duration_mm:ss}]  {song_artist} {song_name} from {playlist_name}";

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
		#endregion

		#region Static Things
		static string _settingsPath = @"/settings.json";
		public static string settingsPath
		{
			get
			{
				if (_settingsPath.StartsWith('/'))
					Util.ParsePath(ref _settingsPath);

				return _settingsPath;
			}
		}

		static Settings _current;
		public static Settings current
		{
			get
			{
				if (_current == null)
				{
					if (!File.Exists(settingsPath))
						File.Create(settingsPath);

					var json = File.ReadAllText(settingsPath);

					if (string.IsNullOrEmpty(json))
						_current = new Settings();
					else
						_current = JsonConvert.DeserializeObject<Settings>(json);

					WriteSettings();
				}

				return _current;
			}
		}

		string _outputFolder;
		[JsonIgnore]
		public string outputFolder
		{
			get
			{
				if (string.IsNullOrEmpty(_outputFolder))
					_outputFolder = Util.ParsePath(current.outputPath);

				return _outputFolder;
			}
		}

		string _musicFolder;
		[JsonIgnore]
		public string musicFolder
		{
			get
			{
				if (string.IsNullOrEmpty(_musicFolder))
					_musicFolder = Util.ParsePath(current.musicPath);

				return _musicFolder;
			}
		}

		public static void MakeDataNull()
		{
			_current = null;
		}

		public static void WriteSettings()
		{
			// Kinda hacky but idk
			File.WriteAllText(
				settingsPath,
				"{\n\t\"$schema\": \"./settings.schema.json\"," + JsonConvert.SerializeObject(_current).Remove(0, 1)
			);
		}
		#endregion
	}
}