namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;

    public interface IDetailedStatisticsAnalyzerContentViewModel
    {
        event Action<IDetailedStatisticsAnalyzerContentViewModel> ChildViewModelChanged;

        bool ShowAsPopup { get; }
    }
}