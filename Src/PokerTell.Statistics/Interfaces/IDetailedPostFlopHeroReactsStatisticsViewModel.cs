namespace PokerTell.Statistics.Interfaces
{
    using System.Windows.Input;

    public interface IDetailedPostFlopHeroReactsStatisticsViewModel : IDetailedStatisticsViewModel
    {
        ICommand InvestigateRaiseReactionCommand { get; }
    }
}