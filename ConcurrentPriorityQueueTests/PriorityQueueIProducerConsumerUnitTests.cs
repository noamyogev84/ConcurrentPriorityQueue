using ConcurrentPriorityQueue;
using Xunit;
using FluentAssertions;
using System;

namespace ConcurrentPriorityQueueTests
{
	public class PriorityQueueIProducerConsumerUnitTests
	{
		private readonly IConcurrentPriorityQueue<IHavePriority<int>, int> _targetQueue;

		public PriorityQueueIProducerConsumerUnitTests()
		{
			_targetQueue = new ConcurrentPriorityByIntegerQueue<IHavePriority<int>>();
		}

		[Fact]
		public void ToArray_QueueContainsElements_ReturnsItemsArray()
		{
			// Arrange
			var mockItems = TestHelpers.GetItemsWithIntegerPriority();
			mockItems.ForEach(i => _targetQueue.Enqueue(i));

			// Arrange
			var result = _targetQueue.ToArray();

			// Assert
			result.Length.Should().Be(mockItems.Count);
			result.Should().BeEquivalentTo(mockItems.ToArray());
		}

		[Fact]
		public void ToArray_QueueContainsNoElements_ReturnsEmptyArray()
		{
			// Arrange
			var result = _targetQueue.ToArray();

			// Assert
			result.Should().BeEmpty();
		}

		[Fact]
		public void CopyTo_GetsArray_CopiesAllItems()
		{
			// Arrange
			var mockItems = TestHelpers.GetItemsWithIntegerPriority();
			mockItems.ForEach(i => _targetQueue.Enqueue(i));
			var newArray = new IHavePriority<int>[3];

			// Act
			_targetQueue.CopyTo(newArray, 0);

			// Assert
			newArray.Length.Should().Be(mockItems.Count);
			newArray.Should().BeEquivalentTo(mockItems.ToArray());
		}

		[Fact]
		public void CopyTo_GetsArrayType_CopiesAllItems()
		{
			// Arrange
			var mockItems = TestHelpers.GetItemsWithIntegerPriority();
			mockItems.ForEach(i => _targetQueue.Enqueue(i));
			var newArray = Array.CreateInstance(typeof(IHavePriority<int>), mockItems.Count);

			// Act
			_targetQueue.CopyTo(newArray, 0);

			// Assert
			newArray.Length.Should().Be(mockItems.Count);
			newArray.Should().BeEquivalentTo(mockItems.ToArray());
		}
	}
}
