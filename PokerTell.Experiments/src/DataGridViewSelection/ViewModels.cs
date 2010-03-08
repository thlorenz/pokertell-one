namespace DataGridViewSelection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    using Tools.WPF;
   
    public class CellViewModel
    {
        #region Constants and Fields

        #endregion

        #region Constructors and Destructors

        public CellViewModel(int value, string unit)
        {
            Value = value;
            Unit = unit;
        }

        #endregion

        #region Properties

        public int Value { get; protected set; }

        public string Unit { get; protected set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Value.ToString();
        }

        #endregion
    }

    public class RowViewModel
    {
        #region Constructors and Destructors

        public RowViewModel(string title, IEnumerable<int> values, string unit)
        {
            Title = title;
            Unit = unit;
            Cells = new List<CellViewModel>();
            IsSelectable = Unit.Length > 0;
            values.ToList().ForEach(value => Cells.Add(new CellViewModel(value, unit)));
        }

        #endregion

        #region Properties

        public IList<CellViewModel> Cells { get; protected set; }

        public string Title { get; protected set; }

        public string Unit { get; protected set; }

        public bool IsSelectable { get; protected set; }

        #endregion
    }

    public class RaiseStatisticsViewModel
    {
        #region Constructors and Destructors

        public RaiseStatisticsViewModel()
        {
            ColumnHeaderTitle = "Bet Size";
            
            SelectedCells = new List<CellViewModel>();
            
            Rows = new List<RowViewModel>
                {
                    new RowViewModel("Fold", new[] { 20, 30, 12, 40 }, "%"), 
                    new RowViewModel("Call", new[] { 10, 35, 7, 60 }, "%"), 
                    new RowViewModel("Raise", new[] { 9, 44, 56, 70 }, "%"),
                    new RowViewModel("Count", new[] {1, 4, 30, 34, 33}, string.Empty)
                };
        }

        /*  minThree :: Int -> Int -> Int -> Int
            minThree x y z 
              | x <= y && x <=z   = x
              | y <=z             = y                      
              | otherwise         = z     */

        int MinThree(int x, int y, int z)
        {
            return
                  x <= y && x <= z    ? x
                : y <= z              ? y
                : z;
            
        }

        int testMinThree()
        {
            return MinThree(2, 5, 1);
        }
        #endregion

        #region Properties

        public IList<RowViewModel> Rows { get; protected set; }

        public IList<CellViewModel> SelectedCells { get; protected set; }

        public string ColumnHeaderTitle { get; protected set; }
        #endregion

        #region Public Methods

        ICommand _investigateCommand;

        public ICommand InvestigateCommand
        {
            get
            {
                return _investigateCommand ?? (_investigateCommand = new SimpleCommand
                    {
                        ExecuteDelegate = _ => {
                            var sb = new StringBuilder();
                            sb.AppendLine("Investigating: ");
                            SelectedCells.ToList().ForEach(cell => sb.Append(cell + ", "));
                            Console.WriteLine(sb);
                        },
                        CanExecuteDelegate = _ => SelectedCells.Count > 0
                    });
                
            }
        }

        #endregion

        public void ClearSelection()
        {
            SelectedCells.Clear();
        }

        public void AddToSelectionFrom(int row, int column)
        {
            SelectedCells.Add(Rows[row].Cells[column]);
        }
    }
}