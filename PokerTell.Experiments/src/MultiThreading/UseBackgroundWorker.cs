namespace MultiThreading
{
    using System;
    using System.ComponentModel;
    using System.Threading;

    public class UseBackgroundWorker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UseBackgroundWorker"/> class. 
        /// 
        /// The BackgroundWorker component works well with WPF because underneath the covers it uses the AsyncOperationManager class, 
        /// which in turn uses the SynchronizationContext class to deal with synchronization. In Windows Forms, 
        /// the AsyncOperationManager hands off a WindowsFormsSynchronizationContext class that derives from the SynchronizationContext class. 
        /// 
        /// Likewise, in ASP.NET it works with a different derivation of SynchronizationContext called AspNetSynchronizationContext. 
        /// These SynchronizationContext-derived classes know how to handle the cross-thread synchronization of method invocation.
        /// In WPF, this model is extended with a DispatcherSynchronizationContext class. By using BackgroundWorker, 
        /// the Dispatcher is being employed automatically to invoke cross-thread method calls.
        /// </summary>
        public UseBackgroundWorker()
        {
            Console.WriteLine("Running on thread: {0}", Thread.CurrentThread.ManagedThreadId);
            var worker = new BackgroundWorker();

            worker.DoWork += (s, e) => {
                Console.WriteLine("Executing on thread: {0}", Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(3000);
            };

            worker.RunWorkerCompleted += (s, e) => Console.WriteLine("Completed on thread {0}", Thread.CurrentThread.ManagedThreadId);

            Console.WriteLine("Starting worker");
            worker.RunWorkerAsync();

            Console.WriteLine("Continuing on main thread");
        }
    }
}