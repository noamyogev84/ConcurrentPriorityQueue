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
				new object[] { new MockWithIntegerPriority(0) },
				new object[] { new MockWithIntegerPriority(1) },
				new object[] { new MockWithIntegerPriority(2) },
			};

		public static List<IHavePriority<int>> GetItemsWithIntegerPriority() =>
			MockItemsWithPriority.Select(objects => objects[0] as IHavePriority<int>).ToList();
	}
}
