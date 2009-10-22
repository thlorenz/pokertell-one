namespace Tools.Interfaces
{
    using System;
    using System.Windows.Threading;

    public interface IDispatcherTimer
    {
        #region Events

        event EventHandler Tick;

        #endregion

        #region Properties

        Dispatcher Dispatcher { get; }

        TimeSpan Interval { get; set; }

        bool IsEnabled { get; set; }

        object Tag { get; set; }

        #endregion

        #region Public Methods

        void Start();

        void Stop();

        #endregion
    }

    /// <summary>
    /// Adapts the Dispatcher class to implement the ITimer interface.
    /// </summary> 
    public class DispatcherTimerAdapter : DispatcherTimer, IDispatcherTimer
    {
        #region Constructors and Destructors

        public DispatcherTimerAdapter()
        {
        }

        public DispatcherTimerAdapter(DispatcherPriority priority)
            : base(priority)
        {
        }

        public DispatcherTimerAdapter(DispatcherPriority priority, Dispatcher dispatcher)
            : base(priority, dispatcher)
        {
        }

        public DispatcherTimerAdapter(
            TimeSpan timeSpan, 
            DispatcherPriority priority, 
            EventHandler callback, 
            Dispatcher dispatcher)
            : base(timeSpan, priority, callback, dispatcher)
        {
        }

        #endregion
    }

    public class FireOnStartDispatcherTimer : DispatcherTimer, IDispatcherTimer
    {
        #region Constructors and Destructors

        public FireOnStartDispatcherTimer()
        {
            EnableOverride = false;
            IsEnabled = true;
        }

        #endregion

        #region Events

        public new event EventHandler Tick;

        #endregion

        #region Properties

        public bool EnableOverride { get; set; }

        public bool Stopped { get; set; }

        #endregion

        #region Implemented Interfaces

        #region IDispatcherTimer

        public new void Start()
        {
            Stopped = false;
            if (!EnableOverride && IsEnabled && Tick != null)
            {
                Tick.Invoke(this, EventArgs.Empty);
            }
        }

        public new void Stop()
        {
            Stopped = true;
        }

        #endregion

        #endregion
    }
}