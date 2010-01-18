namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System;

    using Enumerations.PokerHand;
    
    /// <summary>
    /// Summarizes the situation and actions of a PokerPlayer during a hand
    /// </summary>
    public interface IAnalyzablePokerPlayer
    {
        int HandId { get; }

        /// <summary>
        /// Is player in position on Flop, Turn or River?
        /// </summary>
        bool?[] InPosition { get; }

        /// <summary>
        /// M of player at the start of the hand
        /// </summary>
        int MBefore { get; }

        /// <summary>
        /// Position: SB=0, BB=1, Button=totalplrs (-1 when yet unknown)
        /// </summary>
        int Position { get; }

        /// <summary>
        /// Position of Player in relation to Button (SB - BU)
        /// </summary>
        StrategicPositions StrategicPosition { get; }

        ActionSequences[] ActionSequences { get; }

        int[] BetSizeIndexes { get; }

        long Id { get; }

        /// <summary>
        /// The Big Blind
        /// </summary>
        double BB { get; }

        double Ante { get; set; }

        /// <summary>
        /// Date and Time when the hand occured
        /// </summary>
        DateTime TimeStamp { get; }

        /// <summary>
        /// Total Players that acted in the hand
        /// </summary>
        int TotalPlayers { get; }

        /// <summary>
        /// List of all PokerRound Sequences for current hand Preflop Flop
        /// </summary>
        IConvertedPokerRound[] Sequences { get; }

        string Holecards { get; }

        int PlayersInFlop { get; set; }
    }
}