using ConcurrentPriorityQueue.Core;

namespace ConcurrentPriorityQueue
{
    // new changes
    public class ConcurrentPriorityByIntegerQueue<T> : ConcurrentPriorityQueue<T, int>
        where T : IHavePriority<int>
    {
        public ConcurrentPriorityByIntegerQueue()
            : base() { }

        public ConcurrentPriorityByIntegerQueue(int capacity)
            : base(capacity) { }
    }
}
