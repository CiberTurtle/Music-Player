namespace MusicPlayer.UI.Menus
{
	public class MenuOpen : IMenu
	{
		public string name => "Open";

		public void Update()
		{
			if (GUI.Button("Open Settings..."))
				Util.OpenFile(Settings.settingsPath);

			if (GUI.Button("Open Output..."))
				Util.OpenFile(Settings.current.outputFolder);
		}
	}
}