using System.Collections.Generic;
using System.Linq;
using ConcurrentPriorityQueue.Core;

namespace ConcurrentPriorityQueueTests
{
    public static class TestHelpers
    {
        public static IEnumerable<object[]> MockItemsWithIntegerPriority =>
            new List<object[]>
            {
                new object[] { new MockWithIntegerPriority(0) },
                new object[] { new MockWithIntegerPriority(1) },
                new object[] { new MockWithIntegerPriority(2) },
            };

        public static List<IHavePriority<int>> GetItemsWithIntegerPriority() =>
            MockItemsWithIntegerPriority
                .Select(objects => objects[0] as IHavePriority<int>).ToList();
    }
}
