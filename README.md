[![Build Status](https://dev.azure.com/noamyogev84/noamyogev84/_apis/build/status/noamyogev84.ConcurrentPriorityQueue?branchName=develop)](https://dev.azure.com/noamyogev84/noamyogev84/_build/latest?definitionId=1&branchName=develop)

# ConcurrentPriorityQueue
A thread-safe generic first in first out (FIFO) collection with support for priority queuing.

Nuget: [enter link](https:\\)

### Features

1. Thread-Safe.
2. Manages items  according to a `First in first out` policy and `priority` on top of that.
3. Implements `IProducerConsumerCollection<T>` interface.
4. Extends to a `BlockingCollection<T>`.

### Examples

```csharp
\\ Implement IHavePriority<T>
public class SomeClass : IHavePriority<int> {
    int Priority {get; set;}
}
```

```csharp
\\ Create a new prioritized item.
var itemWithPriority = new SomeClass { Priority = 0 };

\\ Initialize an unbounded priority-by-integer queue.
var priorityQueue = new ConcurrentPriorityQueue<IHavePriority<int>, int>();

\\ Enqueue item and handle result.
Result result = priorityQueue.Enqueue(itemWithPriority);
```

