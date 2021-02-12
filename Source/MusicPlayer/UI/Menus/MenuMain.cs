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

			if (GUI.Button("Outputting " + (Main.enableOutput ? "ON" : "OFF")))
				Main.enableOutput = !Main.enableOutput;

			if (GUI.Button("Reload"))
			{
				Main.ReloadData();
				Main.CreateFilesAndFolders();
			}

			if (GUI.Button("Open Settings..."))
				Process.Start(new ProcessStartInfo("explorer", "\"" + Main.settingsPath + "\""));
		}
	}
}