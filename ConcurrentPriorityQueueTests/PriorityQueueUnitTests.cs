using System.Linq;
using ConcurrentPriorityQueue;
using ConcurrentPriorityQueue.Core;
using FluentAssertions;
using Xunit;

namespace ConcurrentPriorityQueueTests
{
    /// <summary>
    /// Test general functionalty of the queue [Enqueue, Dequeue, Peek].
    /// </summary>
    public class PriorityQueueUnitTests
    {
        private readonly IPriorityQueue<IHavePriority<int>> _targetQueue;

        public PriorityQueueUnitTests()
        {
            var maxCapacity = 3;
            _targetQueue = new ConcurrentPriorityByIntegerQueue<IHavePriority<int>>(maxCapacity);
        }

        [Fact]
        public void Enqueue_GetsValidPriorityItem_ReturnsSuccess()
        {
            // Assert
            TestHelpers.GetItemsWithIntegerPriority().ForEach(i =>
            {
                var result = _targetQueue.Enqueue(i);
                result.IsSuccess.Should().BeTrue();
            });
        }

        [Fact]
        public void Enqueue_MaxCapacityReached_TryToEnqueueNewPriority_Failes()
        {
            // Arrange
            TestHelpers.GetItemsWithIntegerPriority().ForEach(i => _targetQueue.Enqueue(i));

            var mockWithPriority = new MockWithIntegerPriority(10);

            // Act
            var result = _targetQueue.Enqueue(mockWithPriority);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Enqueue_MaxCapacityReached_TryToEnqueueExistingPriority_Succeedes()
        {
            // Arrange
            TestHelpers.GetItemsWithIntegerPriority().ForEach(i => _targetQueue.Enqueue(i));
            var mockWithPriority = TestHelpers.GetItemsWithIntegerPriority().First();

            // Act
            var result = _targetQueue.Enqueue(mockWithPriority);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Dequeue_ReturnsSuccessResultWithTopPriorityItem()
        {
            // Arrange
            var mockItems = TestHelpers.GetItemsWithIntegerPriority();
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
            var result = _targetQueue.Dequeue();

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Dequeue_QueueContainsItemsWithSamePriority_ReturnsSuccessWithFirstInItem()
        {
            // Arrange
            var mockWithPriority1 = new MockWithIntegerPriority(0);
            var mockWithPriority2 = new MockWithIntegerPriority(0);
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
            var mockItems = TestHelpers.GetItemsWithIntegerPriority();
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
            var result = _targetQueue.Peek();

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Peek_QueueContainsItemsWithSamePriority_ReturnsSuccessWithFirstInItem()
        {
            // Arrange
            var mockWithPriority1 = new MockWithIntegerPriority(0);
            var mockWithPriority2 = new MockWithIntegerPriority(0);
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
