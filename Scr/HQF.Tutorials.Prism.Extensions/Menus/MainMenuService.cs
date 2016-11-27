using System;
using System.Collections;
using System.Linq;

namespace HQF.Tutorials.Prism.Extensions.Menus
{
	public class MainMenuService : IMenuService
	{
		MainMenuNode menu = new MainMenuNode();

		public MainMenuService()
		{
		}

		#region IMenuService Members

		public void AddTopLevelMenu(MenuItemNode node)
		{
			menu.Menus.Add(node);
		}

		public void RegisterMenu(MenuItemNode node)
		{
			String[] tokens = node.Path.Split('/');
			RegisterMenu(tokens.GetEnumerator(), menu.Menus, node);
		}

		public MainMenuNode GetMenuRoot()
		{
			return menu;
		}

		#endregion

		private void RegisterMenu(IEnumerator tokenEnumerator, MenuItemNodeCollection current, MenuItemNode item)
		{
			if (!tokenEnumerator.MoveNext())
			{
				current.Add(item);
			}
			else
			{
				MenuItemNode menuPath = current.FirstOrDefault(x=> x.Text == tokenEnumerator.Current.ToString());
				
				if (menuPath == null)
				{
					menuPath = new MenuItemNode(String.Empty);
					menuPath.Text = tokenEnumerator.Current.ToString();
					current.Add(menuPath);
				}

				RegisterMenu(tokenEnumerator, menuPath.Children, item);
			}
		}
	}
}
