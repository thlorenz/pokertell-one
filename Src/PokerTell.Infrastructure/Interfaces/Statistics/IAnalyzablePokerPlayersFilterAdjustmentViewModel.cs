namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;
    using System.Windows.Input;

    public interface IAnalyzablePokerPlayersFilterAdjustmentViewModel
    {
        ICommand ApplyFilterToAllCommand { get; }

        ICommand ApplyFilterToPlayerCommand { get; }

        IAnalyzablePokerPlayersFilterViewModel Filter { get; }

        string PlayerName { get; }

        bool ShowApplyToAllCommand { get; }

        IAnalyzablePokerPlayersFilterAdjustmentViewModel InitializeWith(
            string playerName, 
            IAnalyzablePokerPlayersFilter currentFilter, 
            Action<string, IAnalyzablePokerPlayersFilter> applyTo, 
            Action<IAnalyzablePokerPlayersFilter> applyToAll);
    }
}