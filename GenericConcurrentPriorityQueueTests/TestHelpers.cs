using System.Collections.Generic;
using System.Linq;
using ConcurrentPriorityQueue.Core;

namespace GenericConcurrentPriorityQueueTests
{
    public static class TestHelpers
    {
        public static IEnumerable<object[]> MockItemsWithObjectPriority =>
            new List<object[]>
            {
                new object[] { new MockWithObjectPriority(new TimeToProcess(0.25M)) },
                new object[] { new MockWithObjectPriority(new TimeToProcess(0.5M)) },
                new object[] { new MockWithObjectPriority(new TimeToProcess(1M)) },
            };

        public static List<IHavePriority<TimeToProcess>> GetItemsWithObjectPriority() =>
            MockItemsWithObjectPriority
                .Select(objects => objects[0] as IHavePriority<TimeToProcess>).ToList();
    }
}
