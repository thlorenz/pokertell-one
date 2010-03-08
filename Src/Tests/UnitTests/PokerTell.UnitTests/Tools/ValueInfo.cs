namespace Moq
{
    using System;

    internal class ValueInfo
    {
        #region Constructors and Destructors

        internal ValueInfo(Type type, object value)
        {
            Type = type;
            Value = value;
        }

        #endregion

        #region Properties

        public Type Type { get; private set; }

        public object Value { get; private set; }

        #endregion
    }
}