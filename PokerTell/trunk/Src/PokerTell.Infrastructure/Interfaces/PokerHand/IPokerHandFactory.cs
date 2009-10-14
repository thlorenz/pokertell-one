namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    public interface IPokerHandFactory
    {
        IAquiredPokerAction AquiredPokerAction { get; }

        IAquiredPokerRound AquiredPokerRound { get; }

        IAquiredPokerPlayer AquiredPokerPlayer { get; }

        IAquiredPokerHand AquiredPokerHand { get; }

        IConvertedPokerAction ConvertedPokerAction { get; }

        IConvertedPokerActionWithId ConvertedPokerActionWithId { get; }

        IConvertedPokerRound ConvertedPokerRound { get; }

        IConvertedPokerPlayer ConvertedPokerPlayer { get; }

        IConvertedPokerHand ConvertedPokerHand { get; }

        IPokerHands PokerHands { get; }
    }
}