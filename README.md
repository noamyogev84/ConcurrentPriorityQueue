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

  Items in the collection **must** implement the generic interface `IHavePriority<T>` where T: implements `IEquatable<T>`, `IComparable<T>` and also overrides `Object.GetHashCode()`:

```csharp
// Simplest implementation of IHavePriority<T>
public class SomeClass : IHavePriority<int> {
    int Priority {get; set;}
}
```

Simple flow for creating a `Priority-By-Integer` queue and adding an item:
```csharp
// Create a new prioritized item.
var itemWithPriority = new SomeClass { Priority = 0 };

// Initialize an unbounded priority-by-integer queue.
var priorityQueue = new ConcurrentPriorityQueue<IHavePriority<int>, int>();

// Enqueue item and handle result.
Result result = priorityQueue.Enqueue(itemWithPriority);
```

Use the `ConcurrentPriorityByIntegerQueue` implementation to simplify the above example:

```csharp
var priorityQueue = new ConcurrentPriorityByIntegerQueue<IHavePriority<int>>();
```

**Consume** items by priority/first-in-first-out policy, using `Dequeue()` and `Peek()`:

```csharp
// Lower value -> Higher priority.
var item1 = new SomeClass { Priority = 1 };
var item2 = new SomeClass { Priority = 0 };
var item3 = new SomeClass { Priority = 0 };

priorityQueue.Enqueue(item1);
priorityQueue.Enqueue(item2);
priorityQueue.Enqueue(item3);

var result = priority.Dequeue(); // item2
var result = priority.Dequeue(); // item3
var result = priority.Dequeue(); // item1
```

**Iteration** over the collection will yield items according to their priority and position (FIFO):

```csharp
var item1 = new SomeClass { Priority = 1 };
var item2 = new SomeClass { Priority = 0 };
var item3 = new SomeClass { Priority = 0 };

priorityQueue.Enqueue(item1);
priorityQueue.Enqueue(item2);
priorityQueue.Enqueue(item3);

foreach(var item in priorityQueue) {
    // Iteration 1 -> item2
    // Iteration 2 -> item3
    // Iteration 3 -> item1
}
```

`ConcurrentPriorityQueue` is **Generic**. Implement your own **Business Priority** object and configure the queue to handle it:

```csharp
// TimeToProcess class implements IEquatable<T>, IComparable<T> and overrides Object.GetHashCode().
public class TimeToProcess : IEquatable<TimeToProcess>, IComparable<TimeToProcess> {		
    public decimal TimeInMilliseconds { get; set;}

    public int CompareTo(TimeToProcess other) =>
        TimeInMilliseconds.CompareTo(other.TimeInMilliseconds);

    public bool Equals(TimeToProcess other) =>
        TimeInMilliseconds.Equals(other.TimeInMilliseconds);

    public override int GetHashCode() => TimeInMilliseconds.GetHashCode();
}
```

```csharp
// BusinessPriorityItem implements IHavePriority<T>
public class BusinessPriorityItem : IHavePriority<TimeToProcess> {
    TimeToProcess Priority {get; set;}
}
```

```csharp
// Create a new prioritized item.
var item = new BusinessPriorityItem { Priority = new TimeToProcess { TimeInMilliseconds = 0.25M } };

// Initialize an unbounded priority-by-TimeToProcess queue.
var priorityQueue = new ConcurrentPriorityQueue<IHavePriority<TimeToProcess>, TimeToProcess>();

// Enqueue item and handle result.
Result result = priorityQueue.Enqueue(item);
```

`ConcurrentPriorityQueue<T>` can be bounded to a fixed amount of priorities:

```csharp
// Create a bounded ConcurrentPriorityQueue to support a fixed amount of priorities.
var maxAmountOfPriorities = 2;
var priorityQueue = new ConcurrentPriorityByIntegerQueue<IHavePriority<int>>(maxAmountOfPriorities);

Result result = PriorityQueue.Enqueue(new SomeClass {Priority = 0}); // result.OK
Result result = PriorityQueue.Enqueue(new SomeClass {Priority = 1}); // result.OK
Result result = PriorityQueue.Enqueue(new SomeClass {Priority = 2}); // result.Fail -> Queue supports [0, 1]
Result result = PriorityQueue.Enqueue(new SomeClass {Priority = 0}); // result.OK
```

`ConcurrentPriorityQueue<T>` can be **extended** to a `BlockingCollection<T>` using the `ToBlockingCollection<T>` extension method.

```csharp
var blockingPriorityQueue = new ConcurrentPriorityByIntegerQueue<IHavePriority<int>>()
                                .ToBlockingCollection();

foreach(var item in blockingPriorityQueue.GetConsumingEnumerable()) {
    // Do something...
    // Blocks until signaled of completion.
}


