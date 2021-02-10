using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace MusicPlayer
{
	public class Perams
	{
		public delegate string GetVar();
		static Dictionary<string, GetVar> _outputPerams;
		public static Dictionary<string, GetVar> outputPerams
		{
			get
			{
				if (_outputPerams == null)
				{
					_outputPerams = new Dictionary<string, GetVar>()
					{
						{ "song_name", () => {
							if(MusicSys.currentSong == null) return "No song playing!";

							var elements = MusicSys.currentSong.Name.Split('_');

							if (elements.Length < 2) return elements[0];

							return elements[1];
						} },
						{ "song_artist", () => {
							if(MusicSys.currentSong == null) return "No song playing!";

							var elements = MusicSys.currentSong.Name.Split('_');

							if (elements.Length < 2) return "No artist";

							return elements[0];
						} },

						{ "playlist_name", () => string.IsNullOrEmpty(MusicSys.currentPlaylistPath) ? "No playlist selected!" : new DirectoryInfo(MusicSys.currentPlaylistPath).Name },

						{ "song_duration_mm:ss", () => MusicSys.currentSong == null ? "--:--" : MusicSys.currentSong.Duration.ToString(@"mm\:ss") },
						{ "song_duration_mm:ss.ff", () => MusicSys.currentSong == null ? "--:--.--" : MusicSys.currentSong.Duration.ToString(@"mm\:ss\.ff") },

						{ "song_timeplayed_mm:ss", () => MusicSys.currentSong == null ? "--:--" : Main.timePlayed.ToString(@"mm\:ss") },
						{ "song_timeplayed_mm:ss.ff", () => MusicSys.currentSong == null ? "--:--.--" : Main.timePlayed.ToString(@"mm\:ss\.ff") },

						{
				"song_timebar_10", () =>
			{
				if (MusicSys.currentSong == null)
					return "[          ]";
				else
				{
					int segmentsToMake = (int)Math.Round((Main.timePlayed.TotalSeconds / MusicSys.currentSong.Duration.TotalSeconds) * 10.0 - 0.5);

					var sb = new StringBuilder("[");

					for (int i = 0; i < 10; i++)
					{
						if (i <= segmentsToMake)
							sb.Append('-');
						else
							sb.Append(' ');
					}

					sb.Append(']');

					return sb.ToString();
				}
			} },

						{ "audio_volume", () => Main.volume.ToString() },
						{ "audio_volume_max", () => Main.settings.volumeIncrements.ToString() },
						{ "audio_volume_percent", () => Math.Round(SoundEffect.MasterVolume * 100).ToString() },

						{ "output_enabled", () => Main.enableOutput ? "ON" : "OFF" },

						{ "input_settings", () => Main.settings.settingsKey.ToString() },
						{ "input_output", () => Main.settings.toggleOutputKey.ToString() },
						{ "input_reload", () => Main.settings.reloadKey.ToString() },
						{ "input_volume_up", () => Main.settings.volumeUpKey.ToString() },
						{ "input_volume_down", () => Main.settings.volumeDownKey.ToString() },
					};
				}

				return _outputPerams;
			}
		}
	}
}