using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using BlockingCollectionExtensions;
using NUnit.Framework;
using Shouldly;

namespace BlockingCollectionsExtensions.Unit.Tests;

public class AdditiveUtilitiesTest
{
    [Test]
    public void AdditiveUtilities_AddFromEnumerable_CompleteAddingWhenDone_IsFalse_Success_Test()
    {
        var blockingCollection = new BlockingCollection<int>();
        var enumerable = new List<int> { 100, 200, 300, 400, 500 };

        blockingCollection.AddFromEnumerable(enumerable, completeAddingWhenDone: false);

        blockingCollection.Count.ShouldBe(enumerable.Count);
        blockingCollection.IsAddingCompleted.ShouldBeFalse();
    }

    [Test]
    public void AdditiveUtilities_AddFromEnumerable_CompleteAddingWhenDone_IsTrue_Success_Test()
    {
        var blockingCollection = new BlockingCollection<int>();
        var enumerable = new List<int> { 100, 200, 300, 400, 500 };

        blockingCollection.AddFromEnumerable(enumerable, completeAddingWhenDone: true);

        blockingCollection.Count.ShouldBe(enumerable.Count);
        blockingCollection.IsAddingCompleted.ShouldBeTrue();
    }

    [Test]
    public void AdditiveUtilities_AddFromEnumerable_CancellationRequested_Throws_And_CompletesAdding_WhenRequested()
    {
        var blockingCollection = new BlockingCollection<int>();
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        Should.Throw<OperationCanceledException>(
            () => blockingCollection.AddFromEnumerable(new[] { 1, 2, 3 }, cancellationTokenSource.Token, true));

        blockingCollection.IsAddingCompleted.ShouldBeTrue();
    }

    [Test]
    public async Task AdditiveUtilities_AddFromAsyncEnumerable_Success_Test()
    {
        var blockingCollection = new BlockingCollection<int>();

        await blockingCollection.AddFromAsyncEnumerable(CreateAsyncEnumerable(), completeAddingWhenDone: true);

        blockingCollection.ToArray().ShouldBe(new[] { 10, 20, 30 });
        blockingCollection.IsAddingCompleted.ShouldBeTrue();
    }

    [Test]
    public void AdditiveUtilities_AddFromObservable_CompletesAdding_OnCompleted()
    {
        var blockingCollection = new BlockingCollection<int>();
        var source = new TestObservable<int>(observer =>
        {
            observer.OnNext(5);
            observer.OnNext(6);
            observer.OnCompleted();
        });

        using var _ = blockingCollection.AddFromObservable(source, completeAddingWhenDone: true);

        blockingCollection.ToArray().ShouldBe(new[] { 5, 6 });
        blockingCollection.IsAddingCompleted.ShouldBeTrue();
    }

    private static async IAsyncEnumerable<int> CreateAsyncEnumerable()
    {
        yield return 10;
        await Task.Yield();
        yield return 20;
        await Task.Yield();
        yield return 30;
    }

    private sealed class TestObservable<T>(Action<IObserver<T>> onSubscribe) : IObservable<T>
    {
        public IDisposable Subscribe(IObserver<T> observer)
        {
            onSubscribe(observer);
            return new TestDisposable();
        }
    }

    private sealed class TestDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
