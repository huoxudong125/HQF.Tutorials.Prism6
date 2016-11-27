using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace HQF.Tutorials.Prism.Extensions.Menus
{
	public class MenuGroupDescription : INotifyPropertyChanged, IComparable<MenuGroupDescription>, IComparable
	{
		private int sortIndex;

		public int SortIndex
		{
			get { return sortIndex; }
			set
			{
				if (sortIndex != value)
				{
					sortIndex = value;
					RaisePropertyChanged(() => SortIndex);
				}
			}
		}

		private String name;

		public String Name
		{
			get { return name; }
			set
			{
				if (name != value)
				{
					name = value;
					RaisePropertyChanged(() => this.Name);
				}
			}
		}

		public MenuGroupDescription()
		{
			Name = String.Empty;
			SortIndex = 50;
			
		}

		public override string ToString()
		{
			return Name;
		}

		#region IComparable<MenuGroupDescription> Members

		public int CompareTo(MenuGroupDescription other)
		{
			return SortIndex.CompareTo(other.SortIndex);
		}

		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			if(obj is MenuGroupDescription)
				return sortIndex.CompareTo((obj as MenuGroupDescription).SortIndex);
			return this.GetHashCode().CompareTo(obj.GetHashCode());
		}

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		private void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyExpression.Name));
		}

		#endregion
	}
}
