using System.IO;

namespace MusicPlayer.UI.Menus
{
	public class MenuSelectPlaylist : IMenu
	{
		public void Update()
		{
			GUI.DrawCustomGUI();

			foreach (var playlist in MusicSys.playlists)
			{
				if (GUI.Button(new DirectoryInfo(playlist).Name))
				{
					MusicSys.currentPlaylistPath = playlist;
					MusicSys.PlayRandomSong();
				}
			}
		}
	}
}