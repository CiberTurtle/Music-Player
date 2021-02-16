using System;
using System.Collections.Generic;
using System.IO;
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
							if(MusicSys.currentSong == null)
								return "No song playing!";

							return MusicSys.currentSong.Name;
						} },
						{ "song_artist", () => {
							if(MusicSys.currentSong == null)
								return "No song playing!";

							var elements = new FileInfo(MusicSys.currentSongPath).Name.Split('_');
							if (elements.Length < 2) return "No artist";
							return elements[0];
						} },

						{ "playlist_name", () => string.IsNullOrEmpty(MusicSys.currentPlaylistPath) ? "No playlist selected!" : new DirectoryInfo(MusicSys.currentPlaylistPath).Name },

						{ "song_duration_mm:ss", () => MusicSys.currentSong == null ? "--:--" : MusicSys.currentSong.Duration.ToString(@"mm\:ss") },
						{ "song_duration_mm:ss.ff", () => MusicSys.currentSong == null ? "--:--.--" : MusicSys.currentSong.Duration.ToString(@"mm\:ss\.ff") },

						{ "song_time_mm:ss", () => MusicSys.currentSong == null ? "--:--" : Main.timePlayed.ToString(@"mm\:ss") },
						{ "song_time_mm:ss.ff", () => MusicSys.currentSong == null ? "--:--.--" : Main.timePlayed.ToString(@"mm\:ss\.ff") },

						{ "song_state_name", () => MusicSys.currentSongInstance == null ? "No Song" : MusicSys.currentSongInstance.State.ToString() },
						{ "song_state_inverted_icon", () => {
							if(MusicSys.currentSongInstance == null) return ">>";
							switch (MusicSys.currentSongInstance.State)
							{
								case SoundState.Playing: return "||";
								case SoundState.Paused: return "> ";
								case SoundState.Stopped: return ">>";
							}
							return ">>";
						} },

						{
						"song_time_bar_10", () =>
					Util.MakeBar(
						MusicSys.currentSong == null ?
						0 :
						(int)Math.Round(((float)Main.timePlayed.TotalSeconds / (float)MusicSys.currentSong.Duration.TotalSeconds) * 10.0f),
						10) },

						{ "audio_volume", () => MusicSys.volume.ToString() },
						{ "audio_volume_max", () => Settings.current.volumeIncrements.ToString() },
						{ "audio_volume_percent", () => Math.Round(SoundEffect.MasterVolume * 100).ToString() },
						{ "audio_volume_bar", () => Util.MakeBar(MusicSys.volume, Settings.current.volumeIncrements) },

						{ "output_enabled", () => Main.enableOutput ? "ON" : "OFF" },

						{ "input_volume_up", () => Settings.current.volumeUpKey.ToString() },
						{ "input_volume_down", () => Settings.current.volumeDownKey.ToString() },
					};
				}

				return _outputPerams;
			}
		}
	}
}