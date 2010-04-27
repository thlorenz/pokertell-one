namespace PokerTell.UnitTests.Fakes
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Remoting;

    using global::Tools.Interfaces;

    public class BackgroundWorkerMock : IBackgroundWorker
    {
        public bool CancelAsyncInvoked;

        public bool DisposeInvoked;

        public bool ReportProgressInvoked;

        public int ReportProgressInvokedWithPercent;

        public object ReportProgressInvokedWithUserState;

        public bool RunWorkerAsyncInvoked;

        public object RunWorkerAsyncInvokedWithArgument;

        public event EventHandler Disposed;

        void InvokeDisposed(EventArgs e)
        {
            EventHandler disposed = Disposed;
            if (disposed != null) disposed(this, e);
        }

        public event DoWorkEventHandler DoWork
        {
            add { DoWorkEventHandler = value; }
            remove { throw new NotImplementedException(); }
        }

        public event ProgressChangedEventHandler ProgressChanged
        {
            add { ProgressChangedEventHandler = value; }
            remove { throw new NotImplementedException(); }
        }

        public event RunWorkerCompletedEventHandler RunWorkerCompleted
        {
            add { RunWorkerCompletedEventHandler = value; }
            remove { throw new NotImplementedException(); }
        }

        public bool CancellationPending { get; set; }

        public IContainer Container { get; set; }

        public bool IsBusy { get; set; }

        public ISite Site { get; set; }

        public bool WorkerReportsProgress { get; set; }

        public bool WorkerSupportsCancellation { get; set; }

        protected DoWorkEventHandler DoWorkEventHandler { get; set; }

        protected ProgressChangedEventHandler ProgressChangedEventHandler { get; set; }

        protected RunWorkerCompletedEventHandler RunWorkerCompletedEventHandler { get; set; }

        public void RunWorkerCompletedInvoke(RunWorkerCompletedEventArgs e)
        {
            RunWorkerCompletedEventHandler(this, e);
        }

        public void CancelAsync()
        {
            CancelAsyncInvoked = true;
        }

        public ObjRef CreateObjRef(Type requestedType)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            DisposeInvoked = true;
        }

        public object GetLifetimeService()
        {
            throw new NotImplementedException();
        }

        public object InitializeLifetimeService()
        {
            throw new NotImplementedException();
        }

        public void ReportProgress(int percentProgress)
        {
            ReportProgressInvoked = true;
            ReportProgressInvokedWithPercent = percentProgress;
            ProgressChangedEventHandler(this, new ProgressChangedEventArgs(percentProgress, null));
        }

        public void ReportProgress(int percentProgress, object userState)
        {
            ReportProgressInvoked = true;
            ReportProgressInvokedWithPercent = percentProgress;
            ReportProgressInvokedWithUserState = userState;
            ProgressChangedEventHandler(this, new ProgressChangedEventArgs(percentProgress, userState));
        }

        public void RunWorkerAsync()
        {
            RunWorkerAsyncInvoked = true;
            DoWorkEventHandler(this, new DoWorkEventArgs(null));
        }

        public void RunWorkerAsync(object argument)
        {
            RunWorkerAsyncInvoked = true;
            RunWorkerAsyncInvokedWithArgument = argument;
            DoWorkEventHandler(this, new DoWorkEventArgs(argument));
        }
    }
}