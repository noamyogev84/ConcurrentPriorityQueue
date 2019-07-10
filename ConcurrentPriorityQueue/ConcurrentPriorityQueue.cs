using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace ConcurrentPriorityQueue
{
	public class ConcurrentPriorityQueue<T> : IConcurrentPriorityQueue<T> where T : IHavePriority
	{
		private readonly Dictionary<int, Queue<T>> _internalQueues;
		private readonly object _syncRoot = new object();

		public int Count => _internalQueues.Values.Select(q => q.Count).Aggregate((a, b) => a + b);

		public bool IsSynchronized => true;

		public object SyncRoot => _syncRoot;

		public ConcurrentPriorityQueue(int supportedNumberOfPriorites = 1)
		{
			_internalQueues = Enumerable.Range(0, supportedNumberOfPriorites)
				.ToDictionary(k => k, v => new Queue<T>());
		}

		public void CopyTo(T[] array, int index)
		{
			var itemsArray = ToArray();
			lock (_syncRoot)
				itemsArray.CopyTo(array, index);
		}

		public void CopyTo(Array array, int index) => CopyTo((T[])array, index);
		
		public T[] ToArray()
		{
			lock (_syncRoot)
			{
				var itemsList = new List<T>();
				foreach (var queue in _internalQueues.Values)
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
			lock (_syncRoot)
			{
				item = default(T);
				if (result.IsFailure)
					return false;

				item = result.Value;
				return true;
			}			
		}

		public IEnumerator<T> GetEnumerator() => ToArray().GetEnumerator() as IEnumerator<T>;

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public Result Enqueue(T item)
		{
			lock (_syncRoot)
			{
				if (!_internalQueues.ContainsKey(item.Priority))
					return Result.Fail("Failed to enqueue item");

				_internalQueues[item.Priority].Enqueue(item);
				return Result.Ok();
			}			
		}

		public Result<T> Dequeue()
		{
			lock (_syncRoot)
			{
				return GetNextQueue()
					.OnSuccess(q => q.Dequeue())
					.OnFailure(err => Result.Fail(err));
			}
		}

		public Result<T> Peek()
		{
			lock (_syncRoot)
			{
				return GetNextQueue()
					.OnSuccess(q => q.Peek())
					.OnFailure(err => Result.Fail(err));
			}
		}

		private Result<Queue<T>> GetNextQueue()
		{
			foreach (var queue in _internalQueues.Values)
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
	}
}
