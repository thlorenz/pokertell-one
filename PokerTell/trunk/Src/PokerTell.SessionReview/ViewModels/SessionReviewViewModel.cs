namespace PokerTell.SessionReview.ViewModels
{
    using System;
    using System.Reflection;

    using log4net;

    using Microsoft.Practices.Composite;
    using Microsoft.Practices.Composite.Presentation.Commands;

    internal class SessionReviewViewModel : IActiveAware, ISessionReviewViewModel
    {
        #region Constants and Fields

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        bool _isActive;

        DelegateCommand<object> _saveCommand;

        #endregion

        #region Constructors and Destructors

        public SessionReviewViewModel()
        {
            Commands.SaveSessionReviewCommand.RegisterCommand(SaveCommand);
            
        }

        #endregion

        #region Events

        public event EventHandler IsActiveChanged = delegate { };

        #endregion

        #region Properties

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                SaveCommand.IsActive = value;
            }
        }

        public DelegateCommand<object> SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new DelegateCommand<object>(Save, CanSave) { IsActive = true };
                }

                return _saveCommand;
            }
        }

        #endregion

        #region Public Methods

        public bool CanSave(object arg)
        {
            return IsActive;
        }

        public void Save(object arg)
        {
            Log.Info("SessionReview->Saving");
        }

        #endregion
    }
}