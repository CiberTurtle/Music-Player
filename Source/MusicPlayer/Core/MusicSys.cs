using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace MusicPlayer
{
	public class MusicSys
	{
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
					_playlists = Directory.GetDirectories(Util.ParsePath(Main.settings.musicPath));
				return _playlists;
			}

			set => _playlists = value;
		}

		public static void PlayRandomSong()
		{
			if (currentPlaylistPath == null)
			{
				Main.LogError("There are no active playlists!");
				return;
			}

			// Choose Song
			var songs = Directory.GetFiles(currentPlaylistPath, "*.wav", SearchOption.AllDirectories).ToList();

			if (songs?.Count == 0)
			{
				Main.LogError($"There are no songs in the {new DirectoryInfo(currentPlaylistPath).Name}!");
				return;
			}

			songs.RemoveAll((x) => x.Contains('@'));

			if (currentPlaylistPath == null)
			{
				Main.LogError($"There are no active songs in {new DirectoryInfo(currentPlaylistPath).Name}!");
				return;
			}

			currentSongPath = songs[Main.rng.Next(songs.Count)];

			// Don't play the same song again, play it again ifayhet agai rlye is only one song
			if (currentSong != null && songs.Count > 1)
				while (currentSongPath.Contains(currentSong.Name))
					currentSongPath = songs[Main.rng.Next(songs.Count)];

			// Load Song
			currentSong?.Dispose();
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
			MusicSys.currentPlaylistPath = string.Empty;
		}
	}
}