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

        public int Count => _internalQueues.Count == 0 ? _internalQueues.Count :
            _internalQueues.Values.Select(q => q.Count).Aggregate((a, b) => a + b);

        public bool IsSynchronized => true;

        public object SyncRoot { get; } = new object();

        public void CopyTo(T[] array, int index)
        {
            var itemsArray = ToArray();
            lock (SyncRoot)
                itemsArray.CopyTo(array, index);
        }

        public void CopyTo(Array array, int index) => CopyTo((T[])array, index);

        public T[] ToArray()
        {
            lock (SyncRoot)
            {
                var itemsList = new List<T>();
                foreach (var queue in _internalQueues.OrderBy(q => q.Key).Select(q => q.Value))
                {
                    itemsList.AddRange(queue.ToArray());
                }

                return itemsList.ToArray();
            }
        }

        public bool TryAdd(T item) => Enqueue(item).IsSuccess;

        public bool TryTake(out T item)
        {
            var result = Dequeue();
            lock (SyncRoot)
            {
                item = default;
                if (result.IsFailure)
                    return false;

                item = result.Value;
                return true;
            }
        }

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)ToArray()).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Result Enqueue(T item)
        {
            lock (SyncRoot) return AddOrUpdate(item);
        }

        public Result<T> Dequeue()
        {
            lock (SyncRoot)
            {
                return GetNextQueue()
                    .OnSuccess(q => q.Dequeue())
                    .OnFailure(err => Result.Fail(err));
            }
        }

        public Result<T> Peek()
        {
            lock (SyncRoot)
            {
                return GetNextQueue()
                    .OnSuccess(q => q.Peek())
                    .OnFailure(err => Result.Fail(err));
            }
        }

        private Result<Queue<T>> GetNextQueue()
        {
            foreach (var queue in _internalQueues.OrderBy(q => q.Key).Select(q => q.Value))
            {
                try
                {
                    queue.Peek();
                    return Result.Ok(queue);
                }
                catch (Exception) { }
            }

            return Result.Fail<Queue<T>>("Could not find a queue with items.");
        }

        private Result AddOrUpdate(T item)
        {
            if (!_internalQueues.ContainsKey(item.Priority))
            {
                if (IsAtMaxCapacity())
                    return Result.Fail("Reached max capacity.");
                _internalQueues.Add(item.Priority, new Queue<T>());
            }

            _internalQueues[item.Priority].Enqueue(item);
            return Result.Ok();
        }

        private bool IsAtMaxCapacity() => _capacity != 0 && Count == _capacity;
    }
}
