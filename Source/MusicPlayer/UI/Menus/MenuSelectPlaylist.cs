using System.Collections.Generic;
using System.IO;

namespace MusicPlayer.UI.Menus
{
	public class MenuSelectPlaylist : IMenu
	{
		public string name => "Play";

		public void Update()
		{
			if (MusicSys.playlists.Length > 1)
				if (GUI.Button("Random"))
				{
					MusicSys.currentPlaylistPath = MusicSys.playlists[Main.rng.Next(MusicSys.playlists.Length)];
					MusicSys.prevSongs = new Queue<string>();
					MusicSys.PlayRandomSongFromPlaylist();
				}

			foreach (var playlist in MusicSys.playlists)
			{
				if (GUI.Button(new DirectoryInfo(playlist).Name))
				{
					if (MusicSys.currentPlaylistPath != playlist)
					{
						MusicSys.currentPlaylistPath = playlist;
						MusicSys.prevSongs = new Queue<string>();
					}
					MusicSys.PlayRandomSongFromPlaylist();
				}
			}

			if (GUI.Button("Add more playlists >"))
				Util.OpenFile(Settings.current.musicFolder);
		}
	}
}