namespace Tools.FunctionalCSharp
{
    using System;
    using System.Collections.Generic;

    public class SequenceMapEnumerator<T, U, V> : IEnumerator<V>
    {
        private readonly IEnumerator<T> e1;
        private readonly IEnumerator<U> e2;
        private readonly Func<T, U, V> f;

        public SequenceMapEnumerator(IEnumerator<T> e1, IEnumerator<U> e2, Func<T, U, V> f)
        {
            this.e1 = e1;
            this.e2 = e2;
            this.f = f;
        }

        public V Current
        {
            get { return f(e1.Current, e2.Current); }
        }

        public void Dispose()
        {
            e1.Dispose();
            e2.Dispose();
        }

        object System.Collections.IEnumerator.Current
        {
            get { return f(e1.Current, e2.Current); }
        }

        public bool MoveNext()
        {
            var n1 = e1.MoveNext();
            var n2 = e2.MoveNext();
            if (n1 != n2)
                throw new InvalidOperationException("Inequal list lengths");
            return n1;
        }

        public void Reset()
        {
            e1.Reset();
            e2.Reset();
        }
    }
}