using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using FluentAssertions;
using ConcurrentPriorityQueue;

namespace ConcurrentPriorityQueueTests
{
	/// <summary>
	/// Test general functionalty of the queue [Enqueue, Dequeue, Peek]
	/// </summary>
	public class PriorityQueueIEnumerabeUnitTests
	{			
		private const int supportedNumberOfPriorites = 3;
		private readonly IConcurrentPriorityQueue<IHavePriority> _targetQueue;

		public PriorityQueueIEnumerabeUnitTests()
		{
			_targetQueue = new ConcurrentPriorityQueue<IHavePriority>(supportedNumberOfPriorites);
		}

		public static IEnumerable<object[]> MockItemsWithPriority =>
			new List<object[]>
			{
				new object[] { new MockWithPriority(0) },
				new object[] { new MockWithPriority(1) },
				new object[] { new MockWithPriority(2) },
			};

		[Fact]
		public void GetEnumerator_QueueContainsDifferentPriorities_EnumerationIsOrderedByPriority()
		{
			// Arrange
			MockItemsWithPriority.ToList().ForEach(items => _targetQueue.Enqueue(items[0] as IHavePriority));

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
		public void Count__ReturnsAggregatedCount()
		{
			// Arrange
			MockItemsWithPriority.ToList().ForEach(items => _targetQueue.Enqueue(items[0] as IHavePriority));

			// Act
			var totalCount = _targetQueue.Count;

			// Assert
			totalCount.Should().Be(MockItemsWithPriority.ToList().Count);
		}
	}
}
