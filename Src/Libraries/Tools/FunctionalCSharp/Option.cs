namespace Tools.FunctionalCSharp
{
    using System;
    using System.Collections.Generic;

    public sealed class Option<T> : IEquatable<Option<T>>
    {
        private readonly T value;
        private static readonly Option<T> none = new Option<T>();

        private Option() { }
        private Option(T value)
        {
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Option<T>)
                return Equals((Option<T>)obj);

            return false;
        }
        public bool Equals(Option<T> other)
        {
            if (IsNone)
                return other.IsNone;

            return EqualityComparer<T>.Default.Equals(value, other.value);
        }
        public override int GetHashCode()
        {
            if (IsNone)
                return 0;

            return EqualityComparer<T>.Default.GetHashCode(value);
        }

        public bool IsNone
        {
            get { return this == none; }
        }
        public bool IsSome
        {
            get { return !IsNone; }
        }

        public T Value
        {
            get
            {
                if (IsSome)
                    return value;

                throw new InvalidOperationException();
            }
        }

        public static Option<T> None
        {
            get { return none; }
        }
        public static Option<T> Some(T value)
        {
            return new Option<T>(value);
        }
    }

    public static class Option
    {
        public static Option<T> Some<T>(T value)
        {
            return Option<T>.Some(value);
        }
    }

}