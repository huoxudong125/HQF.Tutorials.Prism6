namespace HQF.Tutorials.Prism.Extensions.Menus
{
	public class MainMenuNode
	{
		public MainMenuNode()
		{
			Menus = new MenuItemNodeCollection();
		}

		public MenuItemNodeCollection Menus { get; private set; }
	}
}
