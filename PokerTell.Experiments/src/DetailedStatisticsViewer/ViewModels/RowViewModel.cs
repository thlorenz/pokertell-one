namespace DetailedStatisticsViewer.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IRowViewModel
    {
        IList<ICellViewModel> Cells { get; }

        string Title { get; }

        string Unit { get; }

        bool IsSelectable { get; }
    }

    public class RowViewModel : IRowViewModel
    {
        #region Constructors and Destructors

        public RowViewModel(string title, IEnumerable<string> values)
        {
           Title = title;
            
           Cells = new List<ICellViewModel>();
           values.ToList().ForEach(value => Cells.Add(new CellViewModel(value)));
        }

        public RowViewModel(string title, IEnumerable<int> values, string unit)
        {
            Title = title;

            Cells = new List<ICellViewModel>();
            values.ToList().ForEach(value => Cells.Add(new CellViewModel(value)));

            Unit = unit;

            IsSelectable = Unit.Length > 0;
            
        }

        #endregion

        #region Properties

        

        public IList<ICellViewModel> Cells { get; protected set; }

        public string Title { get; protected set; }

        public string Unit { get; protected set; }

        public bool IsSelectable { get; protected set; }

        #endregion
    }
}