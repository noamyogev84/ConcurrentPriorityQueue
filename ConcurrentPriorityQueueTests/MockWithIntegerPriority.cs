using ConcurrentPriorityQueue.Core;

namespace ConcurrentPriorityQueueTests
{
    public class MockWithIntegerPriority : IHavePriority<int>
    {
        public MockWithIntegerPriority(int priority = 0)
        {
            Priority = priority;
        }

        public int Priority { get; set; }
    }
}
