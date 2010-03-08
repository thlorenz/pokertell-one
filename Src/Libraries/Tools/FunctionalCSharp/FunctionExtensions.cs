namespace Tools.FunctionalCSharp
{
    using System;

    public static class FunctionExtensions
    {
        // F# - Currying
        public static Func<TArg1, Func<TArg2, TResult>> Curry<TArg1, TArg2, TResult>(this Func<TArg1, TArg2, TResult> func)
        {
            return a1 => a2 => func(a1, a2);
        }

        // F# - Currying
        public static Func<TArg1, Action<TArg2>> Curry<TArg1, TArg2>(this Action<TArg1, TArg2> action)
        {
            return a1 => a2 => action(a1, a2);
        }

        // F# - |>
        public static TResult Forward<TArg1, TArg2, TResult>(this TArg1 arg1, Func<TArg1, TArg2, TResult> func, TArg2 arg2)
        {
            return func(arg1, arg2);
        }

        // F# - |>
        public static void Forward<TArg1, TArg2>(this TArg1 arg1, Action<TArg1, TArg2> action, TArg2 arg2)
        {
            action(arg1, arg2);
        }

        // F# <|
        public static TResult Rev<TArg1, TArg2, TResult>(this TArg2 arg2, Func<TArg1, TArg2, TResult> func, TArg1 arg1)
        {
            return func(arg1, arg2);
        }

        // F# - <|
        public static void Rev<TArg1, TArg2>(this TArg2 arg2, Action<TArg1, TArg2> action, TArg1 arg1)
        {
            action(arg1, arg2);
        }
    }
}