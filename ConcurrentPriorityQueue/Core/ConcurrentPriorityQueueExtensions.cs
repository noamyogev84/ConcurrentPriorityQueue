using System;
using System.Collections.Concurrent;

namespace ConcurrentPriorityQueue.Core
{
    public static class ConcurrentPriorityQueueExtensions
    {
        public static BlockingCollection<T> ToBlockingCollection<T, TP>(this IConcurrentPriorityQueue<T, TP> queue)
            where T : IHavePriority<TP>
            where TP : IEquatable<TP>, IComparable<TP> => new BlockingCollection<T>(queue);
    }
}
