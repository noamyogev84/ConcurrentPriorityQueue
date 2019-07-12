using Xunit;
using ConcurrentPriorityQueue;
using System.Collections.Generic;
using FluentAssertions;
using System.Linq;


namespace ConcurrentPriorityQueueTests
{
	/// <summary>
	/// Test general functionalty of the queue [Enqueue, Dequeue, Peek]
	/// </summary>
	public class PriorityQueueUnitTests
	{
		private const int supportedNumberOfPriorites = 3;
		private readonly IPriorityQueue<IHavePriority> _targetQueue;

		public PriorityQueueUnitTests()
		{
			_targetQueue = new ConcurrentPriorityQueue<IHavePriority>(supportedNumberOfPriorites);
		}

		[Fact]
		public void Enqueue_GetsValidPriorityItem_ReturnsSuccess()
		{			
			// Assert
			TestHelpers.GetItemsWithPriority().ForEach(i =>
			{
				var result = _targetQueue.Enqueue(i);
				result.IsSuccess.Should().BeTrue();
			});
		}

		[Fact]
		public void Enqueue_GetsInvalidPriorityItem_ReturnsFailure()
		{
			// Arrange
			var mockWithPriority = new MockWithPriority(10);

			// Act
			var result = _targetQueue.Enqueue(mockWithPriority);

			// Assert
			result.IsFailure.Should().BeTrue();
		}

		[Fact]
		public void Dequeue_ReturnsSuccessResultWithTopPriorityItem()
		{
			// Arrange
			var mockItems = TestHelpers.GetItemsWithPriority();
			mockItems.ForEach(i => _targetQueue.Enqueue(i));

			// Act
			var result1 = _targetQueue.Dequeue();
			var result2 = _targetQueue.Dequeue();
			var result3 = _targetQueue.Dequeue();

			// Assert
			result1.Value.Priority.Should().Be(0);
			result2.Value.Priority.Should().Be(1);
			result3.Value.Priority.Should().Be(2);
		}

		[Fact]
		public void Dequeue_QueueIsEmptyReturnsFailureResult()
		{
			// Act
			var result = _targetQueue.Dequeue();;

			// Assert
			result.IsFailure.Should().BeTrue();
		}

		[Fact]
		public void Dequeue_QueueContainsItemsWithSamePriority_ReturnsSuccessWithFirstInItem()
		{
			// Arrange
			var mockWithPriority1 = new MockWithPriority(0);
			var mockWithPriority2 = new MockWithPriority(0);
			_targetQueue.Enqueue(mockWithPriority1);
			_targetQueue.Enqueue(mockWithPriority2);
			
			// Act
			var result1 = _targetQueue.Dequeue();
			var result2 = _targetQueue.Dequeue();

			// Assert
			result1.Value.Should().Be(mockWithPriority1);
			result2.Value.Should().Be(mockWithPriority2);
		}

		[Fact]
		public void Peek_ReturnsSuccessResultWithTopPriorityItem()
		{
			// Arrange
			var mockItems = TestHelpers.GetItemsWithPriority();
			mockItems.ForEach(i => _targetQueue.Enqueue(i));

			// Act
			var result = _targetQueue.Peek();

			// Assert
			result.Value.Priority.Should().Be(0);
		}

		[Fact]
		public void Peek_QueueIsEmptyReturnsFailureResult()
		{
			// Act
			var result = _targetQueue.Peek(); ;

			// Assert
			result.IsFailure.Should().BeTrue();
		}

		[Fact]
		public void Peek_QueueContainsItemsWithSamePriority_ReturnsSuccessWithFirstInItem()
		{
			// Arrange
			var mockWithPriority1 = new MockWithPriority(0);
			var mockWithPriority2 = new MockWithPriority(0);
			_targetQueue.Enqueue(mockWithPriority1);
			_targetQueue.Enqueue(mockWithPriority2);

			// Act
			var result = _targetQueue.Dequeue();

			// Assert
			result.Value.Should().Be(mockWithPriority1);
		}
	}
}
