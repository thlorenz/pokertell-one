namespace PokerTell.SessionReview.ViewModels
{
    using System;
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

            ConnectToHandHistoryFilterEvents();
        }

        #endregion

        #region Properties

        public IHandHistoriesViewModel HandHistoriesViewModel
        {
            get { return _handHistoriesViewModel; }
        }

        public IHandHistoriesFilter Filter
        {
            get { return HandHistoriesViewModel.HandHistoriesFilter; }
        }

        #endregion

        #region Methods

        void ClearAllFilters()
        {
            if (Filter.ShowAll && HandHistoriesViewModel != null)
            {
                HandHistoriesViewModel.ApplyFilter(_alwaysTrueCondition);
            }
        }

        void ConnectToHandHistoryFilterEvents()
        {
            Filter.HeroNameChanged += TriggerAllFiltersThatDependOnHeroName;
            Filter.ShowAllChanged += ClearAllFilters;
            Filter.ShowMoneyInvestedChanged += FilterOutHandHistoriesWhereHeroDidNotInvestMoney;
            Filter.ShowSawFlopChanged += FilterOutHandHistoriesWhereHeroDidNotSeeTheFlop;
        }

        void FilterOutHandHistoriesWhereHeroDidNotInvestMoney()
        {
            if (Filter.ShowMoneyInvested && HandHistoriesViewModel != null)
            {
                _investedMoneyCondition.AppliesTo(Filter.HeroName);
                HandHistoriesViewModel.ApplyFilter(_investedMoneyCondition);
            }
        }

        void FilterOutHandHistoriesWhereHeroDidNotSeeTheFlop()
        {
            if (Filter.ShowSawFlop && HandHistoriesViewModel != null)
            {
                _sawFlopCondition.AppliesTo(Filter.HeroName);
                HandHistoriesViewModel.ApplyFilter(_sawFlopCondition);
            }
        }

        void TriggerAllFiltersThatDependOnHeroName()
        {
            // Trigger SelectHeroChangedEvent
            Filter.SelectHero = Filter.SelectHero;

            Console.WriteLine("HeroName: {0}", Filter.HeroName);

            FilterOutHandHistoriesWhereHeroDidNotInvestMoney();
            FilterOutHandHistoriesWhereHeroDidNotSeeTheFlop();
        }

        #endregion
    }
}