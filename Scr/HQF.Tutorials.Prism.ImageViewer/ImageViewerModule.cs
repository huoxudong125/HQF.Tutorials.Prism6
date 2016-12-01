using System.Windows;
using HQF.Tutorials.Prism.Extensions.Menus;
using HQF.Tutorials.Prism.ImageViewer.Views;
using HQF.Tutorials.Prism.Infrastructure.Constants;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;

namespace HQF.Tutorials.Prism.ImageViewer
{
    public class ImageViewerModule : IModule
    {
        private IRegionManager _regionManager;
        private readonly IUnityContainer _unityContainer;
        private readonly IMenuService _menuService;
        private ImageViewerView _view;

        public ImageViewerModule(RegionManager regionManager,
            IUnityContainer unityContainer, IMenuService menuService)
        {
            _regionManager = regionManager;
            _unityContainer = unityContainer;
            this._menuService = menuService;
        }

        public void Initialize()
        {
            _unityContainer.RegisterType<Views.ImageViewerView>();
           
            if (_menuService != null)
                AddMenuItems();
            else
                _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(Views.ImageViewerView));

        }

        private void AddMenuItems()
        {

            // You would need to put useful commands in here too
            _menuService.RegisterMenu(new MenuItemNode(DefaultTopLevelMenuNames.Tools)
            {
                Text = "Tool1",
                Command = new DelegateCommand(ShowImageViewerView)
            });

            // I'll have to think about a way to ignore the mnemonic specifier '_' in the RegisterMenu method, probably just remove all _ before tokenization / comparison
            _menuService.RegisterMenu(new MenuItemNode(DefaultTopLevelMenuNames.File + "/_Open")
            {
                Text = "_Project"
            });
        }

        private void ShowImageViewerView()
        {
            if (_view == null)
            {
                _view=_unityContainer.Resolve<ImageViewerView>();
                _view.HorizontalAlignment=HorizontalAlignment.Stretch;
                _view.VerticalAlignment=VerticalAlignment.Stretch;
                _view.Width = 800;
                _view.Height = 600;
                _regionManager.AddToRegion(RegionNames.ContentRegion, _view);
            }

            _regionManager.Regions[RegionNames.ContentRegion].Activate(_view);
        }
    }
}