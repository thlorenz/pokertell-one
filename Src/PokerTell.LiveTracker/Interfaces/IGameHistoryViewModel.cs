namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Windows.Input;

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

        ICommand PopoutCommand { get; }

        ICommand PopinCommand { get; }

        IGameHistoryViewModel AddNewHand(IConvertedPokerHand convertedPokerHand);

        IGameHistoryViewModel SaveDimensions();

        event Action PopMeOut;

        event Action PopMeIn;
    }
}