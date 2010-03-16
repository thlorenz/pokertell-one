namespace PokerTell.Statistics.ViewModels.StatisticsSetSummary
{
    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;

    using Tools.WPF.ViewModels;

    public class StatisticsSetSummaryRowViewModel : NotifyPropertyChanged, IStatisticsSetSummaryRowViewModel
    {
        readonly IBarGraphViewModel _barGraph;

        string _percentage;

        public StatisticsSetSummaryRowViewModel(ActionSequences actionSequence, IBarGraphViewModel barGraph)
        {
            _barGraph = barGraph;
            ActionLetter = ActionSequencesUtility.GetLastActionIn(actionSequence).ToString();
        }

        public string ActionLetter { get; private set; }

        public IBarGraphViewModel BarGraph
        {
            get { return _barGraph; }
        }

        public string Percentage
        {
            get { return _percentage; }
            private set
            {
                _percentage = value;
                RaisePropertyChanged(() => Percentage);
            }
        }

        public IStatisticsSetSummaryRowViewModel UpdateWith(int percentage, int[] percentagesByColumn)
        {
            Percentage = string.Format("{0:0#}", percentage);

            BarGraph.UpdateWith(percentagesByColumn);

            return this;
        }
    }
}