using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using BlockingCollectionExtensions.Structures;

namespace BlockingCollectionExtensions
{
    public static class AdditiveUtilities
    {
        public static void AddFromEnumerable<T>(this BlockingCollection<T> target, IEnumerable<T> source,
            bool completeAddingWhenDone)
        {
            try
            {
                foreach (var item in source)
                {
                    target.Add(item);
                }
            }
            finally
            {
                if (completeAddingWhenDone)
                {
                    target.CompleteAdding();
                }
            }
        }

        public static IDisposable AddFromObservable<T>(this BlockingCollection<T> target, IObservable<T> source,
            bool completeAddingWhenDone)
        {
            return source.Subscribe(new DelegateBasedObserver<T>
            (
                target.Add,
                error =>
                {
                    if (completeAddingWhenDone) target.CompleteAdding();
                },
                () =>
                {
                    if (completeAddingWhenDone) target.CompleteAdding();
                }
            ));
        }
    }
}