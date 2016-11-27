using System.Windows;
using HQF.Tutorials.Prism.Infrastructure.Events;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Events;

namespace HQF.Tutorials.Prism.MainMenu.Menus
{
    public class FileCommands
    {
        public static DelegateCommand OpenFileCommand
        {
            get;
            private set;
        }

        public static CompositeCommand SaveCommand
        {
            get;
            private set;
        }

        public static CompositeCommand ExportCommand
        {
            get;
            private set;
        }

        public static CompositeCommand PrintCommand
        {
            get;
            private set;
        }

        public static DelegateCommand ExitApplicationCommand
        {
            get;
            private set;
        }

        public static DelegateCommand AboutCommand
        {
            get;
            private set;
        }

        static FileCommands()
        {
            OpenFileCommand = new DelegateCommand(OpenFile);
            SaveCommand = new CompositeCommand(true);
            PrintCommand = new CompositeCommand(true);
            ExitApplicationCommand = new DelegateCommand(ExitApplication);
        }

        private static void OpenFile()
        {
            var events = (IEventAggregator)ServiceLocator.Current.GetService(typeof(IEventAggregator));
            events.GetEvent<FileOpenEvent>().Publish("Lighthouse.jpg");
        }

        private static void ExitApplication()
        {
            var events = (IEventAggregator)ServiceLocator.Current.GetService(typeof(IEventAggregator));
            var args = new ApplicationExitEventArgs();
            events.GetEvent<ApplicationExitEvent>().Publish(args);

            if (!args.Cancel)
                Application.Current.Shutdown();
        }
    }
}