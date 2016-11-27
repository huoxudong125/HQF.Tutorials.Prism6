using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Data;
using HQF.Tutorials.Prism.Extensions.Menus;

namespace HQF.Tutorials.Prism.MainMenu.Menus
{
    [ValueConversion(typeof(MenuItemNodeCollection), typeof(IEnumerable))]
    public class MenuCollectionViewConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(IEnumerable))
                throw new NotImplementedException();

            CollectionViewSource src = new CollectionViewSource();
            src.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
            src.SortDescriptions.Add(new SortDescription("Group", ListSortDirection.Ascending));
            src.SortDescriptions.Add(new SortDescription("SortIndex", ListSortDirection.Ascending));
            src.Source = value as IEnumerable;
            return src.View;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.GetType() != typeof(CollectionViewSource))
                throw new NotImplementedException();
            return (value as CollectionViewSource).Source;
        }

        #endregion IValueConverter Members
    }
}