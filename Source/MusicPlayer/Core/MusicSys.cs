using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace MusicPlayer
{
	public class MusicSys
	{
		static int _volume;
		public static int volume
		{
			get => _volume;
			set
			{
				value = Math.Clamp(value, 0, Settings.current.volumeIncrements);

				_volume = value;
				SoundEffect.MasterVolume = value * (1.0f / Settings.current.volumeIncrements);
			}
		}

		public static string currentSongPath;
		public static SoundEffect currentSong;
		public static SoundEffectInstance currentSongInstance;

		public static string currentPlaylistPath;

		static string[] _playlists;
		public static string[] playlists
		{
			get
			{
				if (_playlists == null)
					_playlists = Directory.GetDirectories(Settings.current.musicFolder);
				return _playlists;
			}

			set => _playlists = value;
		}

		public static void PlayRandomSongFromPlaylist()
		{
			Stop();

			if (currentPlaylistPath == null)
			{
				Main.LogError("There is no active playlist!");
				return;
			}

			// Choose Song
			var songs = Directory.GetFiles(currentPlaylistPath, "*.wav", SearchOption.AllDirectories)?.ToList();
			songs?.RemoveAll((x) => x.Contains('@'));

			if (songs?.Count == 0)
			{
				Main.LogError($"There are no songs in the {new DirectoryInfo(currentPlaylistPath).Name} playlist!");
				return;
			}

			if (currentPlaylistPath == null)
			{
				Main.LogError($"There are no active songs in {new DirectoryInfo(currentPlaylistPath).Name} playlist!");
				return;

			}

			var prevSong = currentSongPath;
			currentSongPath = songs[Main.rng.Next(songs.Count)];

			// Don't play the same song again, play it again if there is only one song
			if (currentSong != null && songs.Count > 1)
				while (currentSongPath == prevSong)
					currentSongPath = songs[Main.rng.Next(songs.Count)];

			// Load Song
			currentSong = SoundEffect.FromFile(currentSongPath);

			var names = new FileInfo(currentSongPath).Name.Split('_');

			if (names.Length < 2)
				currentSong.Name = names[0];
			else
				currentSong.Name = names[1];

			currentSong.Name = currentSong.Name.Remove(currentSong.Name.LastIndexOf('.'));

			OutputSys.UpdateOutput(false);

			Main.timePlayed = TimeSpan.Zero;

			// Play Song
			currentSongInstance?.Stop();
			currentSongInstance?.Dispose();
			currentSongInstance = currentSong.CreateInstance();
			currentSongInstance.Play();
		}

		public static void Stop()
		{
			MusicSys.currentSong?.Dispose();
			MusicSys.currentSong = null;
			MusicSys.currentSongInstance?.Dispose();
			MusicSys.currentSongInstance = null;
			// MusicSys.currentPlaylistPath = string.Empty;
		}


		public static void GetCurrentSoundData()
		{
		}
	}
}