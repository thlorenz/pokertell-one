namespace PokerTell.Statistics.Interfaces
{
    using System.Windows.Input;

    public interface IDetailedPreFlopStatisticsViewModel : IDetailedStatisticsViewModel
    {
        ICommand InvestigateRaiseReactionCommand { get; }

        ICommand InvestigateHoleCardsCommand { get; }
    }
}