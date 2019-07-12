using System.Collections.Generic;
using ConcurrentPriorityQueue;
using System.Linq;

namespace ConcurrentPriorityQueueTests
{
	public static class TestHelpers
	{
		public static IEnumerable<object[]> MockItemsWithPriority =>
			new List<object[]>
			{
				new object[] { new MockWithPriority(0) },
				new object[] { new MockWithPriority(1) },
				new object[] { new MockWithPriority(2) },
			};

		public static List<IHavePriority> GetItemsWithPriority() =>
			MockItemsWithPriority.Select(objects => objects[0] as IHavePriority).ToList();
	}
}
