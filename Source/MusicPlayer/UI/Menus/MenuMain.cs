using System.Diagnostics;

namespace MusicPlayer.UI.Menus
{
	public class MenuMain : IMenu
	{
		public void Update()
		{
			GUI.DrawCustomGUI();

			if (GUI.Button("Play..."))
				Menu.OpenMenu(new MenuSelectPlaylist());

			GUI.Checkbox("Enable Output", ref Main.enableOutput);

			if (GUI.Button("Reload"))
				Main.ReloadData();

			if (GUI.Button("Open Settings..."))
				Util.OpenFile(Main.settingsPath);
		}
	}
}