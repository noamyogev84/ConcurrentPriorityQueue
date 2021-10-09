using System;
using ConcurrentPriorityQueue.Core;
using FluentAssertions;
using Xunit;

namespace GenericConcurrentPriorityQueueTests
{
    public class GenericPriorityQueueIProducerConsumerUnitTests
    {
        private readonly IConcurrentPriorityQueue<IHavePriority<TimeToProcess>, TimeToProcess> _targetQueue;

        public GenericPriorityQueueIProducerConsumerUnitTests()
        {
            _targetQueue = new ConcurrentPriorityQueue<IHavePriority<TimeToProcess>, TimeToProcess>();
        }

        [Fact]
        public void ToArray_QueueContainsElements_ReturnsItemsArray()
        {
            // Arrange
            var mockItems = TestHelpers.GetItemsWithObjectPriority();
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
            var mockItems = TestHelpers.GetItemsWithObjectPriority();
            mockItems.ForEach(i => _targetQueue.Enqueue(i));
            var newArray = new IHavePriority<TimeToProcess>[3];

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
            var mockItems = TestHelpers.GetItemsWithObjectPriority();
            mockItems.ForEach(i => _targetQueue.Enqueue(i));
            var newArray = Array.CreateInstance(typeof(IHavePriority<TimeToProcess>), mockItems.Count);

            // Act
            _targetQueue.CopyTo(newArray, 0);

            // Assert
            newArray.Length.Should().Be(mockItems.Count);
            newArray.Should().BeEquivalentTo(mockItems.ToArray());
        }
    }
}
