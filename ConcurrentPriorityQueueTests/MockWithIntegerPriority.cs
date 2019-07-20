using ConcurrentPriorityQueue;


namespace ConcurrentPriorityQueueTests
{	
	public class MockWithIntegerPriority : IHavePriority<int>
	{
		public MockWithIntegerPriority(int priority = 0)
		{
			Priority = priority;
		}

		public int Priority { get; set; }

		public int CompareTo(int other) => Priority.CompareTo(other);

		public bool Equals(int other) => Priority == other;
	}	
}
