using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HQF.Tutorials.Prism.MainMenu.Menus
{
    public static class MenuGroupStyleSelectorProxy
    {
        public static GroupStyleSelector MenuGroupStyleSelector { get; private set; }

        private static GroupStyle Style { get; set; }

        static MenuGroupStyleSelectorProxy()
        {
            MenuGroupStyleSelector = new GroupStyleSelector(SelectGroupStyle);
            Style = new GroupStyle()
            {
                HeaderTemplate = (DataTemplate)Application.Current.Resources["GroupHeaderTemplate"]
            };
        }

        public static GroupStyle SelectGroupStyle(CollectionViewGroup grp, int target)
        {
            return Style;
        }
    }
}