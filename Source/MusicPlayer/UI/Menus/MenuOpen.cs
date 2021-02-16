namespace MusicPlayer.UI.Menus
{
	public class MenuOpen : IMenu
	{
		public string name => "Open";

		public void Update()
		{
			if (GUI.Button("Open settings.json..."))
				Util.OpenFile(Settings.settingsPath);

			if (GUI.Button("Open Music Folder..."))
				Util.OpenFile(Settings.current.musicFolder);

			if (GUI.Button("Open Output Folder..."))
				Util.OpenFile(Settings.current.outputFolder);
		}
	}
}