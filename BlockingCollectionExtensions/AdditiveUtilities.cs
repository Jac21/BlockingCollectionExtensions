using System.Collections.Concurrent;
using BlockingCollectionExtensions.Structures;

namespace BlockingCollectionExtensions;

/// <summary>
/// Utility methods for adding data to <see cref="BlockingCollection{T}"/> instances.
/// </summary>
public static class AdditiveUtilities
{
    /// <summary>
    /// Transfer contents of an enumerable into a target blocking collection.
    /// </summary>
    public static void AddFromEnumerable<T>(
        this BlockingCollection<T> target,
        IEnumerable<T> source,
        bool completeAddingWhenDone = false)
    {
        AddFromEnumerable(target, source, CancellationToken.None, completeAddingWhenDone);
    }

    /// <summary>
    /// Transfer contents of an enumerable into a target blocking collection with cancellation support.
    /// </summary>
    public static void AddFromEnumerable<T>(
        this BlockingCollection<T> target,
        IEnumerable<T> source,
        CancellationToken cancellationToken,
        bool completeAddingWhenDone = false)
    {
        if (target is null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        try
        {
            foreach (var item in source)
            {
                cancellationToken.ThrowIfCancellationRequested();
                target.Add(item, cancellationToken);
            }
        }
        finally
        {
            CompleteAddingIfRequested(target, completeAddingWhenDone);
        }
    }

    /// <summary>
    /// Transfer contents from an async enumerable into a target blocking collection.
    /// </summary>
    public static async Task AddFromAsyncEnumerable<T>(
        this BlockingCollection<T> target,
        IAsyncEnumerable<T> source,
        CancellationToken cancellationToken = default,
        bool completeAddingWhenDone = false)
    {
        if (target is null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        try
        {
            await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                target.Add(item, cancellationToken);
            }
        }
        finally
        {
            CompleteAddingIfRequested(target, completeAddingWhenDone);
        }
    }

    /// <summary>
    /// Subscribe to an observable and add received values to a blocking collection.
    /// </summary>
    public static IDisposable AddFromObservable<T>(
        this BlockingCollection<T> target,
        IObservable<T> source,
        bool completeAddingWhenDone = false)
    {
        if (target is null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return source.Subscribe(
            new DelegateBasedObserver<T>(
                target.Add,
                _ => CompleteAddingIfRequested(target, completeAddingWhenDone),
                () => CompleteAddingIfRequested(target, completeAddingWhenDone)));
    }

    private static void CompleteAddingIfRequested<T>(BlockingCollection<T> target, bool completeAddingWhenDone)
    {
        if (completeAddingWhenDone && !target.IsAddingCompleted)
        {
            target.CompleteAdding();
        }
    }
}
