/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            // feel free to add your code
            // Create a shared collection
            ConcurrentBag<int> sharedCollection = new ConcurrentBag<int>();
            object lockObject = new object();

            // Task to add elements
            Task addingTask = Task.Run(() =>
            {
                for (int i = 1; i <= 10; i++)
                {
                    sharedCollection.Add(i);
                    Thread.Sleep(500); // Simulate work

                    // Print the current collection
                    lock (lockObject)
                    {
                        Console.WriteLine($"Current Collection: [{string.Join(", ", sharedCollection)}]");
                    }
                }
            });

            // Task to print elements
            Task printingTask = Task.Run(() =>
            {
                while (!addingTask.IsCompleted)
                {
                    // Simply wait for a bit to avoid busy waiting
                    Thread.Sleep(100);
                }
            });

            // Wait for the adding task to complete
            Task.WaitAll(addingTask, printingTask);

            Console.WriteLine("All elements added and printed.");

            Console.ReadLine();
        }
    }
}
