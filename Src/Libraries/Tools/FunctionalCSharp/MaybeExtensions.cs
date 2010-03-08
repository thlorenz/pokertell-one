namespace Tools.FunctionalCSharp
{
    using System;

    public static class MaybeExtensions
    {
        public static Maybe<T> ToMaybe<T>(this T value)
        {
            return new Maybe<T>(value);
        }

        public static Maybe<U> SelectMany<T, U>(this Maybe<T> m, Func<T, Maybe<U>> k)
        {
            return !m.HasValue ? Maybe<U>.Nothing : k(m.Value);
        }

        public static Maybe<V> SelectMany<T, U, V>(this Maybe<T> m, Func<T, Maybe<U>> k, Func<T, U, V> s)
        {
            return !m.HasValue ? Maybe<V>.Nothing : s(m.Value, k(m.Value).Value).ToMaybe();
        }
    }


    public class Maybe<T>
    {
        public readonly static Maybe<T> Nothing = new Maybe<T>();
        public T Value { get; private set; }
        public bool HasValue { get; private set; }

        Maybe()
        {
            HasValue = false;
        }

        public Maybe(T value)
        {
            Value = value;
            HasValue = true;
        }
    }
}