using Prism.Modularity;
using Prism.Regions;
using System;
using System.Threading;
using HQF.Tutorials.Prism.Infrastructure.Events;
using Prism.Events;

namespace HQF.Tutorials.Prism.Module
{
    public class ModuleModule : IModule
    {
        IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        public ModuleModule(RegionManager regionManager,IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
        }

        public void Initialize()
        {
            _eventAggregator.GetEvent<MessageUpdateEvent>().Publish(new MessageUpdateEvent { Message = "Module1 was initialized ... " });
            Thread.Sleep(2000); //simulate long loading of the module
        }
    }
}