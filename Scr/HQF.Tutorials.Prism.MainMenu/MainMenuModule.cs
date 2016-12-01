using System;
using System.Windows;
using HQF.Tutorials.Prism.Extensions.Menus;
using HQF.Tutorials.Prism.Infrastructure.Constants;
using HQF.Tutorials.Prism.Infrastructure.Interfaces;
using HQF.Tutorials.Prism.MainMenu.Menus;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace HQF.Tutorials.Prism.MainMenu
{
    public class MainMenuModule : IModule
    {
        private IRegionManager _regionManager;
        private readonly IUnityContainer _container;

        public MainMenuModule(RegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            // INSERT RESOURCES
            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("pack://application:,,,/HQF.Tutorials.Prism.MainMenu;Component/Resources/Menu.xaml");
            Application.Current.Resources.MergedDictionaries.Add(dictionary);

            // REGISTER MENU TYPES
            _container.RegisterType<IMenuService, MainMenuService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IFileMenu, FileMenu>(new ContainerControlledLifetimeManager());

            // CREATE MENUS
            IMenuService menu = _container.Resolve<MainMenuService>();

            MenuItemNode fileMenu = (MenuItemNode)Application.Current.Resources["FileMenu"];
            MenuItemNode toolMenu = (MenuItemNode)Application.Current.Resources["ToolMenu"];
            MenuItemNode helpMenu = (MenuItemNode)Application.Current.Resources["HelpMenu"];

            menu.AddTopLevelMenu(fileMenu);
            menu.AddTopLevelMenu(toolMenu);
            menu.AddTopLevelMenu(helpMenu);

            MainMenuNode root = menu.GetMenuRoot();
            //_regionManager.Regions[RegionNames.MenuRegion].Add(root);
            _regionManager.RegisterViewWithRegion(RegionNames.MenuRegion, menu.GetMenuRoot);
        }
    }
}