namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using PokerTell.Statistics.ViewModels.StatisticsSetDetails;

    public class DetailedStatisticsRowViewModel : IDetailedStatisticsRowViewModel
    {
        #region Constructors and Destructors

        public DetailedStatisticsRowViewModel(string title, IEnumerable<int> values, string unit)
        {
            Title = title;

            Cells = new List<IDetailedStatisticsCellViewModel>();
            values.ToList().ForEach(value => Cells.Add(new DetailedStatisticsCellViewModel(value)));

            Unit = unit;

            IsSelectable = Unit.Length > 0;
            
        }

        #endregion

        #region Properties

        public IList<IDetailedStatisticsCellViewModel> Cells { get; protected set; }

        public string Title { get; protected set; }

        public string Unit { get; protected set; }

        public bool IsSelectable { get; protected set; }

        #endregion
    }
}