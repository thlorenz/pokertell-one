namespace Tools.Interfaces
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Remoting;

    public interface IBackgroundWorker
    {
        void CancelAsync();

        void ReportProgress(int percentProgress);

        void ReportProgress(int percentProgress, object userState);

        void RunWorkerAsync();

        void RunWorkerAsync(object argument);

        bool CancellationPending { get; }

        bool IsBusy { get; }

        bool WorkerReportsProgress { get; set; }

        bool WorkerSupportsCancellation { get; set; }

        ISite Site { get; set; }

        IContainer Container { get; }

        event DoWorkEventHandler DoWork;

        event ProgressChangedEventHandler ProgressChanged;

        event RunWorkerCompletedEventHandler RunWorkerCompleted;

        void Dispose();

        string ToString();

        event EventHandler Disposed;

        object GetLifetimeService();

        object InitializeLifetimeService();

        ObjRef CreateObjRef(Type requestedType);
    }

    public class BackgroundWorkerAdapter : BackgroundWorker, IBackgroundWorker
    {
    }
}