using System.Threading;
using HQF.Tutorials.Prism.Infrastructure.Events;
using Prism.Events;
using Prism.Modularity;

namespace HQF.Tutorials.Prism.Module
{
    public class ModuleModule : IModule
    {
        private readonly IEventAggregator _eventAggregator;

        public ModuleModule(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Initialize()
        {
            _eventAggregator.GetEvent<MessageUpdateEvent>().Publish(new MessageUpdateEvent { Message = "Module1 was initialized ... " });
            Thread.Sleep(2000); //simulate long loading of the module
        }
    }
}