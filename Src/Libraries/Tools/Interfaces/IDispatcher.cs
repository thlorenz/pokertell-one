namespace Tools.Interfaces
{
    using System;
    using System.Windows;
    using System.Windows.Threading;

    public interface IDispatcher
    {
        Dispatcher CurrentDispatcher { get; }
    }

    public class WindowsApplicationDispatcher : IDispatcher
    {
        public Dispatcher CurrentDispatcher
        {
            get { return Application.Current.Dispatcher; }
        }
    }

    public class WindowsThreadingDispatcher : IDispatcher
    {
        public Dispatcher CurrentDispatcher
        {
            get { return Dispatcher.CurrentDispatcher; }
        }
    }
}