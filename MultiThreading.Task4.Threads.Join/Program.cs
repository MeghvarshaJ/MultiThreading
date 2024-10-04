/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            // feel free to add your code
            // Option A: Using Thread class and Join
            Console.WriteLine("Using Thread class and Join:");
            CreateThreadsWithJoin(10);

            // Option B: Using ThreadPool class and Semaphore
            Console.WriteLine("\nUsing ThreadPool class and Semaphore:");
            CreateThreadsWithSemaphore(10);

            Console.ReadLine();
        }

        // Option A: Using Thread class and Join
        static void CreateThreadsWithJoin(int count)
        {
            if (count <= 0) return;

            Thread thread = new Thread(() =>
            {
                Console.WriteLine($"Thread {count} is running.");
                Thread.Sleep(500); // Simulate work
                CreateThreadsWithJoin(count - 1);
            });

            thread.Start();
            thread.Join(); // Wait for the thread to complete
        }

        // Option B: Using ThreadPool class and Semaphore
        static SemaphoreSlim semaphore = new SemaphoreSlim(1);

        static void CreateThreadsWithSemaphore(int count)
        {
            if (count <= 0) return;

            ThreadPool.QueueUserWorkItem(state =>
            {
                semaphore.Wait();
                try
                {
                    Console.WriteLine($"Thread {count} is running.");
                    Thread.Sleep(500); // Simulate work
                    CreateThreadsWithSemaphore(count - 1);
                }
                finally
                {
                    semaphore.Release();
                }
            });
        }
    }
}
