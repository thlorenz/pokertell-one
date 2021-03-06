namespace PokerTell.Statistics.ViewModels.Base
{
    using System;

    using Infrastructure.Interfaces.Statistics;

    using Tools.WPF.ViewModels;

    public class DetailedStatisticsAnalyzerContentViewModel : NotifyPropertyChanged, IDetailedStatisticsAnalyzerContentViewModel
    {
        public event Action<IDetailedStatisticsAnalyzerContentViewModel> ChildViewModelChanged = delegate { };

        public bool ShowAsPopup { get; protected set; }

        public bool MayInvestigateHoleCards { get; protected set; }

        public bool MayInvestigateRaise { get; protected set; }

        public bool MayBrowseHands { get; protected set; }

        public bool MayVisualizeHands { get; protected set; }

        // To be used by interested views to decide if they should show the Investigator Panel
        public bool MayInvestigate
        {
            get { return MayInvestigateHoleCards || MayInvestigateRaise || MayBrowseHands || MayVisualizeHands; }
        }

        /// <summary>
        /// Allows reacting to scrolling events e.g. MouseWheel.
        /// Not all implementers will implement this method.
        /// </summary>
        /// <param name="change">Sign indicates scrolling direction, value indicates size of the scroll</param>
        public virtual void Scroll(int change)
        {
            // Do nothing by default
        }

        public void RaiseChildViewModelChanged(IDetailedStatisticsAnalyzerContentViewModel childViewModel)
        {
            ChildViewModelChanged(childViewModel); 
        }
    }
}