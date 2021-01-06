using System.Collections.Concurrent;
using System.Threading;
using BlockingCollectionExtensions.Structures;

namespace BlockingCollectionExtensions
{
    public static class TransformativeUtilities
    {
        /// <summary>
        /// Coalesce a target blocking collection to a structure that implements the IProducerConsumerCollection interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="count"></param>
        /// <param name="isSynchronized"></param>
        /// <param name="syncRoot"></param>
        /// <returns></returns>
        public static IProducerConsumerCollection<T> ToProducerConsumerCollection<T>(
            this BlockingCollection<T> collection, int millisecondsTimeout, CancellationToken cancellationToken,
            int count, bool isSynchronized, object syncRoot)
        {
            return new ProducerConsumerWrapper<T>(
                collection, millisecondsTimeout, cancellationToken, count, isSynchronized, syncRoot);
        }
    }
}