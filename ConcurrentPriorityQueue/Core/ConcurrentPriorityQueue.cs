using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace ConcurrentPriorityQueue.Core
{
    public class ConcurrentPriorityQueue<T, TP> : IConcurrentPriorityQueue<T, TP>
        where T : IHavePriority<TP>
        where TP : IEquatable<TP>, IComparable<TP>
    {
        private readonly Dictionary<TP, Queue<T>> _internalQueues;
        private readonly int _capacity;

        public ConcurrentPriorityQueue(int capacity = 0)
        {
            _internalQueues = new Dictionary<TP, Queue<T>>();
            _capacity = capacity;
        }

        public int Count => _internalQueues.Values.Sum((q) => q.Count);

        public bool IsSynchronized => true;

        public object SyncRoot { get; } = new object();

        public void CopyTo(T[] array, int index) => ToArray().CopyTo(array, index);

        public void CopyTo(Array array, int index) => CopyTo((T[])array, index);

        public T[] ToArray()
        {
            lock (SyncRoot)
            {
                return _internalQueues.OrderBy((q) => q.Key).SelectMany((q) => q.Value).ToArray();
            }
        }

        public bool TryAdd(T item) => Enqueue(item).IsSuccess;

        public bool TryTake(out T item)
        {
            var result = Dequeue();

            if (result.IsFailure)
            {
                item = default;
                return false;
            }
            else
            {
                item = result.Value;
                return true;
            }
        }

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)ToArray()).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Result Enqueue(T item)
        {
            lock (SyncRoot)
            {
                return AddOrUpdate(item);
            }
        }

        public Result<T> Dequeue()
        {
            lock (SyncRoot)
            {
                return GetNextQueue()
                    .Map(q => q.Dequeue())
                    .TapError(err => Result.Failure(err));
            }
        }

        public Result<T> Peek()
        {
            lock (SyncRoot)
            {
                return GetNextQueue()
                    .Map(q => q.Peek())
                    .TapError(err => Result.Failure(err));
            }
        }

        private Result<Queue<T>> GetNextQueue()
        {
            return _internalQueues.OrderBy((q) => q.Key).Select((q) => q.Value).FirstOrDefault((q) => q.Count > 0) is Queue<T> queue
                ? Result.Success(queue)
                : Result.Failure<Queue<T>>("Could not find a queue with items.");
        }

        private Result AddOrUpdate(T item)
        {
            if (_internalQueues.TryGetValue(item.Priority, out Queue<T> queue))
            {
                queue.Enqueue(item);
                return Result.Success();
            }
            else if (!IsAtMaxCapacity())
            {
                Queue<T> newQueue = new Queue<T>();
                newQueue.Enqueue(item);
                _internalQueues.Add(item.Priority, newQueue);
                return Result.Success();
            }
            else
            {
                return Result.Failure("Reached max capacity.");
            }
        }

        private bool IsAtMaxCapacity() => _capacity != 0 && Count == _capacity;
    }
}
