namespace PokerTell.SessionReview.ViewModels
{
    using System.Reflection;

    using log4net;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.WPF.ViewModels;

    public class SessionReviewSettingsViewModel : ViewModel
    {
        #region Constants and Fields

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IAlwaysTrueCondition _alwaysTrueCondition;

        readonly IHandHistoriesViewModel _handHistoriesViewModel;

        readonly IInvestedMoneyCondition _investedMoneyCondition;

        readonly ISawFlopCondition _sawFlopCondition;

        string _heroName;

        bool _showAll;

        bool _showMoneyInvested;

        bool _showSawFlop;

        bool _showSelectedOnly;

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
            HeroName = "hero";
            ShowAll = true;
        }

        #endregion

        #region Properties

        public string HeroName
        {
            get { return _heroName; }
            set
            {
                _heroName = value;
                RaisePropertyChanged(() => HeroName);
            }
        }

        public bool ShowAll
        {
            get { return _showAll; }
            set
            {
                _showAll = value;
                if (_showAll && _handHistoriesViewModel != null)
                {
                    _handHistoriesViewModel.ApplyFilterCompositeAction.Invoke(_alwaysTrueCondition);
                }
            }
        }

        public bool ShowMoneyInvested
        {
            get { return _showMoneyInvested; }
            set
            {
                _showMoneyInvested = value;
                if (_showMoneyInvested && _handHistoriesViewModel != null)
                {
                    _investedMoneyCondition.AppliesTo(HeroName);
                    _handHistoriesViewModel.ApplyFilterCompositeAction.Invoke(_investedMoneyCondition);
                }
            }
        }

        public bool ShowPreflopFolds
        {
            get { return _handHistoriesViewModel.ShowPreflopFolds; }
            set { _handHistoriesViewModel.ShowPreflopFolds = value; }
        }

        public bool ShowSawFlop
        {
            get { return _showSawFlop; }
            set
            {
                _showSawFlop = value;
                if (_showSawFlop && _handHistoriesViewModel != null)
                {
                    _sawFlopCondition.AppliesTo(HeroName);
                    _handHistoriesViewModel.ApplyFilterCompositeAction.Invoke(_sawFlopCondition);
                }
            }
        }

        public bool ShowSelectedOnly
        {
            get { return _showSelectedOnly; }
            set
            {
                _showSelectedOnly = value;

                if (_handHistoriesViewModel != null)
                {
                    _handHistoriesViewModel.ShowSelectedOnly = value;
                }

                RaisePropertyChanged(() => ShowSelectedOnly);
            }
        }

        #endregion
    }
}