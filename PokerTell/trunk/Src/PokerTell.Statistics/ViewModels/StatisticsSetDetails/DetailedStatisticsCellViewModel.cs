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
            :this(value.ToString())
        {
        }

        public DetailedStatisticsCellViewModel(string value)
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