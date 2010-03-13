namespace PokerTell.Statistics.Interfaces
{
    using System.Windows.Input;

    public interface IDetailedPostFlopHeroReactsStatisticsViewModel : IDetailedStatisticsViewModel
    {
        ICommand InvestigateRaiseReactionCommand { get; }

        bool MayInvestigateHoleCards { get; }

        bool MayInvestigateRaise { get; }

        bool MayBrowseHands { get; }

        bool MayVisualizeHands { get; }
    }
}