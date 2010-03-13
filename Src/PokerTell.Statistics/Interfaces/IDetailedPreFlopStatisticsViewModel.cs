namespace PokerTell.Statistics.Interfaces
{
    using System.Windows.Input;

    public interface IDetailedPreFlopStatisticsViewModel : IDetailedStatisticsViewModel
    {
        ICommand InvestigateRaiseReactionCommand { get; }

        ICommand InvestigateHoleCardsCommand { get; }

        bool MayInvestigateHoleCards { get; }

        bool MayInvestigateRaise { get; }

        bool MayBrowseHands { get; }

        bool MayVisualizeHands { get; }
    }
}