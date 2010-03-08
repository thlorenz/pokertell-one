namespace PokerTell.UnitTests.Tools
{
    using System;
    using System.Windows.Threading;

    using global::Tools.Interfaces;

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

    public class ManualInvokeDispatcherTimer : IDispatcherTimer
    {
        public event EventHandler Tick;

        public Dispatcher Dispatcher { get; set; }

        public TimeSpan Interval { get; set; }

        public bool IsEnabled { get; set; }

        public object Tag { get; set; }

        public void Start()
        {
            WasStarted = true;
        }

        public bool WasStarted { get;  protected set; }

        public void Stop()
        {
            WasStopped = true;
        }

        public bool WasStopped { get;  protected set; }

        public void InvokeTick(object sender, EventArgs args)
        {
            Tick.Invoke(sender, args);
        }
        
        public void InvokeTick(EventArgs args)
        {
            InvokeTick(this, args);
        }

        public void InvokeTick()
        {
            InvokeTick(EventArgs.Empty);
        }

    }
}