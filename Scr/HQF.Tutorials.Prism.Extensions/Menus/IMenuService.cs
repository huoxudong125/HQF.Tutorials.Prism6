namespace HQF.Tutorials.Prism.Extensions.Menus
{
    public interface IMenuService
    {
        MainMenuNode GetMenuRoot();

        void AddTopLevelMenu(MenuItemNode node);

        void RegisterMenu(MenuItemNode node);
    }
}