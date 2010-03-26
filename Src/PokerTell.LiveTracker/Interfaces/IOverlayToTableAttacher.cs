namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Windows;

    using Infrastructure.Interfaces;

    using Tools.GenericUtilities;
    using Tools.Interfaces;
    using Tools.WPF.Interfaces;

    public interface IOverlayToTableAttacher : IFluentInterface, IDisposable
    {
        event Action<string> TableChanged;

        event Action TableClosed;

        string FullText { get; set; }

        string TableName { get; set; }

        void Activate();

        IOverlayToTableAttacher InitializeWith(IWindowManager tableOverlayWindow, IDispatcherTimer watchTableTimer, IDispatcherTimer waitThenTryToFindTableAgainTimer, IPokerRoomInfo pokerRoomInfo, string tableName);
    }
}