namespace PokerTell.Statistics.Analyzation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Controls;

    using Detailed;

    using Infrastructure;
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using Utilities;

    public class RaiseReactionStatistics : IRaiseReactionStatistics
    {
        #region Constants and Fields

        readonly IConstructor<IConvertedPokerActionWithId> _convertedPokerActionWithIdMake;

        #endregion

        #region Constructors and Destructors

        public RaiseReactionStatistics(IConstructor<IConvertedPokerActionWithId> convertedPokerActionWithIdMake)
        {
            _convertedPokerActionWithIdMake = convertedPokerActionWithIdMake;
        }

        #endregion

        #region Properties

        public IDictionary<int, int> CountsDictionary { get; private set; }

        public IDictionary<ActionTypes, IDictionary<int, IList<IConvertedPokerHand>>> HandsDictionary { get; private set; }

        public IDictionary<ActionTypes, IDictionary<int, int>> PercentagesDictionary { get; private set; }

        #endregion

        #region Implemented Interfaces

        #region IRaiseReactionStatistics

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
            HandsDictionary = new Dictionary<ActionTypes, IDictionary<int, IList<IConvertedPokerHand>>>();
            foreach (ActionTypes reaction in PokerActionsUtility.Reactions)
            {
                HandsDictionary.Add(reaction, new Dictionary<int, IList<IConvertedPokerHand>>());

                foreach (double raiseSizeKey in ApplicationProperties.RaiseSizeKeys)
                {
                    HandsDictionary[reaction][(int)raiseSizeKey] = new List<IConvertedPokerHand>();
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

        public IRaiseReactionStatistics InitializeWith(
            IEnumerable<IConvertedPokerHand> convertedHands, IActionSequenceStatisticsSet actionSequenceStatisticsSet)
        {
            CreateHandsDictionary();

            PopulateHandsDictionary(convertedHands, actionSequenceStatisticsSet);

            CalculateCounts();

            CreatePercentagesDictionary();

            CalculatePercentages();

            return this;
        }

        void PopulateHandsDictionary(IEnumerable<IConvertedPokerHand> convertedHands, IActionSequenceStatisticsSet info)
        {
            foreach (IConvertedPokerHand convertedHand in convertedHands)
            {
                IReactionAnalyzationPreparer analyzationPreparer = new ReactionAnalyzationPreparer(
                    convertedHand, info.Street, info.PlayerName, info.ActionSequence);

                if (analyzationPreparer.WasSuccessful)
                {
                    var analyzer = new RaiseReactionAnalyzer(analyzationPreparer, _convertedPokerActionWithIdMake);

                    if (analyzer.IsValidResult && analyzer.IsStandardSituation)
                    {
                        HandsDictionary[analyzer.HeroReaction][analyzer.OpponentRaiseSize].Add(convertedHand);
                    }
                }
            }
        }

        #endregion
    }
}