/*
 * User: Thorsten Lorenz
 * Date: 6/22/2009
 * 
 */

namespace Tools.Interfaces
{
    using System;
    using System.ComponentModel;
    using System.Timers;

    public interface ITimer : IDisposable
    {
        #region Events

        event ElapsedEventHandler Elapsed;

        #endregion

        #region Properties

        bool Enabled { get; set; }

        double Interval { get; set; }

        #endregion

        #region Public Methods

        void Start();

        void Stop();

        #endregion
    }

    public class FireOnStartTimer : Timer, ITimer
    {
        #region Constructors and Destructors

        public FireOnStartTimer()
        {
            EnableOverride = false;
            Enabled = true;
        }

        public FireOnStartTimer(double interval, ISynchronizeInvoke syncObject)
            : this()
        {
            SynchronizingObject = syncObject;
            AutoReset = false;
            Interval = interval;
        }

        #endregion

        #region Events

        public new event ElapsedEventHandler Elapsed;

        #endregion

        #region Properties

        public bool EnableOverride { get; set; }

        public bool Stopped { get; set; }

        #endregion

        #region Implemented Interfaces

        #region ITimer

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

        #endregion
    }

    /// <summary>
    /// Adapts the System.Timers.Timer class to implement the ITimer interface.
    /// </summary> 
    public class SystemTimerAdapter : Timer, ITimer
    {
        #region Constructors and Destructors

        public SystemTimerAdapter()
        {
        }

        public SystemTimerAdapter(double interval)
            : base(interval)
        {
        }

        #endregion
    }
}