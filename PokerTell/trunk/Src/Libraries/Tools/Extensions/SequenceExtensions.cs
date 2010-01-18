namespace Tools.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class SequenceExtensions
    {
        public static IEnumerable<int> To(this int from, int to)
        {
            return from < to 
                ? Enumerable.Range(from, to - from + 1) 
                : Enumerable.Range(to, from - to + 1).Reverse();
        }

        public static IEnumerable<T> Step<T>(this IEnumerable<T> source, int step)
        {
            if (step == 0)
            {
                throw new ArgumentOutOfRangeException("step", "Param cannot be zero.");
            }
         
            return source.Where((_, indexOfItem) => (indexOfItem % step) == 0);
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            foreach (var item in items)
            {
                action(item);
            }

            return items;
        }

        public static void Times(this int times, Action action)
        {
            Enumerable.Range(1, times).ToList().ForEach(_ => action());
        }
    }
}