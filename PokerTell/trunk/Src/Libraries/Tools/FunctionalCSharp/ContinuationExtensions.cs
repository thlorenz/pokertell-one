namespace Tools.FunctionalCSharp
{
    using System;


    public delegate TAnswer K<T, TAnswer>(Func<T, TAnswer> k);

    public static class ContinuationExtensions
    {
        public static K<V, TAnswer> SelectMany<T, U, V, TAnswer>(this K<T, TAnswer> m, Func<T, K<U, TAnswer>> k, Func<T, U, V> s)
        {
            return m.SelectMany(x => k(x).SelectMany(y => s(x, y).ToContinuation<V, TAnswer>()));
        }

        public static K<U, TAnswer> SelectMany<T, U, TAnswer>(this K<T, TAnswer> m, Func<T, K<U, TAnswer>> k)
        {
            return c => m(x => k(x)(c));
        }

        public static K<T, TAnswer> ToContinuation<T, TAnswer>(this T value)
        {
            return c => c(value);
        }
    }
}