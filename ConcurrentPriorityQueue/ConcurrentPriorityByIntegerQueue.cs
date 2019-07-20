using ConcurrentPriorityQueue.Core;

namespace ConcurrentPriorityQueue
{
	public class ConcurrentPriorityByIntegerQueue<T> : ConcurrentPriorityQueue<T, int>
		where T : IHavePriority<int>
	{
		public ConcurrentPriorityByIntegerQueue() : base() { }

		public ConcurrentPriorityByIntegerQueue(int capacity) : base(capacity) { }
	}
}
