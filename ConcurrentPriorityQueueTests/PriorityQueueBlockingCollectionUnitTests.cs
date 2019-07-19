using Xunit;
using FluentAssertions;
using ConcurrentPriorityQueue;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System;

namespace ConcurrentPriorityQueueTests
{
	/// <summary>
	/// Test Blocking collection functionality
	/// </summary>
	public class PriorityQueueBlockingCollectionUnitTests : IDisposable
	{
		private const int supportedNumberOfPriorites = 3;
		private readonly BlockingCollection<IHavePriority> _targetCollection;

		public PriorityQueueBlockingCollectionUnitTests()
		{
			_targetCollection = new ConcurrentPriorityQueue<IHavePriority>(supportedNumberOfPriorites)
				.ToBlockingCollection();
		}

		[Fact]
		public void Test_ConsumingTaskBlocksUntilNoMoreAdditions()
		{
			const int numberOfItemsToAdd = 10;
			const int defaultSleepTimeBetweenAdds = 1000;

			// Add items and signal for completion when done.
			Task.Run(() => AddItems(numberOfItemsToAdd, defaultSleepTimeBetweenAdds))
				.ContinueWith(t => _targetCollection.CompleteAdding());

			// Take items as long as they continue to be added.
			Task.Run(() => TakeItems());			

			// Wait for all tasks to end.
			Task.WaitAll(AddItems(numberOfItemsToAdd, defaultSleepTimeBetweenAdds), TakeItems());
		}

		public void Dispose() => _targetCollection.Dispose();

		private async Task AddItems(int numberOfItemsToAdd, int sleepTime = 0)
		{
			const int numberOfItems = 10;
			for (var i = 0; i < numberOfItems; ++i)
			{
				_targetCollection.Add(new MockWithPriority());
				await Task.Delay(sleepTime);
			}
		}

		private Task TakeItems()
		{
			// blocks until signaled on completion.
			foreach (var item in _targetCollection.GetConsumingEnumerable())
			{
				item.Should().BeOfType<MockWithPriority>();
			}

			return Task.CompletedTask;
		}
	}
}
