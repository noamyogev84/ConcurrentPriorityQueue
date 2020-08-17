using System;
using System.Collections.Concurrent;

namespace ConcurrentPriorityQueue.Core
{
    public static class ConcurrentPriorityQueueExtensions
    {
        public static BlockingCollection<T> ToBlockingCollection<T, TP>(this IConcurrentPriorityQueue<T, TP> queue)
            where T : IHavePriority<TP>
            where TP : IEquatable<TP>, IComparable<TP>
        {
            if (queue.Capacity > 0)
            {
                return new BlockingCollection<T>(queue, queue.Capacity);
            }
            return new BlockingCollection<T>(queue);
        }

        public static BlockingCollection<T> ToBlockingCollection<T, TP>(this IConcurrentPriorityQueue<T, TP> queue, int capacity)
            where T : IHavePriority<TP>
            where TP : IEquatable<TP>, IComparable<TP>
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity");
            }
            return new BlockingCollection<T>(queue, capacity);

        }
    }
}
