using System.Windows;
using HQF.Tutorials.Prism.Infrastructure.Interfaces;

namespace HQF.Tutorials.Prism.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IShell
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}