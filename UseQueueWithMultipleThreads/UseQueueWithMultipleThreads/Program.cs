using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UseQueueWithMultipleThreads
{
    public class Robot
    {
        public string Name { get; set; }
        public int Speed { get; set; }
    }

    internal class Program
    {
        static Queue<Robot> _robots = new Queue<Robot>();

        static void Main(string[] args)
        {
            //Demo1();
            Demo2();

            Console.ReadLine();
        }

        private static void Demo2()
        {
            try
            {
                var t1 = Task.Run(Setup1);
                var t2 = Task.Run(Setup2);
                Console.WriteLine("Start wait all");
                Task.WaitAll(t1, t2);
                Console.WriteLine("Finished wait all");
                while (_robots.Count > 0)
                {
                    Console.WriteLine(_robots.Dequeue().Speed);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                while (_robots.Count > 0)
                {
                    Console.WriteLine(_robots.Dequeue().Speed);
                }
            }
        }

        private static void Demo1()
        {
            Setup1();
            Setup2();

            while (_robots.Count > 0)
            {
                Console.WriteLine(_robots.Dequeue().Speed);
            }
        }

        private static void Setup2()
        {
            for (int i = 20; i < 27; i++)
            {
                Console.WriteLine($"Thread ID: {Thread.CurrentThread.ManagedThreadId} ADD {i} to queue with length {_robots.Count}");
                Thread.Sleep(1);
                _robots.Enqueue(new Robot() { Speed = i });
            }
        }

        private static void Setup1()
        {
            for (int i = 10; i < 16; i++)
            {
                Console.WriteLine($"Thread ID: {Thread.CurrentThread.ManagedThreadId} ADD {i} to queue with length {_robots.Count}");
                Thread.Sleep(1);
                _robots.Enqueue(new Robot() { Speed = i });
            }
        }
    }
}
