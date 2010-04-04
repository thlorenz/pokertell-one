namespace PokerTell.Statistics.ViewModels.Base
{
    using PokerTell.Statistics.Interfaces;

    public class StatisticsTableCellViewModel : IStatisticsTableCellViewModel
    {
        public StatisticsTableCellViewModel(int value)
            : this(value.ToString())
        {
        }

        public StatisticsTableCellViewModel(string value)
        {
            Value = value;
        }

        public string Value { get; protected set; }

        public override string ToString()
        {
            return Value;
        }
    }
}