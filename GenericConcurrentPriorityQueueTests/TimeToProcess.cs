using System;

namespace GenericConcurrentPriorityQueueTests
{
    public class TimeToProcess : IEquatable<TimeToProcess>, IComparable<TimeToProcess>
    {
        public TimeToProcess(decimal timeToProcessInMilliseconds = 0)
        {
            TimeInMilliseconds = timeToProcessInMilliseconds;
        }

        public decimal TimeInMilliseconds { get; }

        public int CompareTo(TimeToProcess other) =>
            TimeInMilliseconds.CompareTo(other.TimeInMilliseconds);

        public bool Equals(TimeToProcess other) =>
            TimeInMilliseconds.Equals(other.TimeInMilliseconds);

        public override int GetHashCode() => TimeInMilliseconds.GetHashCode();

        public override bool Equals(object obj) => obj is TimeToProcess process && Equals(process);
    }
}
