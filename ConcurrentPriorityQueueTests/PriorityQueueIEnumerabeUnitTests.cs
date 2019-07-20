using Xunit;
using FluentAssertions;
using ConcurrentPriorityQueue;

namespace ConcurrentPriorityQueueTests
{
	/// <summary>
	/// Test IEnumerable functionality
	/// </summary>
	public class PriorityQueueIEnumerabeUnitTests
	{			
		private readonly IConcurrentPriorityQueue<IHavePriority<int>, int> _targetQueue;

		public PriorityQueueIEnumerabeUnitTests()
		{
			_targetQueue = new ConcurrentPriorityByIntegerQueue<IHavePriority<int>>();
		}

		[Fact]
		public void GetEnumerator_QueueContainsDifferentPriorities_EnumerationIsOrderedByPriority()
		{
			// Arrange
			var mockItems = TestHelpers.GetItemsWithIntegerPriority();
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
			var mockWithPriority1 = new MockWithIntegerPriority(0);
			var mockWithPriority2 = new MockWithIntegerPriority(0);

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
			var mockItems = TestHelpers.GetItemsWithIntegerPriority();
			mockItems.ForEach(i => _targetQueue.Enqueue(i));

			// Act
			var totalCount = _targetQueue.Count;

			// Assert
			totalCount.Should().Be(mockItems.Count);
		}
	}
}
