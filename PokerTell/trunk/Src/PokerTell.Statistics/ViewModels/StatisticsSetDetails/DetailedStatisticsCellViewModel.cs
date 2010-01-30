namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    public class DetailedStatisticsCellViewModel : IDetailedStatisticsCellViewModel
    {
        #region Constants and Fields

        #endregion

        #region Constructors and Destructors

        public DetailedStatisticsCellViewModel(int value)
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