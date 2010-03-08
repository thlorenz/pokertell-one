namespace PokerTell.DatabaseSetup
{
    using System;

    public class DatabaseExistsException : Exception
    {
        #region Constructors and Destructors

        public DatabaseExistsException(string message)
            : base(message)
        {
        }

        #endregion
    }

    public class DatabaseDoesNotExistException : Exception
    {
        #region Constructors and Destructors

        public DatabaseDoesNotExistException(string message)
            : base(message)
        {
        }

        #endregion
    }

    public class DatabaseProviderNotSupportedException : Exception
    {
        #region Constructors and Destructors

        public DatabaseProviderNotSupportedException(string message)
            : base(message)
        {
        }

        #endregion
    }

    public class DatabaseUnknownProviderNiceName : Exception
    {
        #region Constructors and Destructors

        public DatabaseUnknownProviderNiceName(string message)
            : base(message)
        {
        }

        #endregion
    }
}