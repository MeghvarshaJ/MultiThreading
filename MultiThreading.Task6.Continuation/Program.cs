/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading.Tasks;
using System.Threading;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            // feel free to add your code
            // Case A: Continuation regardless of the result
            Console.WriteLine("Case A: Continuation regardless of the result");
            Task parentTaskA = Task.Run(() =>
            {
                Console.WriteLine("Parent Task A is running.");
                Thread.Sleep(1000);
            });

            parentTaskA.ContinueWith(t =>
            {
                Console.WriteLine("Continuation A executed regardless of parent task result.");
            });

            Task.WaitAll(parentTaskA); // Wait for the parent task to complete

            // Case B: Continuation when the task finishes without success
            Console.WriteLine("\nCase B: Continuation when the task finishes without success");
            Task parentTaskB = Task.Run(() =>
            {
                Console.WriteLine("Parent Task B is running.");
                Thread.Sleep(1000);
                // Simulate failure
                throw new Exception("Simulated failure in Parent Task B.");
            });

            parentTaskB.ContinueWith(t =>
            {
                Console.WriteLine("Continuation B executed because parent task finished without success.");
            });

            try
            {
                Task.WaitAll(parentTaskB);
            }
            catch (AggregateException)
            {
                // Handle the exception
            }

            // Case C: Continuation when the task fails, using the same thread
            Console.WriteLine("\nCase C: Continuation when the task fails and using the same thread");
            Task parentTaskC = Task.Run(() =>
            {
                Console.WriteLine("Parent Task C is running.");
                Thread.Sleep(1000);
                // Simulate failure
                throw new Exception("Simulated failure in Parent Task C.");
            });

            parentTaskC.ContinueWith(t =>
            {
                Console.WriteLine("Continuation C executed in the same thread because parent task failed.");
            }, TaskContinuationOptions.ExecuteSynchronously);

            try
            {
                Task.WaitAll(parentTaskC);
            }
            catch (AggregateException)
            {
                // Handle the exception
            }

            // Case D: Continuation when the task is cancelled
            Console.WriteLine("\nCase D: Continuation when the task is cancelled");
            CancellationTokenSource cts = new CancellationTokenSource();
            Task parentTaskD = Task.Run(() =>
            {
                Console.WriteLine("Parent Task D is running.");
                Thread.Sleep(1000);
                cts.Token.ThrowIfCancellationRequested();
            }, cts.Token);

            // Cancel the task
            cts.Cancel();

            parentTaskD.ContinueWith(t =>
            {
                if (t.IsCanceled)
                {
                    Console.WriteLine("Continuation D executed outside of the thread pool because parent task was cancelled.");
                }
            }, TaskContinuationOptions.OnlyOnCanceled);

            // Wait for the parent task to complete
            try
            {
                Task.WaitAll(parentTaskD);
            }
            catch (AggregateException)
            {
                // Handle the exception
            }

            Console.ReadLine();
        }
    }
}
