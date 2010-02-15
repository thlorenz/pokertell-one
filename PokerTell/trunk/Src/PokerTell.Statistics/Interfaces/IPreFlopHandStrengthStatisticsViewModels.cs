

namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    public interface IPreFlopUnraisedPotCallingHandStrengthStatisticsViewModel : IPreFlopHandStrengthStatisticsViewModel
    {
    }

    public interface IPreFlopRaisedPotCallingHandStrengthStatisticsViewModel : IPreFlopHandStrengthStatisticsViewModel
    {
    }

    public interface IPreFlopRaisingHandStrengthStatisticsViewModel : IPreFlopHandStrengthStatisticsViewModel
    {
    }

    public interface IPreFlopHandStrengthStatisticsViewModel : IStatisticsTableViewModel
    {
        void InitializeWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers, ActionSequences actionSequence);
    }
}