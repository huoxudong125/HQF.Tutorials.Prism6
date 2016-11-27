using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Input;
using System.Windows.Markup;

namespace HQF.Tutorials.Prism.Extensions.Menus
{
	[ContentProperty("Children")]
	public class MenuItemNode : INotifyPropertyChanged
	{
		private string text;
		private ICommand command;
		private Uri imageSource;
		private int sortIndex;

		public MenuItemNode()
		{
			Children = new MenuItemNodeCollection();
			SortIndex = 50;
		}

		public MenuItemNode(String path)
		{
			Children = new MenuItemNodeCollection();
			SortIndex = 50;
			Path = path;
		}

		public MenuItemNodeCollection Children { get; private set; }

		public ICommand Command
		{
			get
			{
				return command;
			}
			set
			{
				if (command != value)
				{
					command = value;
					RaisePropertyChanged(() => this.Command);
				}
			}
		}

		public Uri ImageSource
		{
			get
			{
				return imageSource;
			}
			set
			{
				if (imageSource != value)
				{
					imageSource = value;
					RaisePropertyChanged(() => this.ImageSource);
				}
			}
		}

		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				if (text != value)
				{
					text = value;
					RaisePropertyChanged(() => this.Text);
				}
			}
		}

		private MenuGroupDescription group;

		public MenuGroupDescription Group
		{
			get { return group; }
			set
			{
				if (group != value)
				{
					group = value;
					RaisePropertyChanged(() => this.Group);
				}
			}
		}

		public int SortIndex
		{
			get
			{
				return sortIndex;
			}
			set
			{
				if (sortIndex != value)
				{
					sortIndex = value;
					RaisePropertyChanged(() => this.SortIndex);
				}
			}
		}

		public string Path
		{
			get;
			private set;
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		// I implemented this because I did not want my WPF library to have a dependency of Prism
		// similar to Prism's Notification object
		private void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyExpression.Name));
		}

		#endregion
	}
}
