using System;

namespace HQF.Tutorials.Prism.Infrastructure.Events
{
	public class ApplicationExitEventArgs : EventArgs
	{
		public bool Cancel
		{
			get;
			set;
		}

		public ApplicationExitEventArgs()
		{

		}
	}
}
