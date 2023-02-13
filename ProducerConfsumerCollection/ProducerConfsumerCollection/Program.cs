using System;
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
            StackConsumerProducer stackConsumerProducer = new StackConsumerProducer();
            stackConsumerProducer.Start();

            Console.ReadLine();
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
        }

        private void Push()
        {
            Random random = new Random();
            while (true)
            {
                _stack.Push(DateTime.Now.Second);
                Thread.Sleep(random.Next(100, 3000));
            }
        }

        public void Pop()
        {
            while (true)
            {
                var b = _stack.TryPop(out int res);
                Console.WriteLine($"Success: {b}, res: {res}");
                Thread.Sleep(10);
            }
        }
    }
}
