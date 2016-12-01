using System;
using System.IO;
using System.Windows;
using HQF.Tutorials.Prism.Extensions.Catalogs;
using HQF.Tutorials.Prism.ImageViewer;
using HQF.Tutorials.Prism.Infrastructure.Interfaces;
using HQF.Tutorials.Prism.MainMenu;
using HQF.Tutorials.Prism.Module;
using HQF.Tutorials.Prism.Splash;
using HQF.Tutorials.Prism.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Unity;

namespace HQF.Tutorials.Prism
{
    internal class Bootstrapper : UnityBootstrapper
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
            var catalog = (ModuleCatalog) ModuleCatalog;

            //be careful the orders
            
            catalog.AddModule(typeof(SplashModule));
            catalog.AddModule(typeof(ModuleModule));
            catalog.AddModule(typeof(MainMenuModule));
            catalog.AddModule(typeof(ImageViewerModule));
        }

        //protected override IModuleCatalog CreateModuleCatalog()
        //{
            
        //    var catalog =
        //        new DynamicDirectoryModuleCatalog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modules"));
        //    return catalog;
        //}

        protected override void ConfigureContainer()
        {
            Container.RegisterType<IShell, MainWindow>();

            base.ConfigureContainer();
        }
    }
}