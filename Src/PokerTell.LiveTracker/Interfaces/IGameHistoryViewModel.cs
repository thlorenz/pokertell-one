namespace PokerTell.LiveTracker.Interfaces
{
    using Infrastructure.Interfaces;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    public interface IGameHistoryViewModel : IFluentInterface
    {
        IHandHistoryViewModel CurrentHandHistory { get; }

        int HandCount { get; }

        int CurrentHandIndex { get; set; }

        string TableName { get; }

        IGameHistoryViewModel AddNewHand(IConvertedPokerHand convertedPokerHand);
    }
}