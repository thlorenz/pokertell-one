namespace Tools.FunctionalCSharp
{
    using System;
    using System.Collections.Generic;

    public class Table<T, U> : IDisposable
    {
        private readonly Dictionary<T, U> dictionary;
        private readonly Func<T, U> func;

        public Table(Dictionary<T, U> dictionary, Func<T, U> func)
        {
            this.dictionary = dictionary;
            this.func = func;
        }

        public U this[T n]
        {
            get
            {
                if (dictionary.ContainsKey(n))
                    return dictionary[n];

                var result = func(n);
                dictionary.Add(n, result);
                return result;
            }
        }

        public void Dispose()
        {
            dictionary.Clear();
        }
    }
}