using System;

namespace ConcurrentPriorityQueue.Core
{
    public interface IHavePriority<TP>
        where TP : IEquatable<TP>, IComparable<TP>
    {
        TP Priority { get; }
    }
}
