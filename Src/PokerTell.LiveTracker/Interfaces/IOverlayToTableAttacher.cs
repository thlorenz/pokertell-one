namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Windows;

    using Tools.GenericUtilities;
    using Tools.Interfaces;
    using Tools.WPF.Interfaces;

    public interface IOverlayToTableAttacher : IFluentInterface, IDisposable
    {
        event EventHandler<GenericEventArgs<Point, Size>> PositionOrSizeOfTableChanged;

        event EventHandler<GenericEventArgs<string>> TableChanged;

        event Action TableClosed;

        string FullText { get; set; }

        string TableName { get; set; }

        void Activate();

        IOverlayToTableAttacher InitializeWith(IWindowManager tableOverlayWindow, string pokerSite, string tableName);
    }
}