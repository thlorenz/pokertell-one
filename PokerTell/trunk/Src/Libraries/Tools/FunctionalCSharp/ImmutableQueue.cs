namespace Tools.FunctionalCSharp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public interface IImmutableQueue<T> : IEnumerable<T>
    {
        bool IsEmpty { get; }
        T Peek();
        IImmutableQueue<T> Enqueue(T value);
        IImmutableQueue<T> Dequeue();
    }

    public sealed class ImmutableQueue<T> : IImmutableQueue<T>
    {
        private sealed class EmptyQueue : IImmutableQueue<T>
        {
            public bool IsEmpty { get { return true; } }
            public T Peek() { throw new Exception("empty queue"); }
            public IImmutableQueue<T> Enqueue(T value)
            {
                return new ImmutableQueue<T>(ImmutableStack<T>.Empty.Push(value), ImmutableStack<T>.Empty);
            }
            public IImmutableQueue<T> Dequeue() { throw new Exception("empty queue"); }
            public IEnumerator<T> GetEnumerator() { yield break; }
            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        }
        private static readonly IImmutableQueue<T> empty = new EmptyQueue();
        public static IImmutableQueue<T> Empty { get { return empty; } }
        public bool IsEmpty { get { return false; } }
        private readonly IImmutableStack<T> backwards;
        private readonly IImmutableStack<T> forwards;
        private ImmutableQueue(IImmutableStack<T> f, IImmutableStack<T> b)
        {
            forwards = f;
            backwards = b;
        }
        public T Peek() { return forwards.Peek(); }
        public IImmutableQueue<T> Enqueue(T value)
        {
            return new ImmutableQueue<T>(forwards, backwards.Push(value));
        }
        public IImmutableQueue<T> Dequeue()
        {
            IImmutableStack<T> f = forwards.Pop();
            if (!f.IsEmpty)
                return new ImmutableQueue<T>(f, backwards);
            if (backwards.IsEmpty)
                return Empty;
            return new ImmutableQueue<T>((IImmutableStack<T>)backwards.Reverse(), ImmutableStack<T>.Empty);
        }
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var t in forwards) yield return t;
            foreach (var t in backwards.Reverse()) yield return t;
        }
        IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }
    }
}


