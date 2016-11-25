using System;
using HQF.Tutorials.Prism.Infrastructure.Events;
using Prism.Events;
using Prism.Mvvm;

namespace HQF.Tutorials.Prism.Splash.ViewModels
{
    public class SplashScreenViewModel : BindableBase
    {
        #region Declarations

        private string _status;

        #endregion Declarations

        #region ctor

        public SplashScreenViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<MessageUpdateEvent>().Subscribe(e => UpdateMessage(e.Message));
        }

        #endregion ctor

        #region Public Properties

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        #endregion Public Properties

        #region Private Methods

        private void UpdateMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            Status += string.Concat(Environment.NewLine, message, "...");
        }

        #endregion Private Methods
    }
}