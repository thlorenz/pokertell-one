namespace PokerTell.Statistics.ViewModels.Base
{
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Statistics.Interfaces;

    using StatisticsSetDetails;

    public class StatisticsTableRowViewModel : IStatisticsTableRowViewModel
    {
        #region Constructors and Destructors

        public StatisticsTableRowViewModel(string title, IEnumerable<int> values, string unit)
        {
            Title = title;

            Cells = new List<IStatisticsTableCellViewModel>();
            values.ToList().ForEach(value => Cells.Add(new StatisticsTableCellViewModel(value)));

            Unit = unit;

            IsSelectable = Unit.Length > 0;
            
        }

        #endregion

        #region Properties

        public IList<IStatisticsTableCellViewModel> Cells { get; protected set; }

        public string Title { get; protected set; }

        public string Unit { get; protected set; }

        public bool IsSelectable { get; protected set; }

        #endregion
    }
}