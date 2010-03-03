namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Windows;

    using Tools.GenericUtilities;

    public interface ITableAttacher : IDisposable
    {
        #region Events

        event EventHandler<GenericEventArgs<Point, Size>> PositionOrSizeOfTableChanged;

        event EventHandler<GenericEventArgs<string>> TableChanged;

        event EventHandler<GenericEventArgs<string>> TableClosed;

        #endregion

        #region Properties

        string FullText { get; set; }

        string TableName { get; set; }

        #endregion

        #region Public Methods

        void Activate();

        void InitializeWith(string tableName, string processName);

        #endregion
    }
}