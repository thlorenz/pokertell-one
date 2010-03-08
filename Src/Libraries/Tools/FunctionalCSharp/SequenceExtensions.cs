namespace Tools.FunctionalCSharp
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
            if (items == null) throw new ArgumentNullException("items");
            if (action == null) throw new ArgumentNullException("action");

            foreach (var item in items)
            {
                action(item);
            }

            return items;
        }

        public static void ForIndex<T>(this IEnumerable<T> items, Action<int, T> action)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (action == null) throw new ArgumentNullException("action");

            var index = 0;
            foreach (var item in items)
            {
                action(index, item);
                index++;
            }
        }

        public static void Times(this int times, Action action)
        {
            Enumerable.Range(1, times).ToList().ForEach(_ => action());
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> items, Predicate<T> predicate)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");

            return items.Where(item => predicate(item));
        }

        // Seq.fold
        public static T Fold<T, U>(this IEnumerable<U> items, Func<T, U, T> func, T acc)
        {
            return items.Aggregate(acc, func);
        }

        // F# List.fold_left
        public static T FoldLeft<T, U>(this IEnumerable<U> list, Func<T, U, T> func, T acc)
        {
            return list.Aggregate(acc, func);
        }

        // F# List.fold_right
        public static T FoldRight<T, U>(this IList<U> list, Func<T, U, T> func, T acc)
        {
            for (var index = list.Count - 1; index >= 0; index--)
                acc = func(acc, list[index]);

            return acc;
        }

        public static IEnumerable<Tuple<TArg1, TArg2>> Zip<TArg1, TArg2>(this IEnumerable<TArg1> arg1, IEnumerable<TArg2> arg2, Func<TArg1, TArg2, Tuple<TArg1, TArg2>> func)
        {
            return arg1.Map2(arg2, func);
        }

        public static IEnumerable<TResult> Map<T, TResult>(this IEnumerable<T> items, Func<T, TResult> func)
        {
            foreach (var item in items)
                yield return func(item);
        }

        public static IEnumerable<TResult> MapIndex<T, TResult>(this IEnumerable<T> items, Func<int, T, TResult> func)
        {
            var index = 0;
            foreach (var item in items)
            {
                yield return func(index, item);
                index++;
            }
        }

        public static IEnumerable<TResult> Map2<TArg1, TArg2, TResult>(this IEnumerable<TArg1> arg1, IEnumerable<TArg2> arg2, Func<TArg1, TArg2, TResult> func)
        {
            var e1 = arg1.GetEnumerator();
            var e2 = arg2.GetEnumerator();
            var s = new SequenceMapEnumerator<TArg1, TArg2, TResult>(e1, e2, func);

            while (s.MoveNext())
                yield return s.Current;
        }

        // F# Seq.unfold
        public static IEnumerable<TResult> Unfold<T, TResult>(Func<T, Option<Tuple<TResult, T>>> generator, T start)
        {
            var next = start;

            while (true)
            {
                var res = generator(next);
                if (res.IsNone)
                    yield break;

                yield return res.Value.First;

                next = res.Value.Second;
            }
        }

        // Seq.init_infinite
        public static IEnumerable<T> InitializeInfinite<T>(Func<int, T> f)
        {
            return Unfold(s => Option.Some(Tuple.New(f(s), s + 1)), 0);
        }

        // Seq.init_finite
        public static IEnumerable<T> InitializeFinite<T>(int count, Func<int, T> f)
        {
            return Unfold(s => s < count ? Option.Some(Tuple.New(f(s), s + 1)) : Option<Tuple<T, int>>.None, 0);
        }

        // F# Seq.generate
        public static IEnumerable<TResult> Generate<T, TResult>(Func<T> opener, Func<T, Option<TResult>> generator, Action<T> closer)
        {
            var openerResult = opener();

            while (true)
            {
                var res = generator(openerResult);
                if (res.IsNone)
                {
                    closer(openerResult);
                    yield break;
                }

                yield return res.Value;
            }
        }

        public static IEnumerable<TResult> GenerateUsing<T, TResult>(Func<T> opener, Func<T, Option<TResult>> generator) where T : IDisposable
        {
            return Generate(opener, generator, x => x.Dispose());
        }

    }
}