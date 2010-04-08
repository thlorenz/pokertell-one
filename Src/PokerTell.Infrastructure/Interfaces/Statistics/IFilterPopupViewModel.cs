namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;

    public interface IFilterPopupViewModel
    {
        IAnalyzablePokerPlayersFilterAdjustmentViewModel Filter { get; set; }

        bool Show { get; set; }

        void ShowFilter(
            string playerName, 
            IAnalyzablePokerPlayersFilter currentFilter, 
            Action<string, IAnalyzablePokerPlayersFilter> applyTo, 
            Action<IAnalyzablePokerPlayersFilter> applyToAll);

        void ShowFilter(
            string playerName, 
            IAnalyzablePokerPlayersFilter currentFilter, 
            Action<string, IAnalyzablePokerPlayersFilter> applyTo);
    }
}