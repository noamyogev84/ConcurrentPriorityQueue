[![Build Status](https://dev.azure.com/noamyogev84/noamyogev84/_apis/build/status/noamyogev84.ConcurrentPriorityQueue?branchName=develop)](https://dev.azure.com/noamyogev84/noamyogev84/_build/latest?definitionId=1&branchName=develop)

# ConcurrentPriorityQueue
A thread-safe generic first in first out (FIFO) collection with support for priority queuing.

Nuget: [enter link](https:\\)

### Features

1. Thread-Safe.
2. Manages items  according to a `First in first out` policy and `priority` on top of that.
3. Implements `IProducerConsumerCollection<T>` interface.
4. Extends to a `BlockingCollection<T>`.

### Examples:

#### Items in the collection must implement the generic interface `IHavePriority<T>` where T: implements `IEquatable<T>`, `IComparable<T>` and also overrides `Object.GetHashCode()`:

```csharp
// Simplest implementation of IHavePriority<T>
public class SomeClass : IHavePriority<int> {
    int Priority {get; set;}
}
```

#### Simple flow of creating a `Priority-By-Integer` queue and adding an item:
```csharp
// Create a new prioritized item.
var itemWithPriority = new SomeClass { Priority = 0 };

// Initialize an unbounded priority-by-integer queue.
var priorityQueue = new ConcurrentPriorityQueue<IHavePriority<int>, int>();

// Enqueue item and handle result.
Result result = priorityQueue.Enqueue(itemWithPriority);
```

#### Use the `ConcurrentPriorityByIntegerQueue` implementation to simplify the above example:

```csharp
var priorityQueue = new ConcurrentPriorityByIntegerQueue<IHavePriority<int>>();
```

#### Consume items by priority plus first-in-first-out policy, using `Dequeue()` and `Peek()`:

```csharp
// Lower value -> Higher priority.
var item1 = new SomeClass { Priority = 1 };
var item2 = new SomeClass { Priority = 0 };
var item3 = new SomeClass { Priority = 0 };

priorityQueue.Enqueue(item1);
priorityQueue.Enqueue(item1);
priorityQueue.Enqueue(item1);

var result = priority.Dequeue(); // item2
var result = priority.Dequeue(); // item3
var result = priority.Dequeue(); // item1
```

#### Iteration over the collection will yield items according to their priority and position (FIFO):

```csharp
var item1 = new SomeClass { Priority = 1 };
var item2 = new SomeClass { Priority = 0 };
var item3 = new SomeClass { Priority = 0 };

priorityQueue.Enqueue(item1);
priorityQueue.Enqueue(item1);
priorityQueue.Enqueue(item1);

foreach(var item in priorityQueue) {
    // Iteration 1 -> item2
    // Iteration 2 -> item3
    // Iteration 3 -> item1
}
```


