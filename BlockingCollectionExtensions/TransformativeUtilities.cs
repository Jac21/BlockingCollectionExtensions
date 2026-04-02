using System.Collections.Concurrent;
using BlockingCollectionExtensions.Structures;

namespace BlockingCollectionExtensions;

/// <summary>
/// Utility methods for adapting <see cref="BlockingCollection{T}"/> instances to consumer-friendly abstractions.
/// </summary>
public static class TransformativeUtilities
{
    /// <summary>
    /// Adapt a blocking collection to <see cref="IProducerConsumerCollection{T}"/> using a timeout in milliseconds.
    /// </summary>
    public static IProducerConsumerCollection<T> ToProducerConsumerCollection<T>(
        this BlockingCollection<T> collection,
        int millisecondsTimeout,
        CancellationToken cancellationToken = default)
    {
        if (collection is null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        return new ProducerConsumerWrapper<T>(collection, millisecondsTimeout, cancellationToken);
    }

    /// <summary>
    /// Adapt a blocking collection to <see cref="IProducerConsumerCollection{T}"/> using a <see cref="TimeSpan"/> timeout.
    /// </summary>
    public static IProducerConsumerCollection<T> ToProducerConsumerCollection<T>(
        this BlockingCollection<T> collection,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        if (collection is null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        return new ProducerConsumerWrapper<T>(collection, timeout, cancellationToken);
    }

    /// <summary>
    /// Modern alias for converting a blocking collection to <see cref="IProducerConsumerCollection{T}"/>.
    /// </summary>
    public static IProducerConsumerCollection<T> AsProducerConsumerCollection<T>(
        this BlockingCollection<T> collection,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        return ToProducerConsumerCollection(collection, timeout, cancellationToken);
    }

    /// <summary>
    /// Legacy overload retained for compatibility. Prefer overloads that derive metadata from the collection.
    /// </summary>
    [Obsolete("Use an overload that accepts only timeout and optional cancellationToken. Metadata is derived from the collection.")]
    public static IProducerConsumerCollection<T> ToProducerConsumerCollection<T>(
        this BlockingCollection<T> collection,
        int millisecondsTimeout,
        CancellationToken cancellationToken,
        int count,
        bool isSynchronized,
        object? syncRoot)
    {
        return ToProducerConsumerCollection(collection, millisecondsTimeout, cancellationToken);
    }
}
