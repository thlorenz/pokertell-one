namespace Tools.FunctionalCSharp
{
    using System;

    using Tools.Interfaces;

    public class Tuple<T1, T2> : ITuple<T1, T2>
    {
        readonly T1 _first;

        /// <summary>
        /// Gets First.
        /// </summary>
        /// <value>The item1.</value>
        public T1 First
        {
            get { return _first; }
        }

        readonly T2 _second;

        /// <summary>
        /// Gets Second.
        /// </summary>
        /// <value>The item2.</value>
        public T2 Second
        {
            get { return _second; }
        }

        /// <summary>
        /// Initializes a new instance of the Tuple class.
        /// </summary>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        public Tuple(T1 first, T2 second)
        {
            _first = first;
            _second = second;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current Tuple.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current Tuple.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current Tuple; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                throw new NullReferenceException("obj is null");
            if (ReferenceEquals(this, obj)) return true;
            if (!(obj is Tuple<T1, T2>)) return false;
            return Equals((Tuple<T1, T2>)obj);
        }


        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current Tuple.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current pair/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("({0},{1})", First, Second);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="obj" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="obj">
        /// An object to compare with this object.
        /// </param>
        public bool Equals(Tuple<T1, T2> obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj.First, First) && Equals(obj.Second, Second);
        }
        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return (_first.GetHashCode() * 397) ^ _second.GetHashCode();
            }
        }

       

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Tuple<T1, T2> left, Tuple<T1, T2> right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Tuple<T1, T2> left, Tuple<T1, T2> right)
        {
            return !Equals(left, right);
        }
    }

    public static class Tuple
    {
        public static Tuple<T1, T2> New<T1, T2>(T1 fst, T2 snd)
        {
            return new Tuple<T1, T2>(fst, snd);
        }
    }
}