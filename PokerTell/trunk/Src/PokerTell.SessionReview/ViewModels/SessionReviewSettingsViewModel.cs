namespace PokerTell.SessionReview.ViewModels
{
    using System.Reflection;

    using log4net;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.WPF.ViewModels;

    public class SessionReviewSettingsViewModel : NotifyPropertyChanged
    {
        #region Constants and Fields

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IAlwaysTrueCondition _alwaysTrueCondition;

        readonly IHandHistoriesViewModel _handHistoriesViewModel;

        readonly IInvestedMoneyCondition _investedMoneyCondition;

        readonly ISawFlopCondition _sawFlopCondition;

        #endregion

        #region Constructors and Destructors

        public SessionReviewSettingsViewModel(
            IHandHistoriesViewModel handHistoriesViewModel, 
            IInvestedMoneyCondition investedMoneyCondition, 
            ISawFlopCondition sawFlopCondition, 
            IAlwaysTrueCondition alwaysTrueCondition)
        {
            _alwaysTrueCondition = alwaysTrueCondition;
            _sawFlopCondition = sawFlopCondition;
            _investedMoneyCondition = investedMoneyCondition;

            _handHistoriesViewModel = handHistoriesViewModel;
            HandHistoriesViewModel.ShowSelectOption = true;

            HandHistoriesViewModel.HandHistoriesFilter.SelectHero = true;

            ConnectToHandHistoryFilterEvents();
        }

        #endregion

        #region Properties

        public IHandHistoriesViewModel HandHistoriesViewModel
        {
            get { return _handHistoriesViewModel; }
        }

        #endregion

        #region Methods

        void ConnectToHandHistoryFilterEvents()
        {
            HandHistoriesViewModel.HandHistoriesFilter.HeroNameChanged += OnHeroNameChanged;
            HandHistoriesViewModel.HandHistoriesFilter.ShowAllChanged += OnShowAllChanged;
            HandHistoriesViewModel.HandHistoriesFilter.ShowMoneyInvestedChanged += OnShowMoneyInvestedChanged;
            HandHistoriesViewModel.HandHistoriesFilter.ShowSawFlopChanged += OnSawFlopChanged;
        }

        void OnHeroNameChanged()
        {
            // Trigger SelectHeroChangedEvent
            HandHistoriesViewModel.HandHistoriesFilter.SelectHero =
                HandHistoriesViewModel.HandHistoriesFilter.SelectHero;

            OnShowMoneyInvestedChanged();
            OnSawFlopChanged();
        }

        void OnSawFlopChanged()
        {
            if (HandHistoriesViewModel.HandHistoriesFilter.ShowSawFlop && HandHistoriesViewModel != null)
            {
                _sawFlopCondition.AppliesTo(_handHistoriesViewModel.HandHistoriesFilter.HeroName);
                HandHistoriesViewModel.ApplyFilter(_sawFlopCondition);
            }
        }

        void OnShowAllChanged()
        {
            if (HandHistoriesViewModel.HandHistoriesFilter.ShowAll && HandHistoriesViewModel != null)
            {
                HandHistoriesViewModel.ApplyFilter(_alwaysTrueCondition);
            }
        }

        void OnShowMoneyInvestedChanged()
        {
            if (HandHistoriesViewModel.HandHistoriesFilter.ShowMoneyInvested && HandHistoriesViewModel != null)
            {
                _investedMoneyCondition.AppliesTo(_handHistoriesViewModel.HandHistoriesFilter.HeroName);
                HandHistoriesViewModel.ApplyFilter(_investedMoneyCondition);
            }
        }

        #endregion
    }
}