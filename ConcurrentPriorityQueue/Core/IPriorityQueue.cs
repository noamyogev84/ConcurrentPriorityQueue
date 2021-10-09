using CSharpFunctionalExtensions;

namespace ConcurrentPriorityQueue.Core
{
    public interface IPriorityQueue<T>
    {
        Result Enqueue(T item);

        Result<T> Dequeue();

        Result<T> Peek();
    }
}
