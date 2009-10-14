namespace Moq
{
    using System.Collections.Generic;

    using PokerTell.UnitTests;

    public class ValueSetup
    {
        #region Constants and Fields

        readonly StubBuilder _builder;

        readonly For _keyToDefine;

        readonly IDictionary<For, ValueInfo> _stubValues;

        #endregion

        #region Constructors and Destructors

        internal ValueSetup(StubBuilder builder, IDictionary<For, ValueInfo> stubValues, For keyToDefine)
        {
            _builder = builder;
            _stubValues = stubValues;
            _keyToDefine = keyToDefine;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Defines the value to be used for the current selected key
        /// </summary>
        /// <typeparam name="T">type of the value</typeparam>
        /// <param name="value">the value</param>
        /// <returns>The StubBuilder in use for further definitions or to access the stub</returns>
        public StubBuilder Is<T>(T value)
        {
            if (_stubValues.ContainsKey(_keyToDefine))
            {
                _stubValues.Remove(_keyToDefine);
            }

            _stubValues.Add(_keyToDefine, new ValueInfo(typeof(T), value));

            return _builder;
        }

        #endregion
    }
}