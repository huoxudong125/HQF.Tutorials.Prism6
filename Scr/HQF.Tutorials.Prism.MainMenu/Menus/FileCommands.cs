using System.Windows;
using HQF.Tutorials.Prism.Infrastructure.Events;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Events;
using Microsoft.Win32;

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
            //var events = (IEventAggregator)ServiceLocator.Current.GetService(typeof(IEventAggregator));
            //events.GetEvent<FileOpenEvent>().Publish("Lighthouse.jpg");
            FileDialog dialog=new OpenFileDialog();
            dialog.Filter = "图像文件(*.bmp, *.jpg)|*.bmp;*.jpg|所有文件(*.*)|*.*";
            dialog.CheckFileExists = true;
            dialog.ShowDialog();


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