namespace PokerTell.Statistics.Detailed
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    public abstract class DetailedStatisticBase : IDetailedStatistic
    {
        public virtual IDetailedStatistic UpdateWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
        {
            _analyzablePokerPlayers = analyzablePokerPlayers;
            return this;
        }

        public IList<int>[] HandIdsLists { get; set; }

        protected IEnumerable<IAnalyzablePokerPlayer> _analyzablePokerPlayers;

        /// <summary>
        /// Compares all occurences for a certain Betsize/Position
        /// for all actions in a Stat Collection and gives percentage for each Betsize/Position
        /// </summary>
        public int[] Percentages { get; set; }

        protected ActionSequences _actionSequence;

        protected Streets _street;

        protected bool _inPosition;

        protected DetailedStatisticBase(ActionSequences actionSequence, Streets street, bool inPosition, int indexesCount)
        {
            _actionSequence = actionSequence;
            _street = street;
            _inPosition = inPosition;

            Percentages = new int[indexesCount];

            HandIdsLists = new List<int>[indexesCount];
            for (int i = 0; i < HandIdsLists.Length; i++)
            {
                HandIdsLists[i] = new List<int>();
            }
        }
    }
}