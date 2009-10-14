using System;
using System.Windows.Threading;

namespace Tools
{
    public interface IDispatcherTimer {
        void Start();
        void Stop();
        Dispatcher Dispatcher { get; }
        bool IsEnabled { get; set; }
        TimeSpan Interval { get; set; }
        object Tag { get; set; }
        event EventHandler Tick;
    }

    /// <summary>
    /// Adapts the Dispatcher class to implement the ITimer interface.
    /// </summary> 
    public class DispatcherTimerAdapter : DispatcherTimer, IDispatcherTimer
    {
        public DispatcherTimerAdapter() { }
        public DispatcherTimerAdapter(DispatcherPriority priority) : base(priority) { }
        public DispatcherTimerAdapter(DispatcherPriority priority, Dispatcher dispatcher) : base(priority, dispatcher) { }

        public DispatcherTimerAdapter(TimeSpan timeSpan,
                                      DispatcherPriority priority,
                                      EventHandler callback,
                                      Dispatcher dispatcher)
            : base(timeSpan, priority, callback, dispatcher) { }
    }

    public class FireOnStartDispatcherTimer : DispatcherTimer, IDispatcherTimer
    {

        public FireOnStartDispatcherTimer()
        {
            EnableOverride = false;
            IsEnabled = true;
        }

        public bool Stopped { get; set; }
        public bool EnableOverride { get; set; }

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

        public new event EventHandler Tick;
    }
}