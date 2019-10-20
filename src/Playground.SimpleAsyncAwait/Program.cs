using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Playgound.SimpleAsyncAwait
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Since this is a console application, this particular thread is not part of the thread pool, but rather a \"regular\" thread");
            Console.WriteLine("In this demo, we will create a bunch of tasks synchronously and add them to a list and wait for all of them to finish.");
            Console.WriteLine("The interesting point is that even though all of the queued-up work is very bad contain, and using .Result on the async work,");
            Console.WriteLine("we will see that it will not cause a deadlock for the application itself, but rather just cause all tasks to sort of complete successfully, ");
            Console.WriteLine("just not in an optimal way.");
            Console.WriteLine("");
            Console.WriteLine("This code demonstrates that the use of .Result from a console applications perspective is not VERY bad, but just bad.");
            Console.WriteLine("");

            Console.WriteLine($"ThreadPool.ThreadCount: {ThreadPool.ThreadCount}");
            Console.WriteLine("");

            var tasks = new List<Task>(1000);

            for (var i = 0; i < 10000; i++)
            {
                var wheel = new MyObject();

                var task = wheel.Delay();

                tasks.Add(task);
            }


            Console.WriteLine($"ThreadPool.ThreadCount: {ThreadPool.ThreadCount}");
            Console.WriteLine("Main: okey, listening for key stroke");
            Task.WhenAll(tasks).GetAwaiter().GetResult();

            Console.WriteLine($"ThreadPool.ThreadCount: {ThreadPool.ThreadCount}");
            Console.ReadKey();
        }
    }
}