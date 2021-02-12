using System.Collections.Generic;

namespace MusicPlayer.UI
{
	public static class Menu
	{
		static Stack<IMenu> menus = new Stack<IMenu>();
		static IMenu queuedMenu = null;

		public static void OpenMenu(IMenu menu)
		{
			if (queuedMenu == null)
			{
				queuedMenu = menu;
				return;
			}
			throw new System.Exception("Queued Menu is not null! How did this happen?");
		}

		public static void Back() => menus.Pop();

		public static void Update()
		{
			if (queuedMenu != null)
			{
				menus.Push(queuedMenu);
				queuedMenu = null;
			}

			menus.Peek().Update();

			GUI.LineBreak();

			if (menus.Count > 1)
				if (GUI.Button("< Back"))
					Back();
		}
	}
}