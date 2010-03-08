namespace PokerTell.PokerHand.Services
{
    using Infrastructure.Interfaces.PokerHand;

    public interface IPokerRoundsConverter
    {
        IConvertedPokerHand ConvertFlopTurnAndRiver();

        IPokerRoundsConverter ConvertPreflop();

        IPokerRoundsConverter InitializeWith(
            IAquiredPokerHand aquiredHand, IConvertedPokerHand convertedHand, double pot, double toCall);
    }
}