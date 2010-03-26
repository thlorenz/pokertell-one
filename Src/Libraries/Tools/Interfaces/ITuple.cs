namespace Tools.Interfaces
{
    using System;

    using FunctionalCSharp;

    public interface ITuple<T1, T2> : IEquatable<Tuple<T1, T2>>
    {
        /// <summary>
        /// Gets First.
        /// </summary>
        /// <value>The item1.</value>
        T1 First { get; }

        /// <summary>
        /// Gets Second.
        /// </summary>
        /// <value>The item2.</value>
        T2 Second { get; }
    }
}