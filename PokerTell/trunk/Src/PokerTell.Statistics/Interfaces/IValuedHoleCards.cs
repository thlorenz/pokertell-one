namespace PokerTell.Statistics.Interfaces
{
    public interface IValuedHoleCards
    {
        int ChenValue { get; }

        int SklanskyMalmuthGrouping { get; }

        bool IsValid { get; }
    }
}