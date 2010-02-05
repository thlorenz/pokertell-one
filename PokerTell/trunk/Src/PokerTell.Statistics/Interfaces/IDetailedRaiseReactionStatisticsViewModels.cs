namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Tools.Interfaces;

    /// <summary>
    /// Used when hero bet (acted first e.g. HeroB) and then reacted to a raise
    /// </summary>
    public interface IPostFlopHeroActsRaiseReactionStatisticsViewModel : IDetailedRaiseReactionStatisticsViewModel<double>
    {
    }

    /// <summary>
    /// Used when hero raised or check-raised (reacted e.g. OppBHeroR, HeroXOppBHeroR) and then reacted to a reraise 
    /// </summary>
    public interface IPostFlopHeroReactsRaiseReactionStatisticsViewModel : IDetailedRaiseReactionStatisticsViewModel<double>
    {
    }

    /// <summary>
    /// Used for preflop raised and unraised pot situations where the hero raised (either first or as a reraise) and was reraised
    /// </summary>
    public interface IPreFlopRaiseReactionStatisticsViewModel : IDetailedRaiseReactionStatisticsViewModel<StrategicPositions>
    {
    }

    public interface IDetailedRaiseReactionStatisticsViewModel<T> : IStatisticsTableViewModel
    {
        ICommand BrowseHandsCommand { get; }

        IDetailedRaiseReactionStatisticsViewModel<T> InitializeWith(
            IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers,
            ITuple<T, T> selectedRatioSizeSpan,
            string playerName,
            ActionSequences actionSequence,
            Streets street);
    }


}