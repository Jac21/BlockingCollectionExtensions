using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace BlockingCollectionExtensions.Structures
{
    internal sealed class ProducerConsumerWrapper<T> :
        IProducerConsumerCollection<T>
    {
        private readonly BlockingCollection<T> _collection;

        private readonly int _millisecondsTimeout;

        private readonly CancellationToken _cancellationToken;

        public int Count { get; }

        public bool IsSynchronized { get; }

        public object SyncRoot { get; }

        /// <inheritdoc />
        public ProducerConsumerWrapper(
            BlockingCollection<T> collection, int millisecondsTimeout,
            CancellationToken cancellationToken, int count, bool isSynchronized, object syncRoot)

        {
            if (millisecondsTimeout < -1)
                throw new ArgumentOutOfRangeException(
                    nameof(millisecondsTimeout));

            _collection = collection ?? throw new ArgumentNullException(nameof(collection));

            _millisecondsTimeout = millisecondsTimeout;

            _cancellationToken = cancellationToken;

            Count = count;
            IsSynchronized = isSynchronized;
            SyncRoot = syncRoot;
        }

        public bool TryAdd(T item)

        {
            return _collection.TryAdd(
                item, _millisecondsTimeout,
                _cancellationToken);
        }

        public bool TryTake(out T item)

        {
            return _collection.TryTake(
                out item, _millisecondsTimeout,
                _cancellationToken);
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
            return _collection.GetConsumingEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            lock (SyncRoot)
            {
                _collection.CopyTo((T[]) array, index);
            }
        }
    }
}