using System.Collections.Concurrent;
using System.Collections.Generic;
using BlockingCollectionExtensions;
using NUnit.Framework;
using Shouldly;

namespace BlockingCollectionsExtensions.Unit.Tests
{
    public class AdditiveUtilitiesTest
    {
        [Test]
        public void AdditiveUtilities_AddFromEnumerable_CompleteAddingWhenDone_IsFalse_Success_Test()
        {
            // arrange
            var blockingCollection = new BlockingCollection<int>();

            var enumerable = new List<int>
            {
                100, 200, 300, 400, 500
            };

            const bool completeAddingWhenDone = false;

            // act
            blockingCollection.AddFromEnumerable(enumerable, completeAddingWhenDone);

            // assert
            blockingCollection.Count.ShouldBe(enumerable.Count);
            blockingCollection.IsAddingCompleted.ShouldBe(completeAddingWhenDone);
        }

        [Test]
        public void AdditiveUtilities_AddFromEnumerable_CompleteAddingWhenDone_IsTrue_Success_Test()
        {
            // arrange
            var blockingCollection = new BlockingCollection<int>();

            var enumerable = new List<int>
            {
                100, 200, 300, 400, 500
            };

            const bool completeAddingWhenDone = true;

            // act
            blockingCollection.AddFromEnumerable(enumerable, completeAddingWhenDone);

            // assert
            blockingCollection.Count.ShouldBe(enumerable.Count);
            blockingCollection.IsAddingCompleted.ShouldBe(completeAddingWhenDone);
        }
    }
}