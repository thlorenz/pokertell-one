namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System.Collections;
    using System.Collections.Generic;

    using Enumerations.PokerHand;

    public interface IAquiredPokerPlayer : IPokerPlayer, IEnumerable
    {
        #region Properties

        /// <summary>
        /// Seat relative to seat of small blind
        /// </summary>
        int RelativeSeat { get; set; }

        /// <summary>
        /// Stack of Player after the hand is played
        /// Calculated by substracting  betting,raising and calling amounts and adding winning amounts
        /// for all rounds
        /// </summary>
        double StackAfter { get; }

        /// <summary>
        /// Stack of Player at the start of the hand
        /// Determined by the Parser from the Hand History
        /// </summary>
        double StackBefore { get; set; }

        /// <summary>
        /// List of all Poker Rounds for current hand Preflop Flop
        /// </summary>
        IList<IAquiredPokerRound> Rounds { get; }

        /// <summary>
        /// Number of Rounds that player saw
        /// </summary>
        int Count { get; }

        #endregion

        #region Indexers

        IAquiredPokerRound this[Streets theStreet]
        {
            get;
        }

        IAquiredPokerRound this[int index]
        {
            get;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a new Poker Round to the player
        /// </summary>
        IAquiredPokerPlayer AddRound();

        /// <summary>
        /// Add a given Poker round to the Player
        /// </summary>
        /// <param name="aquiredRound">Poker Round to add</param>
        IAquiredPokerPlayer AddRound(IAquiredPokerRound aquiredRound);

        IAquiredPokerPlayer InitializeWith(string name, double stack);

        IAquiredPokerPlayer InitializeWith(int seatNum, string holecards, long playerId);

        /// <summary>
        /// This is called from the Parser, after all players have been added.
        /// </summary>
        /// <description>
        /// Important: At this point playerCount = TotalPlayers at table allowed to play
        ///	Determines and sets the Position of the player in a Seat# x
        ///	 Result:
        ///	 SB will be 0
        ///	 BB will be 1
        ///	 etc.
        ///	 Button will be TotalPlayers -1
        /// Then it determines the strategic position (SB,BB,EA,MI) etc. by calling SetStrategicPosition
        /// </description>
        /// <param name="sbPosition">Position (Seat) of the small blind</param>
        /// <param name="playerCount">Total amount of players in the hand - empty seats not counted</param>
        /// <returns>true if all went well</returns>
        bool SetPosition(int sbPosition, int playerCount);

        #endregion
    }
}