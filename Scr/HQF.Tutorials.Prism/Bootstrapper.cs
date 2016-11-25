using Microsoft.Practices.Unity;
using Prism.Unity;
using HQF.Tutorials.Prism.Views;
using System.Windows;
using HQF.Tutorials.Prism.ImageViewer;
using HQF.Tutorials.Prism.Infrastructure.Interfaces;
using HQF.Tutorials.Prism.Module;
using HQF.Tutorials.Prism.Splash;
using Prism.Modularity;

namespace HQF.Tutorials.Prism
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        //protected override void InitializeShell()
        //{
        //    Application.Current.MainWindow.Show();
        //}

        protected override void ConfigureModuleCatalog()
        {
            ModuleCatalog catalog = (ModuleCatalog)ModuleCatalog;

            //becarefull the orders
            catalog.AddModule(typeof(SplashModule));
            catalog.AddModule(typeof(ModuleModule));
            catalog.AddModule(typeof(ImageViewerModule));
        }

        protected override void ConfigureContainer()
        {
            Container.RegisterType<IShell, MainWindow>();

            base.ConfigureContainer();
        }
    }
}
