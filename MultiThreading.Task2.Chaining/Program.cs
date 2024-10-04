/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            // feel free to add your code
            ChainTasks();

            Console.ReadLine();
        }

        static void ChainTasks()
        {
            // 1st Task: Create an array of 10 random integers
            var task1 = Task.Run(() =>
            {
                Random rand = new Random();
                int[] randomArray = Enumerable.Range(0, 10).Select(_ => rand.Next(1, 101)).ToArray();
                Console.WriteLine($"Task 1 - Random Array: {string.Join(", ", randomArray)}");
                return randomArray;
            });

            // 2nd Task: Multiply the array with another random integer
            var task2 = task1.ContinueWith(previousTask =>
            {
                Random rand = new Random();
                int multiplier = rand.Next(1, 11); // Random integer between 1 and 10
                int[] multipliedArray = previousTask.Result.Select(x => x * multiplier).ToArray();
                Console.WriteLine($"Task 2 - Multiplier: {multiplier}");
                Console.WriteLine($"Task 2 - Multiplied Array: {string.Join(", ", multipliedArray)}");
                return multipliedArray;
            });

            // 3rd Task: Sort the array in ascending order
            var task3 = task2.ContinueWith(previousTask =>
            {
                int[] sortedArray = previousTask.Result.OrderBy(x => x).ToArray();
                Console.WriteLine($"Task 3 - Sorted Array: {string.Join(", ", sortedArray)}");
                return sortedArray;
            });

            // 4th Task: Calculate the average value
            task3.ContinueWith(previousTask =>
            {
                double average = previousTask.Result.Average();
                Console.WriteLine($"Task 4 - Average Value: {average}");
            }).Wait(); // Wait for the final task to complete
        }
    }
}
