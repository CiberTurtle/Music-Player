using System.Diagnostics;

namespace MusicPlayer.UI.Menus
{
	public class MenuMain : IMenu
	{
		public string name => "Main";

		public void Update()
		{
			if (GUI.Button("Play >"))
				Menu.OpenMenu(new MenuSelectPlaylist());

			if (MusicSys.currentSong != null)
				if (GUI.Button("Stop"))
					MusicSys.Stop();

			GUI.Checkbox("Enable Autoplay", ref Main.enableAutoplay);
			GUI.Checkbox("Enable Output", ref Main.enableOutput);

			if (GUI.Button("Reload"))
				Main.ReloadData();

			if (GUI.Button("Open >"))
				Menu.OpenMenu(new MenuOpen());

			GUI.LineBreak();

			if (GUI.Button("Quit"))
				Main.current.Exit();
		}
	}
}