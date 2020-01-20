using System.Collections.Concurrent;
using System.Threading;
using BlockingCollectionExtensions.Structures;

namespace BlockingCollectionExtensions
{
    public static class TransformationUtilities
    {
        public static IProducerConsumerCollection<T> ToProducerConsumerCollection<T>(
            this BlockingCollection<T> collection, int millisecondsTimeout, CancellationToken cancellationToken,
            int count, bool isSynchronized, object syncRoot)
        {
            return new ProducerConsumerWrapper<T>(
                collection, millisecondsTimeout, cancellationToken, count, isSynchronized, syncRoot);
        }
    }
}