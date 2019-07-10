using ConcurrentPriorityQueue;


namespace ConcurrentPriorityQueueTests
{	
	public class MockWithPriority : IHavePriority
	{
		public MockWithPriority(int priority)
		{
			Priority = priority;
		}

		public int Priority { get; set; }
	}	
}
