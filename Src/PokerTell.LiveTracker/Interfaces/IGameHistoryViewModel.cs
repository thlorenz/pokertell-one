namespace PokerTell.LiveTracker.Interfaces
{
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.Interfaces;

    public interface IGameHistoryViewModel : IFluentInterface
    {
        IHandHistoryViewModel CurrentHandHistory { get; }

        int HandCount { get; }

        int CurrentHandIndex { get; set; }

        IGameHistoryViewModel AddNewHand(IConvertedPokerHand convertedPokerHand);
    }
}