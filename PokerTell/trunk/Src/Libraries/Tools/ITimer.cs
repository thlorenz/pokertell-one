/*
 * User: Thorsten Lorenz
 * Date: 6/22/2009
 * 
 */

#region Using Directives

using System;
using System.ComponentModel;
using System.Timers;
using System.Windows.Threading;

#endregion

namespace Tools
{
    public interface ITimer : IDisposable
    {
        bool Enabled { get; set; }
        double Interval { get; set; }
        void Start();
        void Stop();

        event ElapsedEventHandler Elapsed;
    }

    public class FireOnStartTimer : Timer, ITimer
    {
        public FireOnStartTimer()
        {
            EnableOverride = false;
            Enabled = true;
        }

        public FireOnStartTimer(double interval, ISynchronizeInvoke syncObject) : this()
        {
            SynchronizingObject = syncObject;
            AutoReset = false;
            Interval = interval;
        }

        public bool Stopped { get; set; }
        public bool EnableOverride { get; set; }

        #region ITimer Members

        public new event ElapsedEventHandler Elapsed;

        public new void Start()
        {
            Stopped = false;
            if (!EnableOverride && Enabled && Elapsed != null)
            {
                Elapsed.Invoke(this, new EventArgs() as ElapsedEventArgs);
            }
        }

        public new void Stop()
        {
            Stopped = true;
        }

        #endregion
    }

    /// <summary>
    /// Adapts the System.Timers.Timer class to implement the ITimer interface.
    /// </summary> 
    public class SystemTimerAdapter : Timer, ITimer
    {
        public SystemTimerAdapter() {}
        public SystemTimerAdapter(double interval) : base(interval) {}
    }

    
}