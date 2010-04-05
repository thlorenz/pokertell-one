namespace Tools.Interfaces
{
    using System;
    using System.Windows.Threading;

    public interface IDispatcherTimer
    {
        event EventHandler Tick;

        Dispatcher Dispatcher { get; }

        TimeSpan Interval { get; set; }

        bool IsEnabled { get; set; }

        object Tag { get; set; }

        void Start();

        void Stop();
    }

    /// <summary>
    /// Adapts the DispatcherTimer class to implement the <see cref="IDispatcherTimer"/> interface.
    /// </summary> 
    public class DispatcherTimerAdapter : DispatcherTimer, IDispatcherTimer
    {
    }
}