using Prism.Modularity;
using Prism.Regions;
using System;

namespace HQF.Tutorials.Prism.ModuleTracker
{
    public class ModuleTrackerModule : IModule
    {
        IRegionManager _regionManager;

        public ModuleTrackerModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}