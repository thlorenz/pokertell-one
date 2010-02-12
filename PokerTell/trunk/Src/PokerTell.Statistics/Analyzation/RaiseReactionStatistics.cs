namespace PokerTell.Statistics.Analyzation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Utilities;

    public class RaiseReactionStatistics : IRaiseReactionStatistics
    {
        IRaiseReactionsAnalyzer _raiseReactionsAnalyzer;

        #region Properties

        public IDictionary<int, int> TotalCountsByColumnDictionary { get; private set; }

        public IDictionary<ActionTypes, IDictionary<int, IList<IAnalyzablePokerPlayer>>> AnalyzablePlayersDictionary { get; private set; }

        public IDictionary<ActionTypes, IDictionary<int, int>> PercentagesDictionary { get; private set; }

        #endregion

        #region Implemented Interfaces

        #region IRaiseReactionStatistics

        public IRaiseReactionStatistics InitializeWith(IRaiseReactionsAnalyzer raiseReactionsAnalyzer)
        {
            _raiseReactionsAnalyzer = raiseReactionsAnalyzer;

            if (raiseReactionsAnalyzer.RaiseSizeKeys.Length < 1)
            {
                throw new ArgumentException("ReactionsAnalyzer needs to have at least one raise size");
            }

            CreateAnalyzablePlayersDictionary();

            CreatePercentagesDictionary();

            PopulateAnalyzablePlayersDictionary();
            
            CalculateCounts();
           
            CalculatePercentages();

            return this;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Percentages: \n");
            //            foreach (ActionTypes reaction in ActionTypesUtility.Reactions)
            //            {
            //                ActionTypes reaction1 = reaction;
            //                string line = _raiseReactionAnalyzer.RaiseSizeKeys.Aggregate(
            //                    string.Empty,
            //                    (current, ratioSizeKey) =>
            //                    current + string.Format("| {0}% |", PercentagesDictionary[reaction1][(int)ratioSizeKey]));
            //
            //                sb.AppendLine(reaction + "  " + line);
            //            }

            return sb.ToString();
        }

        #endregion

        #endregion

        #region Methods

        void CalculateCounts()
        {
            TotalCountsByColumnDictionary = new Dictionary<int, int>();
            foreach (double raiseSizeKey in _raiseReactionsAnalyzer.RaiseSizeKeys)
            {
                var totalCount =
                    AnalyzablePlayersDictionary[ActionTypes.F][(int)raiseSizeKey].Count +
                    AnalyzablePlayersDictionary[ActionTypes.C][(int)raiseSizeKey].Count +
                    AnalyzablePlayersDictionary[ActionTypes.R][(int)raiseSizeKey].Count;

                TotalCountsByColumnDictionary.Add((int)raiseSizeKey, totalCount);
            }
        }

        void CalculatePercentages()
        {
            foreach (double raiseSize in _raiseReactionsAnalyzer.RaiseSizeKeys)
            {
                var raiseSizeKey = (int)raiseSize;
                foreach (ActionTypes actionWhat in ActionTypesUtility.Reactions)
                {
                    PercentagesDictionary[actionWhat][raiseSizeKey] =
                        AnalyzablePlayersDictionary[actionWhat][raiseSizeKey].Count > 0
                            ? (int)
                              ((100 * AnalyzablePlayersDictionary[actionWhat][raiseSizeKey].Count) / (double)TotalCountsByColumnDictionary[raiseSizeKey])
                            : 0;
                }
            }

            /*
             * for some reason settin percentages doesn't work here
             * so we take the easy way out and used the uncool alternative above
                
               new AcrossRowsPercentagesCalculator().CalculatePercentages(
                    () => ActionTypesUtility.Reactions.Count(),
                    row => _raiseReactionsAnalyzer.RaiseSizeKeys.Length,
                    (row, col) => AnalyzablePlayersDictionary[ActionTypesUtility.Reactions.ElementAt(row)][(int)_raiseReactionsAnalyzer.RaiseSizeKeys[col]].Count,
                    (perc, row, col) => PercentagesDictionary[ActionTypesUtility.Reactions.ElementAt(row)][(int)_raiseReactionsAnalyzer.RaiseSizeKeys[col]] = perc);
             */
        }

        void CreateAnalyzablePlayersDictionary()
        {
            AnalyzablePlayersDictionary = new Dictionary<ActionTypes, IDictionary<int, IList<IAnalyzablePokerPlayer>>>();
            foreach (ActionTypes reaction in ActionTypesUtility.Reactions)
            {
                AnalyzablePlayersDictionary.Add(reaction, new Dictionary<int, IList<IAnalyzablePokerPlayer>>());

                foreach (double ratioSizeKey in _raiseReactionsAnalyzer.RaiseSizeKeys)
                {
                    AnalyzablePlayersDictionary[reaction][(int)ratioSizeKey] = new List<IAnalyzablePokerPlayer>();
                }
            }
        }

        void CreatePercentagesDictionary()
        {
            PercentagesDictionary = new Dictionary<ActionTypes, IDictionary<int, int>>();
            foreach (ActionTypes reaction in ActionTypesUtility.Reactions)
            {
                PercentagesDictionary.Add(reaction, new Dictionary<int, int>());
                foreach (double ratioSizeKey in _raiseReactionsAnalyzer.RaiseSizeKeys)
                {
                    PercentagesDictionary[reaction][(int)ratioSizeKey] = 0;
                }
            }
        }

        void PopulateAnalyzablePlayersDictionary()
        {
            foreach (IRaiseReactionAnalyzer raiseReactionAnalyzer in _raiseReactionsAnalyzer.RaiseReactionAnalyzers)
            {
                AnalyzablePlayersDictionary [raiseReactionAnalyzer.HeroReactionType] [raiseReactionAnalyzer.ConsideredRaiseSize]
                    .Add(raiseReactionAnalyzer.AnalyzablePokerPlayer);
            }
        }

        #endregion
    }
}