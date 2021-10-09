﻿using System;
using System.Collections.Concurrent;

namespace ConcurrentPriorityQueue.Core
{
    public interface IConcurrentPriorityQueue<T, TP> : IProducerConsumerCollection<T>, IPriorityQueue<T>
        where T : IHavePriority<TP>
        where TP : IEquatable<TP>, IComparable<TP>
    {
    }
}
