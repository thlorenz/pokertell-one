namespace PokerTell.SessionReview.ViewModels
{
    using System.Reflection;

    using log4net;

    using Microsoft.Practices.Composite.Presentation.Commands;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.WPF.ViewModels;

    internal class SessionReviewViewModel : ItemsRegionViewModel, ISessionReviewViewModel
    {
        #region Constants and Fields

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IHandHistoriesViewModel _handHistoriesViewModel;

        DelegateCommand<object> _saveCommand;

        #endregion

        #region Constructors and Destructors

        public SessionReviewViewModel(IHandHistoriesViewModel handHistoriesViewModel)
        {
            _handHistoriesViewModel = handHistoriesViewModel;

            _handHistoriesViewModel.ShowSelectOption = true;

            Commands.SaveSessionReviewCommand.RegisterCommand(SaveCommand);
            
            HeaderInfo = "SessionReview " + GetHashCode() + " for: " + _handHistoriesViewModel.GetHashCode();
        }

        #endregion

        #region Properties

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

        public IHandHistoriesViewModel HandHistoriesViewModel
        {
            get { return _handHistoriesViewModel; }
        }

        #endregion

        #region Public Methods

        public bool CanSave(object arg)
        {
            return IsActive;
        }

        public void Save(object arg)
        {
            Log.InfoFormat("SessionReview->Saving: {0}", GetHashCode());
        }

        #endregion

        #region Methods

        protected override void OnIsActiveChanged()
        {
            SaveCommand.IsActive = IsActive;
        }

        #endregion
    }
}