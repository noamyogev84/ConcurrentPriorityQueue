using CSharpFunctionalExtensions;

namespace ConcurrentPriorityQueue.Core
{
	public interface IPriorityQueue<T>
	{
		Result Enqueue(T item);
        int Capacity { get; }

		Result<T> Dequeue();

		Result<T> Peek();
	}
}
