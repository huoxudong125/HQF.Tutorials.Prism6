using System.Windows.Input;
using HQF.Tutorials.Prism.Infrastructure.Interfaces;
using Prism.Commands;
using Prism.Regions;

namespace HQF.Tutorials.Prism.MainMenu.Menus
{
    public class FileMenu : IFileMenu
    {
        // Here's an example of my menu
        // Basically any class/viewmodel can resolve IFileMenu and add to its composite commands
        // But the FileCommands are static so the style can use it

        #region IFileMenu Members

        public CompositeCommand SaveCommand
        {
            get
            {
                return FileCommands.SaveCommand;
            }
        }

        public CompositeCommand PrintCommand
        {
            get
            {
                return FileCommands.PrintCommand;
            }
        }

        public ICommand OpenFile
        {
            get;
            private set;
        }

        #endregion IFileMenu Members

        public FileMenu(IRegionManager regionManager)
        {
            OpenFile = new DelegateCommand(OpenFileCommand);
        }

        #region IFileMenu Members

        private void OpenFileCommand()
        {
            FileCommands.OpenFileCommand.Execute();
        }

        #endregion IFileMenu Members
    }
}