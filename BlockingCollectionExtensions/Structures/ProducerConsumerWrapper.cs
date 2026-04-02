using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BlockingCollectionExtensions.Structures;

internal sealed class ProducerConsumerWrapper<T> : IProducerConsumerCollection<T>
{
    private readonly BlockingCollection<T> _collection;
    private readonly int _millisecondsTimeout;
    private readonly CancellationToken _cancellationToken;
    private readonly object _syncRoot = new();

    public ProducerConsumerWrapper(
        BlockingCollection<T> collection,
        int millisecondsTimeout,
        CancellationToken cancellationToken)
    {
        if (millisecondsTimeout <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(millisecondsTimeout));
        }

        _collection = collection ?? throw new ArgumentNullException(nameof(collection));
        _millisecondsTimeout = millisecondsTimeout;
        _cancellationToken = cancellationToken;
    }

    public ProducerConsumerWrapper(
        BlockingCollection<T> collection,
        TimeSpan timeout,
        CancellationToken cancellationToken)
        : this(collection, ValidateTimeout(timeout), cancellationToken)
    {
    }

    public int Count => _collection.Count;

    public bool IsSynchronized => false;

    public object SyncRoot => _syncRoot;

    public bool TryAdd(T item)
    {
        return _collection.TryAdd(item, _millisecondsTimeout, _cancellationToken);
    }

    public bool TryTake(out T item)
    {
        if (_collection.TryTake(out var localItem, _millisecondsTimeout, _cancellationToken))
        {
            item = localItem;
            return true;
        }

        item = default!;
        return false;
    }

    public void CopyTo(T[] array, int index)
    {
        _collection.CopyTo(array, index);
    }

    public T[] ToArray()
    {
        return _collection.ToArray();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)_collection).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void CopyTo(Array array, int index)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        lock (_syncRoot)
        {
            if (array is not T[] typedArray)
            {
                throw new ArgumentException("Array must be compatible with the collection item type.", nameof(array));
            }

            _collection.CopyTo(typedArray, index);
        }
    }

    private static int ValidateTimeout(TimeSpan timeout)
    {
        var milliseconds = (long)timeout.TotalMilliseconds;
        if (milliseconds <= 0 || milliseconds > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout));
        }

        return (int)milliseconds;
    }
}
