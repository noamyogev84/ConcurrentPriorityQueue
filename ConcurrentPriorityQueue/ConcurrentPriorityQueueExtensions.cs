using System.Collections.Concurrent;

namespace ConcurrentPriorityQueue
{
	public static class ConcurrentPriorityQueueExtensions
	{
		public static BlockingCollection<T> ToBlockingCollection<T>(this IConcurrentPriorityQueue<T> queue)
			where T : IHavePriority => new BlockingCollection<T>(queue);
	}
}
