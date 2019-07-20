using System;

namespace ConcurrentPriorityQueue
{
	public interface IHavePriority<TP> where TP : IEquatable<TP>, IComparable<TP>
	{
		TP Priority { get; set; }
	}
}
