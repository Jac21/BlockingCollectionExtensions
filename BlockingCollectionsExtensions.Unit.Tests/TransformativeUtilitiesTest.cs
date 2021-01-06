using System.Collections.Concurrent;
using System.Threading;
using BlockingCollectionExtensions;
using NUnit.Framework;
using Shouldly;

namespace BlockingCollectionsExtensions.Unit.Tests
{
    public class TransformativeUtilitiesTest
    {
        [Test]
        public void TransformativeUtilities_ToProducerConsumerCollection_Success_Test()
        {
            // arrange
            var blockingCollection = new BlockingCollection<int>();

            const int count = 1000;

            const bool isSynchronized = true;

            // act
            var producerConsumerCollection =
                blockingCollection.ToProducerConsumerCollection(1000, CancellationToken.None, count, isSynchronized,
                    null);

            // assert
            producerConsumerCollection.ShouldNotBe(null);
            producerConsumerCollection.Count.ShouldBe(count);
            producerConsumerCollection.IsSynchronized.ShouldBe(isSynchronized);
        }
    }
}