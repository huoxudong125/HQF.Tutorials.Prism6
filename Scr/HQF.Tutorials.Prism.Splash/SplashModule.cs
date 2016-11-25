using System;
using System.Threading;
using System.Windows.Threading;
using HQF.Tutorials.Prism.Infrastructure.Events;
using HQF.Tutorials.Prism.Infrastructure.Interfaces;
using HQF.Tutorials.Prism.Splash.Views;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Modularity;

namespace HQF.Tutorials.Prism.Splash
{
    public class SplashModule : IModule
    {
        #region ctors

        public SplashModule(IUnityContainer container, 
            IEventAggregator eventAggregator,
            IShell shell)
        {
            Container = container;
            EventAggregator = eventAggregator;
            Shell = shell;
        }

        #endregion ctors

        #region Private Properties

        private IUnityContainer Container { get; set; }

        private IEventAggregator EventAggregator { get; set; }

        private IShell Shell { get; set; }

        private AutoResetEvent WaitForCreation { get; set; }

        #endregion Private Properties

       
        public void Initialize()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(
              (Action)(() =>
              {
                  Shell.Show();
                  EventAggregator.GetEvent<CloseSplashEvent>().Publish(new CloseSplashEvent());
              }));

            WaitForCreation = new AutoResetEvent(false);

            ThreadStart showSplash =
              () =>
              {
                  Dispatcher.CurrentDispatcher.BeginInvoke(
                    (Action)(() =>
                    {
                        Container.RegisterType<SplashScreenView>();

                        var splash = Container.Resolve<SplashScreenView>();
                        EventAggregator.GetEvent<CloseSplashEvent>().Subscribe(
                                        e => splash.Dispatcher.BeginInvoke((Action)splash.Close),
                                        ThreadOption.PublisherThread, true);

                        splash.Show();

                        WaitForCreation.Set();
                    }));

                  Dispatcher.Run();
              };

            var thread = new Thread(showSplash) { Name = "Splash Thread", IsBackground = true };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            WaitForCreation.WaitOne();
        }
    }
}