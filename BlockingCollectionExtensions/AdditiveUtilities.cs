using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using BlockingCollectionExtensions.Structures;

namespace BlockingCollectionExtensions
{
    public static class AdditiveUtilities
    {
        /// <summary>
        /// Transfer contents of a generic enumerable into a target blocking collection,
        /// determine whether blocking collection should complete adding when enumerable has finished being added
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="completeAddingWhenDone"></param>
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

        /// <summary>
        /// Transfer contents arriving asynchronously in the form of an IObservable, subscribe a delegate that takes any data from the observable
        /// and adds it to the BlockingCollection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="completeAddingWhenDone"></param>
        /// <returns></returns>
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