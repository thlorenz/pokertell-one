namespace PokerTell.SessionReview.ViewModels
{
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.WPF.ViewModels;

    public class SessionReviewSettingsViewModel : NotifyPropertyChanged
    {
        readonly IAlwaysTrueCondition _alwaysTrueCondition;

        readonly IHandHistoriesViewModel _handHistoriesViewModel;

        readonly IInvestedMoneyCondition _investedMoneyCondition;

        readonly ISawFlopCondition _sawFlopCondition;

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

        public IHandHistoriesViewModel HandHistoriesViewModel
        {
            get { return _handHistoriesViewModel; }
        }

        public IHandHistoriesFilter Filter
        {
            get { return HandHistoriesViewModel.HandHistoriesFilter; }
        }

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

            FilterOutHandHistoriesWhereHeroDidNotInvestMoney();
            FilterOutHandHistoriesWhereHeroDidNotSeeTheFlop();
        }
    }
}