using ConcurrentPriorityQueue.Core;
using FluentAssertions;
using Xunit;

namespace GenericConcurrentPriorityQueueTests
{
    /// <summary>
    /// Test IEnumerable functionality.
    /// </summary>
    public class GenericPriorityQueueIEnumerabeUnitTests
    {
        private readonly IConcurrentPriorityQueue<IHavePriority<TimeToProcess>, TimeToProcess> _targetQueue;

        public GenericPriorityQueueIEnumerabeUnitTests()
        {
            _targetQueue = new ConcurrentPriorityQueue<IHavePriority<TimeToProcess>, TimeToProcess>();
        }

        [Fact]
        public void GetEnumerator_QueueContainsDifferentPriorities_EnumerationIsOrderedByPriority()
        {
            // Arrange
            var mockItems = TestHelpers.GetItemsWithObjectPriority();
            mockItems.ForEach(i => _targetQueue.Enqueue(i));

            // Assert
            var index = 0;
            foreach (var item in _targetQueue)
            {
                item.Priority.Should().Be(mockItems[index++].Priority);
            }
        }

        [Fact]
        public void GetEnumerator_QueueContainsSamePriorities_EnumerationIsOrderedByFirstIn()
        {
            // Arrange
            var mockWithPriority1 = new MockWithObjectPriority(new TimeToProcess(0.25M));
            var mockWithPriority2 = new MockWithObjectPriority(new TimeToProcess(0.25M));

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
            var mockItems = TestHelpers.GetItemsWithObjectPriority();
            mockItems.ForEach(i => _targetQueue.Enqueue(i));

            // Act
            var totalCount = _targetQueue.Count;

            // Assert
            totalCount.Should().Be(mockItems.Count);
        }
    }
}
