using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;

namespace HQF.Tutorials.Prism.Infrastructure.Interfaces
{
    public interface IFileMenu
    {
        ICommand OpenFile
        {
            get;
        }

        CompositeCommand SaveCommand
        {
            get;
        }

        CompositeCommand PrintCommand
        {
            get;
        }
    }
}
