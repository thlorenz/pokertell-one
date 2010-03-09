namespace PokerTell.LiveTracker.Overlay
{
    using System;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using log4net;

    using PokerTell.Infrastructure.PokerRooms;
    using PokerTell.LiveTracker.Interfaces;

    using Tools.Interfaces;
    using Tools.WPF.Interfaces;

    /// <summary>
    /// Description of OverlayToTableAttacher.
    /// </summary>
    /// <remarks>
    /// The way this class is designed it is only possible to test a subset, but it has been proven to work in the past.
    /// So for now this will have to do.
    /// Specifically the callback actions couldn't be tested.
    /// </remarks>
    public class OverlayToTableAttacher : IOverlayToTableAttacher
    {
        public OverlayToTableAttacher(IWindowManipulator windowManipulator, IWindowFinder windowFinder)
        {
            _windowManipulator = windowManipulator;
            _windowFinder = windowFinder;
        }

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        IDispatcherTimer _watchTableTimer;

        IDispatcherTimer _waitThenTryToFindTableAgainTimer;

        readonly IWindowManipulator _windowManipulator;

        string _fullText;

        string _processName;

        string _tableName;

        protected bool _waitingForNewTableName;

        readonly IWindowFinder _windowFinder;

        IWindowManager _tableOverlayWindow;

        public string TableName
        {
            get { return _tableName; }

            set
            {
                _tableName = value;
                _waitingForNewTableName = false;
            }
        }

        public string FullText
        {
            get { return _fullText; }

            set { _fullText = value; }
        }

        public event Action<string> TableChanged = delegate { };

        public event Action TableClosed = delegate { };

        public void Activate()
        {
            AttachToTableWith(_tableName, _processName);
        }

        public IOverlayToTableAttacher InitializeWith(
            IWindowManager tableOverlayWindow, 
            IDispatcherTimer watchTableTimer, 
            IDispatcherTimer waitThenTryToFindTableAgainTimer, 
            string pokerSite, 
            string tableName)
        {
            _tableOverlayWindow = tableOverlayWindow;

            _tableName = tableName;
            _processName = PokerRoomInfoUtility.GetProcessNameFor(pokerSite);

            _watchTableTimer = watchTableTimer;
            _watchTableTimer.Tick += WatchTableTimer_Tick;

            _waitThenTryToFindTableAgainTimer = waitThenTryToFindTableAgainTimer;
            _waitThenTryToFindTableAgainTimer.Tick += delegate { TryToFindTableAgain(); };

            return this;
        }

        public void Dispose()
        {
            _watchTableTimer.Stop();
            _waitThenTryToFindTableAgainTimer.Stop();
        }

        public void TableLostHandler()
        {
            _watchTableTimer.Stop();
            _waitThenTryToFindTableAgainTimer.Start();
        }

        void AttachToTableWith(string tableName, string processName)
        {
            bool tableFound = false;

            Func<IntPtr, bool> foundWindowCallback = handle => {
                _windowManipulator.WindowHandle = handle;
                tableFound = true;
                _watchTableTimer.Start();
                return true;
            };

            _windowFinder.FindWindowMatching(new Regex(tableName), new Regex(processName), foundWindowCallback);
            if (! tableFound)
            {
                Log.DebugFormat("Could not find table with Name: [{0}] and ProcessName : [{1}]", tableName, processName);
                TableClosed();
            }

            _waitThenTryToFindTableAgainTimer.Stop();
        }

        void WatchTableTimer_Tick(object sender, EventArgs e)
        {
            _windowManipulator.PlaceThisWindowDirectlyOnTopOfYours(_tableOverlayWindow, TableLostHandler);

            _windowManipulator.CheckWindowLocationAndSize(
                (location, size) => {
                    _tableOverlayWindow.Left = location.X;
                    _tableOverlayWindow.Top = location.Y;
                    _tableOverlayWindow.Width = size.Width;
                    _tableOverlayWindow.Height = size.Height;
                });

            if (! _waitingForNewTableName)
            {
                _windowManipulator.SetTextTo(TableName,
                                             () => { TableChanged(FullText); _waitingForNewTableName = true; });
            }
        }

        void TryToFindTableAgain()
        {
            AttachToTableWith(TableName, _processName);
        }
    }
}