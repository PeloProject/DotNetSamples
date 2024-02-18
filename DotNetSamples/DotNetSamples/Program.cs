using System.Threading;

namespace DotNetSamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(100, 100);
            for (int i = 0; i < 25; i++)
            {
                // have to wait or threadpool never gives out threads to requests
                //Thread.Sleep(50); // queue thread request
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc), i);
            } // have to wait here or the background threads in the thread // pool would not run before the main thread exits.

            Console.WriteLine("Main Thread waiting to complete...");
            bool working = true;
            int workerThreads = 0;
            int completionPortThreads = 0;
            int maxWorkerThreads = 0;
            int maxCompletionPortThreads = 0; // get max threads in the pool
            ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxCompletionPortThreads);
            while (working)
            { // get available threads
                ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
                if (workerThreads == maxWorkerThreads)
                {
                    Console.WriteLine("Thread complete!!");
                    break;
                }
            }
        }

        private static Object lockObj = new Object();
        // This thread procedure performs the task.
        static void ThreadProc(Object stateInfo)
        {
            
            // No state object was passed to QueueUserWorkItem, so stateInfo is null.
            Console.WriteLine($"Hello from the thread pool. {stateInfo} , {ThreadPool.CompletedWorkItemCount}, {ThreadPool.ThreadCount}, {ThreadPool.PendingWorkItemCount}, {Thread.CurrentThread.ManagedThreadId}, {Thread.GetCurrentProcessorId()}");
            //String msg = null;
            //Thread thread = Thread.CurrentThread;
            //lock (lockObj)
            //{
            //    msg = String.Format("{0} thread information\n", Thread.CurrentThread.Name) +
            //          String.Format("   Background: {0}\n", thread.IsBackground) +
            //          String.Format("   Thread Pool: {0}\n", thread.IsThreadPoolThread) +
            //          String.Format("   Thread ID: {0}\n", thread.ManagedThreadId);
            //}
            //Console.WriteLine(msg);
        }
    }

}
