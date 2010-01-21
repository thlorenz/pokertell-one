namespace Tools.Interfaces
{
    using System;
    using System.ComponentModel;

    public interface IFluentInterface
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object other);

        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();
    }
}