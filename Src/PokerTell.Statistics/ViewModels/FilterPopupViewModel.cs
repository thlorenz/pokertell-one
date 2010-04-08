namespace PokerTell.Statistics.ViewModels
{
    using System;

    using PokerTell.Infrastructure.Interfaces.Statistics;

    using Tools.WPF.ViewModels;

    public class FilterPopupViewModel : NotifyPropertyChanged, IFilterPopupViewModel
    {
        bool _show;

        public FilterPopupViewModel(IAnalyzablePokerPlayersFilterAdjustmentViewModel filter)
        {
            Filter = filter;
        }

        public IAnalyzablePokerPlayersFilterAdjustmentViewModel Filter { get; set; }

        public bool Show
        {
            get { return _show; }
            set
            {
                _show = value;
                RaisePropertyChanged(() => Show);
            }
        }

        public void ShowFilter(
            string playerName, 
            IAnalyzablePokerPlayersFilter currentFilter, 
            Action<string, IAnalyzablePokerPlayersFilter> applyTo)
        {
            ShowFilter(playerName, currentFilter, applyTo, null);
        }

        public void ShowFilter(
            string playerName, 
            IAnalyzablePokerPlayersFilter currentFilter, 
            Action<string, IAnalyzablePokerPlayersFilter> applyTo, 
            Action<IAnalyzablePokerPlayersFilter> applyToAll)
        {
            Filter.InitializeWith(playerName, currentFilter, applyTo, applyToAll);

            Show = true;
        }
    }
}