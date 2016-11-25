using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace HQF.Tutorials.Prism.ImageViewer.ViewModels
{
    public class ImageViewerViewModel : BindableBase
    {
        private ObservableCollection<FileInfo> _fileInfos = new ObservableCollection<FileInfo>();
        private ICollectionView _fileInfoCollections;

        public ICollectionView FileCollection
        {
            get { return _fileInfoCollections; }
        }

        public ICommand OpenCommand { get; set; }

        public ImageViewerViewModel()
        {
            _fileInfoCollections = new CollectionView(_fileInfos);
            //OpenCommand=new DelegateCommand<string>(OpenCommandHandler);
            GetImages();
        }

        //private void OpenCommandHandler(string obj)
        //{
        //    var dlg = new CommonOpenFileDialog();
        //    dlg.Title = "My Title";
        //    dlg.IsFolderPicker = true;
        //    dlg.InitialDirectory = currentDirectory;

        //    dlg.AddToMostRecentlyUsedList = false;
        //    dlg.AllowNonFileSystemItems = false;
        //    dlg.DefaultDirectory = currentDirectory;
        //    dlg.EnsureFileExists = true;
        //    dlg.EnsurePathExists = true;
        //    dlg.EnsureReadOnly = false;
        //    dlg.EnsureValidNames = true;
        //    dlg.Multiselect = false;
        //    dlg.ShowPlacesList = true;

        //    if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
        //    {
        //        var folder = dlg.FileName;
        //        // Do something with selected folder string
        //    }

        //}

        private void GetImages()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            DirectoryInfo di = new DirectoryInfo(path); // Image mask

            string[] extensions = new string[] { "*.jpg", "*.png", "*.gif" };
            foreach (string extension in extensions)
            {
                foreach (FileInfo fi in di.GetFiles(extension))
                {
                    _fileInfos.Add(fi);
                }
            }
        }
    }
}