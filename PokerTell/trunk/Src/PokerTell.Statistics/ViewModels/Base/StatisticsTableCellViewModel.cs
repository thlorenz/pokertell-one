namespace PokerTell.Statistics.ViewModels.Base
{
    using PokerTell.Statistics.Interfaces;

    public class StatisticsTableCellViewModel : IStatisticsTableCellViewModel
    {
        #region Constants and Fields

        #endregion

        #region Constructors and Destructors

        public StatisticsTableCellViewModel(int value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public int Value { get; protected set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Value.ToString();
        }

        #endregion
    }
}