using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using BlockingCollectionExtensions;
using NUnit.Framework;
using Shouldly;

namespace BlockingCollectionsExtensions.Unit.Tests;

public class TransformativeUtilitiesTest
{
    [Test]
    public void TransformativeUtilities_ToProducerConsumerCollection_Success_Test()
    {
        var blockingCollection = new BlockingCollection<int>();

        var producerConsumerCollection =
            blockingCollection.ToProducerConsumerCollection(TimeSpan.FromSeconds(1), CancellationToken.None);

        producerConsumerCollection.ShouldSatisfyAllConditions(
            () => producerConsumerCollection.ShouldNotBeNull(),
            () => producerConsumerCollection.Count.ShouldBe(0),
            () => producerConsumerCollection.IsSynchronized.ShouldBeFalse());
    }

    [Test]
    public void TransformativeUtilities_ToProducerConsumerCollection_Timeout_Exception_Test()
    {
        var blockingCollection = new BlockingCollection<int>();

        Should.Throw<ArgumentOutOfRangeException>(
            () => blockingCollection.ToProducerConsumerCollection(TimeSpan.Zero, CancellationToken.None));
    }

    [Test]
    public void TransformativeUtilities_ToProducerConsumerCollection_TryAdd_Success_Test()
    {
        var blockingCollection = new BlockingCollection<int>();
        const int collectionItem = 0;

        var producerConsumerCollection =
            blockingCollection.ToProducerConsumerCollection(TimeSpan.FromSeconds(1), CancellationToken.None);

        var tryAdd = producerConsumerCollection.TryAdd(collectionItem);

        producerConsumerCollection.Count.ShouldBe(1);
        tryAdd.ShouldBeTrue();
    }

    [Test]
    public void TransformativeUtilities_ToProducerConsumerCollection_TryAdd_TryTake_Success_Test()
    {
        var blockingCollection = new BlockingCollection<int>();
        const int collectionItem = 0;

        var producerConsumerCollection =
            blockingCollection.ToProducerConsumerCollection(TimeSpan.FromSeconds(1), CancellationToken.None);

        var tryAdd = producerConsumerCollection.TryAdd(collectionItem);
        var tryTake = producerConsumerCollection.TryTake(out var takenItem);

        tryAdd.ShouldBeTrue();
        tryTake.ShouldBeTrue();
        takenItem.ShouldBe(collectionItem);
        producerConsumerCollection.Count.ShouldBe(0);
    }

    [Test]
    public void TransformativeUtilities_ToProducerConsumerCollection_TryAdd_CopyTo_Success_Test()
    {
        var blockingCollection = new BlockingCollection<int>();
        const int collectionItem = 0;

        var producerConsumerCollection =
            blockingCollection.ToProducerConsumerCollection(TimeSpan.FromSeconds(1), CancellationToken.None);

        producerConsumerCollection.TryAdd(collectionItem).ShouldBeTrue();

        var array = new int[1];
        producerConsumerCollection.CopyTo(array, 0);

        array.ShouldNotBeNull();
        array.FirstOrDefault().ShouldBe(collectionItem);
    }

    [Test]
    public void TransformativeUtilities_ToProducerConsumerCollection_TryAdd_ToArray_Success_Test()
    {
        var blockingCollection = new BlockingCollection<int>();
        const int collectionItem = 0;

        var producerConsumerCollection =
            blockingCollection.ToProducerConsumerCollection(TimeSpan.FromSeconds(1), CancellationToken.None);

        producerConsumerCollection.TryAdd(collectionItem).ShouldBeTrue();

        var array = producerConsumerCollection.ToArray();

        array.ShouldNotBeNull();
        array.FirstOrDefault().ShouldBe(collectionItem);
    }

    [Test]
    public void TransformativeUtilities_AsProducerConsumerCollection_Success_Test()
    {
        var blockingCollection = new BlockingCollection<int>();

        var producerConsumerCollection =
            blockingCollection.AsProducerConsumerCollection(TimeSpan.FromMilliseconds(100));

        producerConsumerCollection.ShouldNotBeNull();
    }
}
