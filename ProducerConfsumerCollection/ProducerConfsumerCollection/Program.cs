using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConfsumerCollection
{
    internal class Program
    {

        static void Main(string[] args)
        {
            StackConsumerProducer consumerProducer = new StackConsumerProducer();
            consumerProducer.Start();

            Console.ReadLine();
        }
    }

    public class BagConsumerProducer
    {
        ConcurrentBag<int> _bag;

        public BagConsumerProducer()
        {
            _bag = new ConcurrentBag<int>();
        }

        public void Start()
        {
            Task.Run(TryTake);
            Task.Run(Add);
        }

        private void Add()
        {
            Random random = new Random();
            while (true)
            {
                _bag.Add(DateTime.Now.Second);
                Thread.Sleep(random.Next(100, 3000));
            }
        }

        public void TryTake()
        {
            while (true)
            {
                var b = _bag.TryTake(out int res);
                Console.WriteLine($"Success: {b}, res: {res}");
                Thread.Sleep(10);
            }
        }
    }


    public class StackConsumerProducer
    {
        ConcurrentStack<int> _stack;

        public StackConsumerProducer()
        {
            _stack = new ConcurrentStack<int>();
        }

        public void Start()
        {
            Task.Run(Pop);
            Task.Run(Push);
            Task.Run(Push);
        }

        private void Push()
        {
            Random random = new Random();
            while (true)
            {
                var v = DateTime.Now.Millisecond;
                _stack.Push(v);
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} insert value {v}");
                Thread.Sleep(random.Next(500, 3000));
            }
        }

        public void Pop()
        {
            while (true)
            {
                var b = _stack.TryPop(out int res);
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Success: {b}, res: {res}");
                Thread.Sleep(1000);
            }
        }
    }
}
