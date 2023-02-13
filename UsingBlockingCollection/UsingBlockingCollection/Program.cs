
using System.Collections.Concurrent;
var consumer = new BlockingCollectionProducerConsumer();
Console.ReadKey();

public class BlockingCollectionProducerConsumer
{
    BlockingCollection<int> _blockingCollection;

    public BlockingCollectionProducerConsumer()
    {
        ConcurrentQueue<int> _queue = new ConcurrentQueue<int>();
        _blockingCollection = new BlockingCollection<int>(_queue);
        _blockingCollection.TryAdd(1);
        _blockingCollection.Add(2);
        var i = _blockingCollection.Take();
        i = _blockingCollection.Take();
        _blockingCollection.TryTake(out i, 2000);

    }

}