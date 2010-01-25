namespace PokerTell.Statistics.Interfaces
{
    using Infrastructure.Interfaces.PokerHand;

    public interface IPreflopCallingAnalyzer
    {
        string HeroHoleCards { get; }

        double Ratio { get; }

        IConvertedPokerHand ConvertedHand { get; }
    }
}