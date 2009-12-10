namespace PokerTell.Statistics.Detailed
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    public abstract class ActionSequenceStatistic : IActionSequenceStatistic
    {
        public ActionSequences ActionSequence { get; private set; }

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
            ActionSequence = actionSequence;
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
            _totalCounts = 0;
            foreach (var matchingPlayerCollection in MatchingPlayers)
            {
                _totalCounts += matchingPlayerCollection.Count;
            }
        }

        protected abstract void ExtractMatchingPlayers(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers);
    }
}