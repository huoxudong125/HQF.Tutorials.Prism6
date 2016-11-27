using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HQF.Tutorials.Prism.Extensions.Menus
{
    public class MenuItemNodeCollection : ObservableCollection<MenuItemNode>
    {
        public MenuItemNodeCollection()
        {
        }

        public MenuItemNodeCollection(IEnumerable<MenuItemNode> items) : base(items)
        {
        }
    }
}