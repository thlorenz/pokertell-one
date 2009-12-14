namespace PokerTell.Statistics.ViewModels.StatisticsSetSummary
{
    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Statistics.Interfaces;

    using Tools.WPF.ViewModels;

    public class StatisticsSetSummaryRowViewModel : NotifyPropertyChanged, IStatisticsSetSummaryRowViewModel
    {
        #region Constants and Fields

        readonly IBarGraphViewModel _barGraph;

        string _percentage;

        #endregion

        #region Constructors and Destructors

        public StatisticsSetSummaryRowViewModel(ActionSequences actionSequence, IBarGraphViewModel barGraph)
        {
            _barGraph = barGraph;
            ActionLetter = ActionSequencesUtility.GetLastActionIn(actionSequence).ToString();
        }

        #endregion

        #region Properties

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

        #endregion

        #region Public Methods

        public StatisticsSetSummaryRowViewModel UpdateWith(int percentage, int[] percentagesByColumn)
        {
            Percentage = string.Format("{0}%", percentage);
            
            BarGraph.UpdateWith(percentagesByColumn);
            
            return this;
        }

        #endregion
    }
}