namespace PokerTell.PokerHand.Analyzation
{
    using System;
    using System.Linq;
    using System.Text;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    public class AnalyzablePokerPlayer : IAnalyzablePokerPlayer
    {
        public AnalyzablePokerPlayer()
        {
            InPosition = new bool?[(int)(Streets.River + 1)];
            ActionSequences = new ActionSequences[(int)(Streets.River + 1)];
            BetSizeIndexes = new int[(int)(Streets.River + 1)];
        }

        public long Id { get; set; }

        public int HandId { get; set; }

        /// <summary>
        /// Is player in position on Flop, Turn or River?
        /// </summary>
        public bool?[] InPosition { get; set; }

        /// <summary>
        /// M of player at the start of the hand
        /// </summary>
        public int MBefore { get; set; }

        /// <summary>
        /// Position of Player in relation to Button (SB - BU)
        /// </summary>
        public StrategicPositions StrategicPosition { get; set; }

        public ActionSequences[] ActionSequences { get; set; }

        public int[] BetSizeIndexes { get; set; }

        public override string ToString()
        {
            var sb =
                new StringBuilder(string.Format("Id: {0}, HandId: {1}, MBefore: {2}, StrategicPosition: {3}",
                                                Id,
                                                HandId,
                                                MBefore,
                                                StrategicPosition));
            sb.AppendLine("\nInPosition");
            InPosition.ToList().ForEach(sp => sb.Append(sp.ToString() + " "));
            sb.AppendLine("\nActionSequences");
            ActionSequences.ToList().ForEach(seq => sb.Append(seq.ToString() + " "));
            sb.AppendLine("\nBetSizeIndexes");
            BetSizeIndexes.ToList().ForEach(bs => sb.Append(bs.ToString() + " "));

            return sb.ToString();
        }
    }
}