namespace Tools.FunctionalCSharp
{
    using System;
    using System.Collections.Generic;

    public static class MemoizationExtensions
    {
        // Memoize Function
        public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func)
        {
            var t = new Dictionary<T, TResult>();
            return n =>
            {
                if (t.ContainsKey(n)) return t[n];
                
                var result = func(n);
                t.Add(n, result);
                Console.WriteLine("{0} - {1}", n, result);
                return result;
            };
        }
    }
}