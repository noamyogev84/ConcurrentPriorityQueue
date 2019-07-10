using CSharpFunctionalExtensions;

namespace ConcurrentPriorityQueue
{
	public interface IPriorityQueue<T>
	{
		Result Enqueue(T item);

		Result<T> Dequeue();

		Result<T> Peek();
	}
}
