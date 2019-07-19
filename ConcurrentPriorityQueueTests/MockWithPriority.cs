using ConcurrentPriorityQueue;


namespace ConcurrentPriorityQueueTests
{	
	public class MockWithPriority : IHavePriority
	{
		public MockWithPriority(int priority = 0)
		{
			Priority = priority;
		}

		public int Priority { get; set; }
	}	
}
