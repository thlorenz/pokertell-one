namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Windows;

    using Tools.GenericUtilities;
    using Tools.WPF.Interfaces;

    public interface IOverlayToTableAttacher : IDisposable
    {
        event EventHandler<GenericEventArgs<Point, Size>> PositionOrSizeOfTableChanged;

        event EventHandler<GenericEventArgs<string>> TableChanged;

        event EventHandler<GenericEventArgs<string>> TableClosed;

        string FullText { get; set; }

        string TableName { get; set; }

        void Activate();

        IOverlayToTableAttacher InitializeWith(IWindowManager tableOverlayWindow, string pokerSite, string tableName);
    }
}