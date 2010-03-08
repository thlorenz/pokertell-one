namespace MultiThreading
{
    using System;
    using System.Threading;

    public class UseBeginInvoke
    {
        
        delegate void WorkerDelegate();

        static readonly WorkerDelegate Worker = new WorkerDelegate(() => {
                Console.WriteLine("Executing on thread: {0}", Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(3000);
        });

        public UseBeginInvoke()
        {
            Console.WriteLine("Running on thread: {0}", Thread.CurrentThread.ManagedThreadId);
            
            Worker.BeginInvoke(WorkerIsDone, null);
            Console.WriteLine("Continuing on main thread");
        }

        static void WorkerIsDone(IAsyncResult result)
        {
            Console.WriteLine("Called back on thread {0}", Thread.CurrentThread.ManagedThreadId);
            Worker.EndInvoke(result);

            Console.WriteLine("End invoked, now on thread {0}", Thread.CurrentThread.ManagedThreadId);

            // At this point I am still on the async thread, so UI ops shouldn't be done from here
        }
    }
}