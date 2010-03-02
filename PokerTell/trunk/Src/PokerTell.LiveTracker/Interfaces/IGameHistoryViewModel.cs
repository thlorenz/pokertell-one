namespace PokerTell.LiveTracker.Interfaces
{
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    public interface IGameHistoryViewModel
    {
        IHandHistoryViewModel CurrentHandHistory { get; }

        int HandCount { get; }

        int CurrentHandIndex { get; set; }

        IGameHistoryViewModel AddNewHand(IConvertedPokerHand convertedPokerHand);
    }
}