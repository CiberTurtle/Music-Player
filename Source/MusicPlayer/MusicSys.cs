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

		public static void PlayRandomSong()
		{
			// Find Active Playlist
			var playlists = Directory.GetDirectories(Util.ParsePath(Main.settings.musicPath)).ToList();

			if (playlists?.Count == 0)
			{
				Main.LogError("There are no playlists in the music folder!");
				return;
			}

			currentPlaylistPath = playlists.Find((x) => !x.Contains('@'));

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
			currentSong.Name = new FileInfo(currentSongPath).Name.Substring(new FileInfo(currentSongPath).Name.LastIndexOf('.'));

			OutputSys.UpdateOutput(false);

			Main.timePlayed = TimeSpan.Zero;

			// Play Song
			currentSongInstance?.Stop();
			currentSongInstance?.Dispose();
			currentSongInstance = currentSong.CreateInstance();
			currentSongInstance.Play();
		}
	}
}