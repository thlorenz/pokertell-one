namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System.Collections;
    using System.Collections.Generic;

    using Enumerations.PokerHand;

    public interface IConvertedPokerPlayer : IPokerPlayer, IEnumerable
    {
        /// <summary>
        /// Is player in position on Flop, Turn or River? 0 = yes, 1 = no
        /// </summary>
        int[] InPosition { get; }

        /// <summary>
        /// M of player after the hand is over
        /// </summary>
        int MAfter { get; set; }

        /// <summary>
        /// M of player at the start of the hand
        /// </summary>
        int MBefore { get; set; }

        /// <summary>
        /// Position, that a preflop raise came from, before player could act
        /// </summary>
        int PreflopRaiseInFrontPos { get; }

        /// <summary>
        /// Contains Sequence strings for each Round of the Player
        /// representing the way he acted or reacted to opponents actions
        /// Sometimes referred to as the 'line' and is used to determine bettin patterns
        /// during statistical analysis
        /// </summary>
        string[] Sequence { get; }

        /// <summary>
        /// Position of Player in relation to Button (SB - BU)
        /// </summary>
        StrategicPositions StrategicPosition { get; }

        /// <summary>
        /// List of all Poker Rounds for current hand Preflop Flop
        /// </summary>
        IList<IConvertedPokerRound> Rounds { get; }

        /// <summary>
        /// Number of Rounds that player saw
        /// </summary>
        int Count { get; }

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        IConvertedPokerRound this[int index]
        {
            get;
        }

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="theStreet">
        /// The the street.
        /// </param>
        IConvertedPokerRound this[Streets theStreet]
        {
            get;
        }

        /// <summary>
        /// Add a new Poker Round to the player
        /// </summary>
        IConvertedPokerPlayer AddRound();

        /// <summary>
        /// Add a given Poker round to the Player
        /// </summary>
        /// <param name="convertedRound">
        /// The converted Round.
        /// </param>
        IConvertedPokerPlayer AddRound(IConvertedPokerRound convertedRound);

        IConvertedPokerPlayer InitializeWith(string name, double mBefore, double mAfter, int positionNum, int totalPlayers, string holecards);

        /// <summary>
        /// Determines a Sequence string, representing, what the player did in a round
        /// </summary>
        /// <param name="currentSequence">
        /// Actions affecting us that happened so far
        /// </param>
        /// <param name="myNextAction">
        /// What am I about to do
        /// </param>
        /// <param name="street">
        /// Preflop,Flop, Turn, etc.
        /// </param>
        /// <para>
        /// Sequences are of the following format: 
        /// Relevant actions are listed in order. Action.What abreviations are used, except for bet, in that case
        /// the normalized Bet size is listed.
        /// if we assume a bet of 0.5 which is normalized to 5 (multiplied by 10), we end up with the following 
        /// representations for the following ActionSequences
        /// HeroB - "5"
        /// HeroXOppBHeroF - "X5F"
        /// HeroXOppBHeroC - "X5C"
        /// HeroXOppBHeroR - "X5R"
        /// OppBHeroF - "5F"
        /// OppBHeroC - "5C"
        /// OppBHeroR - "5R"
        /// Preflop - "F" or "C", or "R"
        /// It will also produce Sequences such as "X5RC" (he checked, opp bet 0.5, another opp raised and he called)
        /// These don't fit in any of the above standard situations and cannot be included in the standard
        /// statistics analysis. 
        /// They will automatically be ignored when querying for statistics because they won't match any of the
        /// standard Sequence strings.
        /// </para>
        void SetActionSequence(ref string currentSequence, IConvertedPokerAction myNextAction, Streets street);

        /// <summary>
        /// Determines and sets the strategic Position of the player
        /// </summary>
        /// <description>
        /// The number from 0 - 9 (10 player table)
        /// will be transformed into a number from (SB - BU) - SB,BB,EA,MI,LT,CO,BU
        /// to do this, all we need to do is to see, how far the player is away from the button
        /// and if he is SB or BB.
        /// 
        /// This works for a 2 - 10 Player Table
        /// Note that only with at least 8 players will there be an early Position
        /// any table smaller will start with MI Position, some will only have EA etc.
        /// This way the playing style is analyzed only according to the relative position to
        /// the BU and the actual table size becomes irrelevant
        /// </description>
        /// <param name="playerCount">
        /// A number from 0 (SB) to TotalPlayers -1 (BU)
        /// </param>
        void SetStrategicPosition(int playerCount);
    }
}