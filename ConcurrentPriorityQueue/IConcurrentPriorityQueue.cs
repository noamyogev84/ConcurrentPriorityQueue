using System.Collections.Concurrent;

namespace ConcurrentPriorityQueue
{
	public interface IConcurrentPriorityQueue<T> : IProducerConsumerCollection<T>, IPriorityQueue<T> where T : IHavePriority
	{
		
	}
}
