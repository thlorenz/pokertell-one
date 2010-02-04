namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Tools.Interfaces;

    using ViewModels.Analyzation;

    public interface IDetailedRaiseReactionStatisticsViewModel<T>
    {
        ICommand BrowseHandsCommand { get; }

        /// <summary>
        ///   Describes the situation and player of the statistics
        /// </summary>
        string StatisticsDescription { get; }

        IDetailedRaiseReactionStatisticsViewModel<T> InitializeWith(
            IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers,
            ITuple<T, T> selectedBetSizeSpan,
            string playerName,
            ActionSequences actionSequence,
            Streets street);
    }


}