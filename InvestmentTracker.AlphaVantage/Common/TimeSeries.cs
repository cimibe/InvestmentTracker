using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace InvestmentTracker.AlphaVantage.Common
{
    internal class DescendingOrder : IComparer<DateTime>
    {
        public static DescendingOrder Instance = new DescendingOrder();

        public int Compare(DateTime x, DateTime y)
        {
            return y.CompareTo(x);
        }
    }

    public class TimeSeries<T> : SortedList<DateTime, T>
    {
        public TimeSeries(int capacity) : base(capacity, DescendingOrder.Instance) { }

        public TimeSeries() : this(100) { }
    }
}
