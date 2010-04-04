namespace PokerTell.Statistics.ViewModels.Base
{
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Statistics.Interfaces;

    public class StatisticsTableRowViewModel : IStatisticsTableRowViewModel
    {
        public StatisticsTableRowViewModel(string title, IEnumerable<string> values, string unit)
            : this(title, unit)
        {
            values.ToList().ForEach(value => Cells.Add(new StatisticsTableCellViewModel(value)));
        }

        public StatisticsTableRowViewModel(string title, IEnumerable<int> values, string unit)
            : this(title, unit)
        {
            values.ToList().ForEach(value => Cells.Add(new StatisticsTableCellViewModel(value)));
        }

        StatisticsTableRowViewModel(string title, string unit)
        {
            Title = title;

            Cells = new List<IStatisticsTableCellViewModel>();

            Unit = unit;

            IsSelectable = Unit.Length > 0;
        }

        public IList<IStatisticsTableCellViewModel> Cells { get; protected set; }

        public bool IsSelectable { get; protected set; }

        public string Title { get; protected set; }

        public string Unit { get; protected set; }
    }
}