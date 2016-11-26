using Prism.Modularity;
using Prism.Regions;
using System;

namespace HQF.Tutorials.Prism.ImageEditor
{
    public class ImageEditorModule : IModule
    {
        IRegionManager _regionManager;

        public ImageEditorModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            //throw new NotImplementedException();
        }
    }
}