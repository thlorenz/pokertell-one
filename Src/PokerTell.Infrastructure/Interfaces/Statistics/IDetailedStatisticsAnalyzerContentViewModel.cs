namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;

    public interface IDetailedStatisticsAnalyzerContentViewModel
    {
        event Action<IDetailedStatisticsAnalyzerContentViewModel> ChildViewModelChanged;

        bool ShowAsPopup { get; }

        bool MayInvestigateHoleCards { get; }

        bool MayInvestigateRaise { get; }

        bool MayBrowseHands { get; }

        bool MayVisualizeHands { get; }

        bool MayInvestigate { get; }

        void RaiseChildViewModelChanged(IDetailedStatisticsAnalyzerContentViewModel childViewModel);
    }
}