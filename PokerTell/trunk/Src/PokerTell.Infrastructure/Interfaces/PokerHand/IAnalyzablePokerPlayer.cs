namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using Enumerations.PokerHand;

    public interface IAnalyzablePokerPlayer
    {
        int HandId { get; set; }

        /// <summary>
        /// Is player in position on Flop, Turn or River?
        /// </summary>
        bool?[] InPosition { get; }

        /// <summary>
        /// M of player at the start of the hand
        /// </summary>
        int MBefore { get; set; }

        /// <summary>
        /// Position of Player in relation to Button (SB - BU)
        /// </summary>
        StrategicPositions StrategicPosition { get; set; }

        ActionSequences[] ActionSequences { get; set; }

        int[] BetSizeIndexes { get; }

        long Id { get; set; }
    }
}