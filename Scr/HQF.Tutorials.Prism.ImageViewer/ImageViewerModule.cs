using HQF.Tutorials.Prism.Infrastructure.Constants;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace HQF.Tutorials.Prism.ImageViewer
{
    public class ImageViewerModule : IModule
    {
        private IRegionManager _regionManager;
        private readonly IUnityContainer _unityContainer;

        public ImageViewerModule(RegionManager regionManager,
            IUnityContainer unityContainer)
        {
            _regionManager = regionManager;
            _unityContainer = unityContainer;
        }

        public void Initialize()
        {
            _unityContainer.RegisterType<Views.ImageViewerView>();
            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(Views.ImageViewerView));
        }

    }
}