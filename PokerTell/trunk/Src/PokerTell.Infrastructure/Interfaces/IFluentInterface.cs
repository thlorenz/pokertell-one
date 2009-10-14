namespace PokerTell.Infrastructure.Interfaces
{
    using System;
    using System.ComponentModel;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentInterface
    {
        /// <summary/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        /// <summary/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        /// <summary/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        /// <summary/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);
    }
}