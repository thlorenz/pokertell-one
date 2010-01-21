namespace Tools.FunctionalCSharp
{
    using System;
    using System.Collections.Generic;

    public class PatternMatchContext<T>
    {
        private readonly T value;

        internal PatternMatchContext(T value)
        {
            this.value = value;
        }

        public PatternMatch<T, TResult> With<TResult>(
            Predicate<T> condition,
            Func<T, TResult> result)
        {
            var match = new PatternMatch<T, TResult>(value);
            return match.With(condition, result);
        }
    }

    public static class PatternMatchExtensions
    {
        public static PatternMatchContext<T> Match<T>(this T value)
        {
            return new PatternMatchContext<T>(value);
        }
    }

    public class MatchNotFoundException : Exception
    {
        public MatchNotFoundException(string message) : base(message) { }
    }

    public class PatternMatch<T, TResult>
    {
        private readonly T value;
        private readonly List<Tuple<Predicate<T>, Func<T, TResult>>> cases
            = new List<Tuple<Predicate<T>, Func<T, TResult>>>();
        private Func<T, TResult> elseFunc;

        internal PatternMatch(T value)
        {
            this.value = value;
        }

        public PatternMatch<T, TResult> With(Predicate<T> pred, Func<T, TResult> action)
        {
            cases.Add(Tuple.New(pred, action));
            return this;
        }

        public PatternMatch<T, TResult> Else(Func<T, TResult> action)
        {
            if (elseFunc != null)
                throw new InvalidOperationException("Cannot have multiple else cases");

            elseFunc = action;
            return this;
        }

        public TResult Do()
        {
            if (elseFunc != null)
                cases.Add(
                    Tuple.New<Predicate<T>, Func<T, TResult>>(x => true, elseFunc));
            foreach (var item in cases)
                if (item.First(value))
                    return item.Second(value);

            throw new MatchNotFoundException("Incomplete pattern match");
        }
    }
}