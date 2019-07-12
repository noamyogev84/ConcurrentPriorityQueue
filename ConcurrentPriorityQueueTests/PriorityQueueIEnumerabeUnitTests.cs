using System.Collections.Generic;
using Xunit;
using System.Linq;
using FluentAssertions;
using ConcurrentPriorityQueue;

namespace ConcurrentPriorityQueueTests
{
	/// <summary>
	/// Test IEnumerable functionality
	/// </summary>
	public class PriorityQueueIEnumerabeUnitTests
	{			
		private const int supportedNumberOfPriorites = 3;
		private readonly IConcurrentPriorityQueue<IHavePriority> _targetQueue;

		public PriorityQueueIEnumerabeUnitTests()
		{
			_targetQueue = new ConcurrentPriorityQueue<IHavePriority>(supportedNumberOfPriorites);
		}

		[Fact]
		public void GetEnumerator_QueueContainsDifferentPriorities_EnumerationIsOrderedByPriority()
		{
			// Arrange
			var mockItems = TestHelpers.GetItemsWithPriority();
			mockItems.ForEach(i => _targetQueue.Enqueue(i));

			// Assert
			var expectedPriority = 0;
			foreach (var item in _targetQueue)
			{
				item.Priority.Should().Be(expectedPriority++);
			}
		}

		[Fact]
		public void GetEnumerator_QueueContainsSamePriorities_EnumerationIsOrderedByFirstIn()
		{
			// Arrange
			var mockWithPriority1 = new MockWithPriority(0);
			var mockWithPriority2 = new MockWithPriority(0);

			_targetQueue.Enqueue(mockWithPriority1);
			_targetQueue.Enqueue(mockWithPriority2);

			// Act
			var enumerator = _targetQueue.GetEnumerator();

			// Assert
			enumerator.MoveNext();
			enumerator.Current.Should().Be(mockWithPriority1);
			enumerator.MoveNext();
			enumerator.Current.Should().Be(mockWithPriority2);
		}

		[Fact]
		public void Count_ReturnsAggregatedCount()
		{
			// Arrange
			var mockItems = TestHelpers.GetItemsWithPriority();
			mockItems.ForEach(i => _targetQueue.Enqueue(i));

			// Act
			var totalCount = _targetQueue.Count;

			// Assert
			totalCount.Should().Be(mockItems.Count);
		}
	}
}
