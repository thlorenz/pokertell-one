namespace PokerTell.Statistics.Detailed
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    public abstract class ActionSequenceStatistic : IActionSequenceStatistic
    {
        public ActionSequences _actionSequence { get; private set; }

        public IActionSequenceStatistic UpdateWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
        {
            ExtractMatchingPlayers(analyzablePokerPlayers);
            CalculateTotalCounts();
            return this;
        }

        public IList<IAnalyzablePokerPlayer>[] MatchingPlayers { get; protected set; }

        public int TotalCounts
        {
            get { return _totalCounts; }
        }

        /// <summary>
        /// Compares all occurences for a certain Betsize/Position
        /// for all actions in a Stat Collection and gives percentage for each Betsize/Position
        /// </summary>
        public int[] Percentages { get; set; }

        protected readonly Streets _street;

        int _totalCounts;

        protected ActionSequenceStatistic(ActionSequences actionSequence, Streets street, int indexesCount)
        {
            _actionSequence = actionSequence;
            _street = street;

            Percentages = new int[indexesCount];

            MatchingPlayers = new List<IAnalyzablePokerPlayer>[indexesCount];
            for (int i = 0; i < MatchingPlayers.Length; i++)
            {
                MatchingPlayers[i] = new List<IAnalyzablePokerPlayer>();
            }
        }

        protected virtual void CalculateTotalCounts()
        {
            _totalCounts = (from matchingPlayerCollection in MatchingPlayers
                            select matchingPlayerCollection.Count)
                            .Sum();
        }

        protected abstract void ExtractMatchingPlayers(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers);

        public override string ToString()
        {
            var sb = new StringBuilder(string.Format("{0} on {1} with {2} total counts: ", _actionSequence, _street, _totalCounts));
            Percentages.ToList().ForEach(p => sb.Append(p + "% "));
            return sb.ToString();
        }
    }
}