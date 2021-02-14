using System.Collections.Generic;
using System.Linq;
using System.Text;
using MusicPlayer.UI.Menus;

namespace MusicPlayer.UI
{
	public static class Menu
	{
		static Stack<IMenu> menus = new Stack<IMenu>();
		static StringBuilder menusText;

		static IMenu queuedMenu = null;
		static bool goBack = false;

		public static void OpenMenu(IMenu menu)
		{
			if (queuedMenu != null)
				throw new System.Exception("Queued Menu is not null! How did this happen?");

			queuedMenu = menu;
		}

		public static void Back() => goBack = true;

		public static void Update()
		{
			GUI.DrawCustomGUI();

			if (goBack)
			{
				goBack = false;

				menus.Pop();
				UpdateText();
			}

			if (queuedMenu != null)
			{
				menus.Push(queuedMenu);
				queuedMenu = null;
				UpdateText();
			}

			if (GUI.Button(menusText.ToString(), false) && menus.Count > 1)
				Back();

			GUI.LineBreak();

			if (menus.Count == 0)
			{
				menus.Push(new MenuMain());
				Main.LogError("Menus stack was empty! How did this happen? Oh well I fixed the problem for you, still report the bug though.");
			}

			menus.Peek().Update();
		}

		static void UpdateText()
		{
			menusText = new StringBuilder();

			var list = menus.ToList();
			list.Reverse();

			foreach (var menu in list)
				menusText.Append(menu.name + " / ");

			menusText.Remove(menusText.Length - 3, 3);
		}
	}
}