namespace Tools.Interfaces
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Remoting;

    public interface IBackgroundWorker
    {
        event EventHandler Disposed;

        event DoWorkEventHandler DoWork;

        event ProgressChangedEventHandler ProgressChanged;

        event RunWorkerCompletedEventHandler RunWorkerCompleted;

        bool CancellationPending { get; }

        IContainer Container { get; }

        bool IsBusy { get; }

        ISite Site { get; set; }

        bool WorkerReportsProgress { get; set; }

        bool WorkerSupportsCancellation { get; set; }

        void CancelAsync();

        ObjRef CreateObjRef(Type requestedType);

        void Dispose();

        object GetLifetimeService();

        object InitializeLifetimeService();

        void ReportProgress(int percentProgress);

        void ReportProgress(int percentProgress, object userState);

        void RunWorkerAsync();

        void RunWorkerAsync(object argument);

        string ToString();
    }

    public class BackgroundWorkerAdapter : BackgroundWorker, IBackgroundWorker
    {
    }
}