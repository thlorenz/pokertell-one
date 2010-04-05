namespace PokerTell.Statistics.Interfaces
{
    using System;
    using System.Windows.Input;

    using Infrastructure.Interfaces.Statistics;

    using ViewModels;

    public interface IAnalyzablePokerPlayersFilterAdjustmentViewModel
    {
        ICommand ApplyFilterToAllCommand { get; }

        ICommand ApplyFilterToPlayerCommand { get; }

        IAnalyzablePokerPlayersFilterViewModel Filter { get; }

        string PlayerName { get; }

        IAnalyzablePokerPlayersFilterAdjustmentViewModel InitializeWith(string playerName, IAnalyzablePokerPlayersFilter currentFilter, Action<string, IAnalyzablePokerPlayersFilter> applyTo, Action<IAnalyzablePokerPlayersFilter> applyToAll);
    }
}