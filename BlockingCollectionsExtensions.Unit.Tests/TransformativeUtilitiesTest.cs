using System;
using System.Collections.Concurrent;
using System.Linq;
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

        [Test]
        public void TransformativeUtilities_ToProducerConsumerCollection_Timeout_Exception_Test()
        {
            // arrange
            var blockingCollection = new BlockingCollection<int>();

            const int count = 1000;

            const bool isSynchronized = true;

            // act
            // assert
            Assert.Throws<ArgumentOutOfRangeException>(() => blockingCollection.ToProducerConsumerCollection(0,
                CancellationToken.None, count, isSynchronized,
                null));
        }

        [Test]
        public void TransformativeUtilities_ToProducerConsumerCollection_TryAdd_Success_Test()
        {
            // arrange
            var blockingCollection = new BlockingCollection<int>();
            const int collectionItem = 0;

            const int count = 1000;

            const bool isSynchronized = true;

            var producerConsumerCollection =
                blockingCollection.ToProducerConsumerCollection(1000, CancellationToken.None, count, isSynchronized,
                    null);

            // act 
            var tryAdd = producerConsumerCollection.TryAdd(collectionItem);

            // assert
            producerConsumerCollection.ShouldNotBe(null);
            producerConsumerCollection.Count.ShouldBe(count);
            producerConsumerCollection.IsSynchronized.ShouldBe(isSynchronized);

            tryAdd.ShouldBe(true);
        }

        [Test]
        public void TransformativeUtilities_ToProducerConsumerCollection_TryAdd_TryTake_Success_Test()
        {
            // arrange
            var blockingCollection = new BlockingCollection<int>();
            const int collectionItem = 0;

            const int count = 1000;

            const bool isSynchronized = true;

            var producerConsumerCollection =
                blockingCollection.ToProducerConsumerCollection(1000, CancellationToken.None, count, isSynchronized,
                    null);

            var tryAdd = producerConsumerCollection.TryAdd(collectionItem);

            // act 
            var tryTake = producerConsumerCollection.TryTake(out var takenItem);

            // assert
            producerConsumerCollection.ShouldNotBe(null);
            producerConsumerCollection.Count.ShouldBe(count);
            producerConsumerCollection.IsSynchronized.ShouldBe(isSynchronized);

            tryAdd.ShouldBe(true);

            tryTake.ShouldBe(true);
            takenItem.ShouldBe(collectionItem);
        }

        [Test]
        public void TransformativeUtilities_ToProducerConsumerCollection_TryAdd_CopyTo_Success_Test()
        {
            // arrange
            var blockingCollection = new BlockingCollection<int>();
            const int collectionItem = 0;

            const int count = 1000;

            const bool isSynchronized = true;

            var producerConsumerCollection =
                blockingCollection.ToProducerConsumerCollection(1000, CancellationToken.None, count, isSynchronized,
                    null);

            var tryAdd = producerConsumerCollection.TryAdd(collectionItem);

            // act
            var array = new int[count];

            producerConsumerCollection.CopyTo(array, 0);

            // assert
            producerConsumerCollection.ShouldNotBe(null);
            producerConsumerCollection.Count.ShouldBe(count);
            producerConsumerCollection.IsSynchronized.ShouldBe(isSynchronized);

            tryAdd.ShouldBe(true);

            array.ShouldNotBeNull();
            array.FirstOrDefault().ShouldBe(collectionItem);
        }

        [Test]
        public void TransformativeUtilities_ToProducerConsumerCollection_TryAdd_ToArray_Success_Test()
        {
            // arrange
            var blockingCollection = new BlockingCollection<int>();
            const int collectionItem = 0;

            const int count = 1000;

            const bool isSynchronized = true;

            var producerConsumerCollection =
                blockingCollection.ToProducerConsumerCollection(1000, CancellationToken.None, count, isSynchronized,
                    null);

            var tryAdd = producerConsumerCollection.TryAdd(collectionItem);

            // act
            var array = producerConsumerCollection.ToArray();

            // assert
            producerConsumerCollection.ShouldNotBe(null);
            producerConsumerCollection.Count.ShouldBe(count);
            producerConsumerCollection.IsSynchronized.ShouldBe(isSynchronized);

            tryAdd.ShouldBe(true);

            array.ShouldNotBeNull();
            array.FirstOrDefault().ShouldBe(collectionItem);
        }
    }
}