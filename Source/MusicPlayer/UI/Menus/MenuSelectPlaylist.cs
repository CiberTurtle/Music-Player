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
					MusicSys.PlayRandomSongFromPlaylist();
				}

			foreach (var playlist in MusicSys.playlists)
			{
				if (GUI.Button(new DirectoryInfo(playlist).Name))
				{
					MusicSys.currentPlaylistPath = playlist;
					MusicSys.PlayRandomSongFromPlaylist();
				}
			}

			if (GUI.Button("Add more playlists >"))
				Util.OpenFile(Settings.current.musicFolder);
		}
	}
}