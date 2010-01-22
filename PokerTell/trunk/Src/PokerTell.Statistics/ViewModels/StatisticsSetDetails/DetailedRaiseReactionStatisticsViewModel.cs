namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    using Infrastructure.Interfaces.Statistics;

    using Tools.WPF;

    public class DetailedRaiseReactionStatisticsViewModel : DetailedStatisticsViewModel
    {
        #region Constants and Fields


        #endregion

        #region Constructors and Destructors

        public DetailedRaiseReactionStatisticsViewModel()
            : base("Raise Size")
        {
           
        }

        #endregion

        #region Properties

        #endregion

        public override IDetailedStatisticsViewModel InitializeWith(IActionSequenceStatisticsSet statisticsSet)
        {
            return this;
        }
    }
}