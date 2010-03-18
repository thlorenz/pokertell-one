namespace PokerTell.Statistics.Interfaces
{
    using System.Windows.Input;

    public interface IDetailedPostFlopHeroActsStatisticsViewModel : IDetailedStatisticsViewModel
    {
        ICommand InvestigateRaiseReactionCommand { get; }
    }
}