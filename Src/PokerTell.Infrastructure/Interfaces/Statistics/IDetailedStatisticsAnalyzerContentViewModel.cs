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

        /// <summary>
        /// Allows reacting to scrolling events e.g. MouseWheel.
        /// Not all implementers will implement this method.
        /// </summary>
        /// <param name="change">Sign indicates scrolling direction, value indicates size of the scroll</param>
        void Scroll(int change);

        void RaiseChildViewModelChanged(IDetailedStatisticsAnalyzerContentViewModel childViewModel);
    }
}