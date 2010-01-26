namespace PokerTell.Statistics.Analyzation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Infrastructure;
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Utilities;

    public class RaiseReactionStatistics : IRaiseReactionStatistics
    {
        #region Properties

        public IDictionary<int, int> CountsDictionary { get; private set; }

        public IDictionary<ActionTypes, IDictionary<int, IList<IAnalyzablePokerPlayer>>> HandsDictionary { get; private set; }

        public IDictionary<ActionTypes, IDictionary<int, int>> PercentagesDictionary { get; private set; }

        #endregion

        #region Implemented Interfaces

        #region IRaiseReactionStatistics

        public IRaiseReactionStatistics InitializeWith(
            IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers, Streets street, ActionSequences actionSequence)
        {
            CreateHandsDictionary();

            PopulateHandsDictionary(analyzablePokerPlayers, street, actionSequence);

            CalculateCounts();

            CreatePercentagesDictionary();

            CalculatePercentages();

            return this;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Percentages: \n");
            foreach (ActionTypes reaction in PokerActionsUtility.Reactions)
            {
                ActionTypes reaction1 = reaction;
                string line = ApplicationProperties.RaiseSizeKeys.Aggregate(
                    string.Empty,
                    (current, raiseSizeKey) =>
                    current + string.Format("| {0}% |", PercentagesDictionary[reaction1][(int)raiseSizeKey]));

                sb.AppendLine(reaction + "  " + line);
            }

            return sb.ToString();
        }

        #endregion

        #endregion

        #region Methods

        void CalculateCounts()
        {
            CountsDictionary = new Dictionary<int, int>();
            foreach (double raiseSizeKey in ApplicationProperties.RaiseSizeKeys)
            {
                CountsDictionary.Add(
                    (int)raiseSizeKey,
                    HandsDictionary[ActionTypes.F][(int)raiseSizeKey].Count +
                    HandsDictionary[ActionTypes.C][(int)raiseSizeKey].Count +
                    HandsDictionary[ActionTypes.R][(int)raiseSizeKey].Count);
            }
        }

        void CalculatePercentages()
        {
            //            foreach (double raiseSize in ApplicationProperties.RaiseSizeKeys)
            //            {
            //                var raiseSizeKey = (int)raiseSize;
            //                foreach (ActionTypes actionWhat in PokerActionsUtility.Reactions)
            //                {
            //                    PercentagesDictionary[actionWhat][raiseSizeKey] =
            //                        HandsDictionary[actionWhat][raiseSizeKey].Count > 0
            //                            ? (int)
            //                              ((100 * HandsDictionary[actionWhat][raiseSizeKey].Count) / (double)CountsDictionary[raiseSizeKey])
            //                            : 0;
            //                }
            //            }

            new AcrossRowsPercentagesCalculator().CalculatePercentages(
                () => PercentagesDictionary.Count,
                row => PercentagesDictionary[PokerActionsUtility.Reactions.ElementAt(row)].Count,
                (row, col) => HandsDictionary[PokerActionsUtility.Reactions.ElementAt(row)][col].Count,
                (perc, row, col) => PercentagesDictionary[PokerActionsUtility.Reactions.ElementAt(row)][col] = perc);
        }

        void CreateHandsDictionary()
        {
            HandsDictionary = new Dictionary<ActionTypes, IDictionary<int, IList<IAnalyzablePokerPlayer>>>();
            foreach (ActionTypes reaction in PokerActionsUtility.Reactions)
            {
                HandsDictionary.Add(reaction, new Dictionary<int, IList<IAnalyzablePokerPlayer>>());

                foreach (double raiseSizeKey in ApplicationProperties.RaiseSizeKeys)
                {
                    HandsDictionary[reaction][(int)raiseSizeKey] = new List<IAnalyzablePokerPlayer>();
                }
            }
        }

        void CreatePercentagesDictionary()
        {
            PercentagesDictionary = new Dictionary<ActionTypes, IDictionary<int, int>>();
            foreach (ActionTypes reaction in PokerActionsUtility.Reactions)
            {
                PercentagesDictionary.Add(reaction, new Dictionary<int, int>());
                foreach (double raiseSizeKey in ApplicationProperties.RaiseSizeKeys)
                {
                    PercentagesDictionary[reaction][(int)raiseSizeKey] = 0;
                }
            }
        }

        void PopulateHandsDictionary(
            IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers, Streets street, ActionSequences actionSequence)
        {
            foreach (IAnalyzablePokerPlayer analyzablePokerPlayer in analyzablePokerPlayers)
            {
                IReactionAnalyzationPreparer analyzationPreparer =
                    new ReactionAnalyzationPreparer(
                        analyzablePokerPlayer.Sequences[(int)street], analyzablePokerPlayer.Position, actionSequence);

                if (analyzationPreparer.WasSuccessful)
                {
                    var analyzer = new RaiseReactionAnalyzer(analyzationPreparer);

                    if (analyzer.IsValidResult && analyzer.IsStandardSituation)
                    {
                        HandsDictionary[analyzer.HeroReactionType][analyzer.OpponentRaiseSize].Add(analyzablePokerPlayer);
                    }
                }
            }
        }

        #endregion
    }
}