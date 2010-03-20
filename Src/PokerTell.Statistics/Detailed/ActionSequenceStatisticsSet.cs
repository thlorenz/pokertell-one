namespace PokerTell.Statistics.Detailed
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.Interfaces;

    public class ActionSequenceStatisticsSet : IActionSequenceStatisticsSet
    {
        protected readonly IPercentagesCalculator _percentagesCalculator;

        protected readonly IEnumerable<IActionSequenceStatistic> _statistics;

        IEnumerable<IAnalyzablePokerPlayer> _analyzablePokerPlayers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionSequenceStatisticsSet"/> class. 
        ///   Creates instance of ActionSequenceStatisticsSet.
        ///   Use for PostFlop Statistics only
        /// </summary>
        /// <param name="percentagesCalculator">
        /// </param>
        /// <param name="statistics">
        /// </param>
        /// <param name="playerName">
        /// <see cref="IActionSequenceStatisticsSet.PlayerName"/>
        /// </param>
        /// <param name="street">
        /// <see cref="IActionSequenceStatisticsSet.Street"/>
        /// </param>
        /// <param name="actionSequence">
        /// <see cref="IActionSequenceStatisticsSet.ActionSequence"/>
        /// </param>
        /// <param name="inPosition">
        /// <see cref="IActionSequenceStatisticsSet.InPosition"/>
        /// </param>
        public ActionSequenceStatisticsSet(
            IPercentagesCalculator percentagesCalculator, 
            IEnumerable<IActionSequenceStatistic> statistics, 
            string playerName, 
            Streets street, 
            ActionSequences actionSequence, 
            bool inPosition)
        {
            _percentagesCalculator = percentagesCalculator;
            _statistics = statistics;
            ActionSequence = actionSequence;
            PlayerName = playerName;
            Street = street;
            InPosition = inPosition;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionSequenceStatisticsSet"/> class. 
        ///   Creates instance of ActionSequenceStatisticsSet.
        ///   Use for PreFlop Statistics only
        /// </summary>
        /// <param name="percentagesCalculator">
        /// </param>
        /// <param name="statistics">
        /// </param>
        /// <param name="playerName">
        /// <see cref="IActionSequenceStatisticsSet.PlayerName"/>
        /// </param>
        /// <param name="actionSequence">
        /// <see cref="IActionSequenceStatisticsSet.ActionSequence"/>
        /// </param>
        /// <param name="raisedPot">
        /// <see cref="IActionSequenceStatisticsSet.RaisedPot"/>
        /// </param>
        public ActionSequenceStatisticsSet(
            IPercentagesCalculator percentagesCalculator, 
            IEnumerable<IActionSequenceStatistic> statistics, 
            string playerName, 
            ActionSequences actionSequence, 
            bool raisedPot)
            : this(percentagesCalculator, statistics, playerName, Streets.PreFlop, actionSequence, false)
        {
            RaisedPot = raisedPot;
        }

        public event Action<IActionSequenceStatisticsSet> StatisticsWereUpdated = delegate { };

        public ActionSequences ActionSequence { get; protected set; }

        public virtual IEnumerable<IActionSequenceStatistic> ActionSequenceStatistics
        {
            get { return _statistics; }
        }

        public int[] CumulativePercentagesByRow { get; private set; }

        public bool InPosition { get; protected set; }

        public string PlayerName { get; protected set; }

        public bool RaisedPot { get; protected set; }

        public Streets Street { get; protected set; }

        public virtual int[] SumOfCountsByColumn
        {
            get { return _percentagesCalculator.SumsOfCountsByColumn; }
        }

        public int[] TotalCounts
        {
            get { return (from statistic in _statistics select statistic.TotalCounts).ToArray(); }
        }

        public IActionSequenceStatisticsSet UpdateWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
        {
            _analyzablePokerPlayers = analyzablePokerPlayers;

            foreach (IActionSequenceStatistic statistic in _statistics)
            {
                statistic.UpdateWith(_analyzablePokerPlayers);
            }

            CalculateIndividualPercentages();
            CalculateCumulativePercentages();

            StatisticsWereUpdated(this);

            return this;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            _statistics.ToList().ForEach(s => sb.AppendLine(s.ToString()));
            sb.Append("Sum of counts by column: ");
            SumOfCountsByColumn.ToList().ForEach(sumOfCounts => sb.Append(sumOfCounts.ToString() + ", "));
            sb.AppendLine().Append("CumulativePercentages: ");
            CumulativePercentagesByRow.ToList().ForEach(perc => sb.Append(perc + "%, "));
            sb.AppendLine();
            return sb.ToString();
        }

        protected virtual void CalculateCumulativePercentages()
        {
            CumulativePercentagesByRow = new int[_statistics.Count()];
            int sumOfTotalCounts = (from statistic in _statistics select statistic.TotalCounts).Sum();

            for (int row = 0; row < CumulativePercentagesByRow.Length; row++)
            {
                if (_statistics.ElementAt(row).TotalCounts == 0)
                {
                    CumulativePercentagesByRow[row] = 0;
                }
                else
                {
                    double percentage = (double)_statistics.ElementAt(row).TotalCounts / sumOfTotalCounts * 100;
                    double roundedPercentage = Math.Round(percentage, MidpointRounding.AwayFromZero);
                    CumulativePercentagesByRow[row] = (int)roundedPercentage;
                }
            }
        }

        protected virtual void CalculateIndividualPercentages()
        {
            Func<int> getNumberOfRows = () => _statistics.Count();

            Func<int, int> getNumberOfColumnsAtRow = row => _statistics.ElementAt(row).MatchingPlayers.Length;

            Func<int, int, int> getCountAtRowColumn =
                (row, col) => _statistics.ElementAt(row).MatchingPlayers[col].Count;

            Action<int, int, int> setPercentageAtRowColumn =
                (row, col, percentage) => _statistics.ElementAt(row).Percentages[col] = percentage;

            _percentagesCalculator.CalculatePercentages(
                getNumberOfRows, 
                getNumberOfColumnsAtRow, 
                getCountAtRowColumn, 
                setPercentageAtRowColumn);
        }
    }
}