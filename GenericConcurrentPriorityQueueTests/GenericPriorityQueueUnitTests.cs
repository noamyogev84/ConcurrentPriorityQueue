using System;
using System.Collections.Generic;
using System.Text;
using ConcurrentPriorityQueue.Core;
using Xunit;
using FluentAssertions;
using System.Linq;

namespace GenericConcurrentPriorityQueueTests
{
	/// <summary>
	/// Test general functionalty of the queue [Enqueue, Dequeue, Peek]
	/// </summary>
	public class GenericPriorityQueueUnitTests
	{
		private readonly IPriorityQueue<IHavePriority<TimeToProcess>> _targetQueue;

		public GenericPriorityQueueUnitTests()
		{
			var maxCapacity = 3;
			_targetQueue = new ConcurrentPriorityQueue<IHavePriority<TimeToProcess>, TimeToProcess>(maxCapacity);
		}

		[Fact]
		public void Enqueue_GetsValidPriorityItem_ReturnsSuccess()
		{
			// Assert
			TestHelpers.GetItemsWithObjectPriority().ForEach(i =>
			{
				var result = _targetQueue.Enqueue(i);
				result.IsSuccess.Should().BeTrue();
			});
		}

		[Fact]
		public void Enqueue_MaxCapacityReached_TryToEnqueueNewPriority_Failes()
		{
			// Arrange
			TestHelpers.GetItemsWithObjectPriority().ForEach(i => _targetQueue.Enqueue(i));


			var mockWithPriority = new MockWithObjectPriority(new TimeToProcess(1.5M));

			// Act
			var result = _targetQueue.Enqueue(mockWithPriority);

			// Assert
			result.IsFailure.Should().BeTrue();
		}

		[Fact]
		public void Enqueue_MaxCapacityReached_TryToEnqueueExistingPriority_Succeedes()
		{
			// Arrange
			TestHelpers.GetItemsWithObjectPriority().ForEach(i => _targetQueue.Enqueue(i));
			var mockWithPriority = TestHelpers.GetItemsWithObjectPriority().First();

			// Act
			var result = _targetQueue.Enqueue(mockWithPriority);

			// Assert
			result.IsSuccess.Should().BeTrue();
		}

		[Fact]
		public void Dequeue_ReturnsSuccessResultWithTopPriorityItem()
		{
			// Arrange
			var mockItems = TestHelpers.GetItemsWithObjectPriority();
			mockItems.ForEach(i => _targetQueue.Enqueue(i));

			// Act
			var result1 = _targetQueue.Dequeue();
			var result2 = _targetQueue.Dequeue();
			var result3 = _targetQueue.Dequeue();

			// Assert
			result1.Value.Priority.Should().Be(new TimeToProcess(0.25M));
			result2.Value.Priority.Should().Be(new TimeToProcess(0.5M));
			result3.Value.Priority.Should().Be(new TimeToProcess(1M));
		}

		[Fact]
		public void Dequeue_QueueIsEmptyReturnsFailureResult()
		{
			// Act
			var result = _targetQueue.Dequeue();
			;

			// Assert
			result.IsFailure.Should().BeTrue();
		}

		[Fact]
		public void Dequeue_QueueContainsItemsWithSamePriority_ReturnsSuccessWithFirstInItem()
		{
			// Arrange
			var mockWithPriority1 = new MockWithObjectPriority(new TimeToProcess(0.5M));
			var mockWithPriority2 = new MockWithObjectPriority(new TimeToProcess(0.5M));
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
			var mockItems = TestHelpers.GetItemsWithObjectPriority();
			mockItems.ForEach(i => _targetQueue.Enqueue(i));

			// Act
			var result = _targetQueue.Peek();

			// Assert
			result.Value.Priority.Should().Be(new TimeToProcess(0.25M));
		}

		[Fact]
		public void Peek_QueueContainsItemsWithSamePriority_ReturnsSuccessWithFirstInItem()
		{
			// Arrange
			var mockWithPriority1 = new MockWithObjectPriority(new TimeToProcess(0.5M));
			var mockWithPriority2 = new MockWithObjectPriority(new TimeToProcess(0.5M));
			_targetQueue.Enqueue(mockWithPriority1);
			_targetQueue.Enqueue(mockWithPriority2);

			// Act
			var result1 = _targetQueue.Dequeue();
			var result2 = _targetQueue.Dequeue();

			// Assert
			result1.Value.Should().Be(mockWithPriority1);
			result2.Value.Should().Be(mockWithPriority2);

		}
	}
}
