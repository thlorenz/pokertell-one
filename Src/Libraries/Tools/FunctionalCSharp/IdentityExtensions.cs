namespace Tools.FunctionalCSharp
{
    using System;

    public static class IdentityExtensions
    {
        #region Public Methods

        public static Identity<U> SelectMany<T, U>(this Identity<T> id, Func<T, Identity<U>> k)
        {
            return k(id.Value);
        }

        public static Identity<V> SelectMany<T, U, V>(this Identity<T> id, Func<T, Identity<U>> k, Func<T, U, V> s)
        {
            return s(id.Value, k(id.Value).Value).ToIdentity();
        }

        public static Identity<T> ToIdentity<T>(this T value)
        {
            return new Identity<T>(value);
        }

        #endregion
    }

    public class IdentityMonad
    {
        #region Public Methods

        public static Identity<U> Bind<T, U>(Identity<T> id, Func<T, Identity<U>> k)
        {
            return k(id.Value);
        }

        public static Identity<T> Unit<T>(T value)
        {
            return new Identity<T>(value);
        }

        #endregion
    }

    public class Identity<T>
    {
        #region Constructors and Destructors

        public Identity(T value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public T Value { get; private set; }

        #endregion
    }
}