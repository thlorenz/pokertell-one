namespace DetailedStatisticsViewer.ViewModels
{
    using System;

    public interface ICellViewModel
    {
        string Value { get; }
    }

    public class CellViewModel : ICellViewModel
    {
        #region Constants and Fields

        #endregion

        #region Constructors and Destructors

        public CellViewModel(int value)
            :this(value.ToString())
        {
        }

        public CellViewModel(string value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public string Value { get; protected set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Value;
        }

        #endregion
    }
}