namespace PokerTell.LiveTracker.Interfaces
{
    using Infrastructure.Interfaces;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.WPF.Interfaces;

    public interface IGameHistoryViewModel : IFluentInterface
    {
        IHandHistoryViewModel CurrentHandHistory { get; }

        int HandCount { get; }

        int CurrentHandIndex { get; set; }

        string TableName { get; }

        IDimensionsViewModel Dimensions { get; }

        IGameHistoryViewModel AddNewHand(IConvertedPokerHand convertedPokerHand);

        IGameHistoryViewModel SaveDimensions();
    }
}