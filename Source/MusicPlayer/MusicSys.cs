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
			currentPlaylistPath = Directory.GetDirectories(Main.path + Main.settings.musicPath).ToList().Find((x) => !x.Contains('@'));

			// Choose Song
			var songs = Directory.GetFiles(currentPlaylistPath, "*.wav", SearchOption.AllDirectories).ToList();

			songs.RemoveAll((x) => x.Contains('@'));

			currentSongPath = songs[Main.rng.Next(songs.Count)];

			if (currentSong != null && songs.Count > 1)
				while (currentSongPath.Contains(currentSong.Name))
					currentSongPath = songs[Main.rng.Next(songs.Count)];

			// Load Song
			currentSong?.Dispose();
			currentSong = SoundEffect.FromFile(currentSongPath);
			currentSong.Name = new FileInfo(currentSongPath).Name.Replace(".wav", "");

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