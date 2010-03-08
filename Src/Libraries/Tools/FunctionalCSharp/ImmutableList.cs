namespace Tools.FunctionalCSharp
{
    using System.Collections;
    using System.Collections.Generic;

    public interface IImmutableList<T> : IEnumerable<T>
    {
        #region Properties

        T Head { get; }

        bool IsCons { get; }

        bool IsEmpty { get; }

        IImmutableList<T> Tail { get; }

        #endregion
    }

    public class ImmutableList<T> : IImmutableList<T>
    {
        #region Constants and Fields

        static readonly EmptyList empty = new EmptyList();

        IEnumerator<T> _enumerator;

        IImmutableList<T> _tail;

        #endregion

        #region Properties

        public static IImmutableList<T> Empty
        {
            get { return empty; }
        }

        public T Head { get; private set; }

        public bool IsCons
        {
            get { return this is ConsList; }
        }

        public bool IsEmpty
        {
            get { return this is EmptyList; }
        }

        public IImmutableList<T> Tail
        {
            get
            {
                if (_enumerator != null)
                {
                    _tail = Create(_enumerator);
                    _enumerator = null;
                }
                return _tail;
            }
        }

        #endregion

        #region Public Methods

        public static IImmutableList<T> Create(IEnumerator<T> enumerator)
        {
            return enumerator.MoveNext() ? new ConsList(enumerator.Current, enumerator) : Empty;
        }

        #endregion

        #region Implemented Interfaces

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable<T>

        public IEnumerator<T> GetEnumerator()
        {
            for (IImmutableList<T> current = this; !current.IsEmpty; current = current.Tail)
            {
                yield return current.Head;
            }
        }

        #endregion

        #endregion

        internal class ConsList : ImmutableList<T>
        {
            #region Constructors and Destructors

            public ConsList(T head, IEnumerator<T> enumerator)
            {
                Head = head;
                this._enumerator = enumerator;
            }

            #endregion
        }

        internal class EmptyList : ImmutableList<T>
        {
        }
    }
}