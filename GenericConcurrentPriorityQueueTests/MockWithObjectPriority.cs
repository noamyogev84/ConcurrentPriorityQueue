using ConcurrentPriorityQueue.Core;

namespace GenericConcurrentPriorityQueueTests
{
	public class MockWithObjectPriority : IHavePriority<TimeToProcess>
	{
		public MockWithObjectPriority(TimeToProcess timeToProcess)
		{
			Priority = timeToProcess;
		}

		public TimeToProcess Priority { get; set; }
	}	
}
