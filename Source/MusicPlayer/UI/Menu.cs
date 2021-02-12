using System.Collections.Generic;

namespace MusicPlayer.UI
{
	public static class Menu
	{
		static List<IMenu> menus = new List<IMenu>();

		public static void OpenMenu(IMenu menu) =>
			menus.Add(menu);

		public static void Back() =>
			menus.RemoveAt(menus.Count - 1);

		public static void Update()
		{
			menus[menus.Count - 1].Update();

			GUI.LineBreak();

			if (menus.Count > 1)
				if (GUI.Button("< Back"))
					Back();
		}
	}
}