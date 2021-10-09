using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConcurrentPriorityQueue;
using ConcurrentPriorityQueue.Core;
using FluentAssertions;
using Xunit;

namespace ConcurrentPriorityQueueTests
{
    /// <summary>
    /// Test Blocking collection functionality.
    /// </summary>
    public class PriorityQueueBlockingCollectionUnitTests : IDisposable
    {
        private readonly BlockingCollection<IHavePriority<int>> _targetCollection;

        public PriorityQueueBlockingCollectionUnitTests()
        {
            _targetCollection = new ConcurrentPriorityByIntegerQueue<IHavePriority<int>>()
                .ToBlockingCollection();
        }

        [Fact]
        public async Task Test_ConsumingTaskBlocksUntilNoMoreAdditions()
        {
            const int numberOfItemsToAdd = 10;
            const int defaultSleepTimeBetweenAdds = 1000;

            // Add items and signal for completion when done.
            await Task.Run(() => AddItems(numberOfItemsToAdd, defaultSleepTimeBetweenAdds))
                .ContinueWith(t => _targetCollection.CompleteAdding()).ConfigureAwait(false);

            // Take items as long as they continue to be added.
            await Task.Run(() => TakeItems()).ConfigureAwait(false);

            // Wait for all tasks to end.
            await Task.WhenAll(AddItems(numberOfItemsToAdd, defaultSleepTimeBetweenAdds), TakeItems()).ConfigureAwait(false);
        }

        public void Dispose() => _targetCollection.Dispose();

        private async Task AddItems(int numberOfItemsToAdd, int sleepTime = 0)
        {
            for (var i = 0; i < numberOfItemsToAdd; ++i)
            {
                _targetCollection.Add(new MockWithIntegerPriority());
                await Task.Delay(sleepTime).ConfigureAwait(false);
            }
        }

        private Task TakeItems()
        {
            // blocks until signaled on completion.
            foreach (var item in _targetCollection.GetConsumingEnumerable())
            {
                item.Should().BeOfType<MockWithIntegerPriority>();
            }

            return Task.CompletedTask;
        }
    }
}
