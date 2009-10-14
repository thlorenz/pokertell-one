namespace Moq
{
    using System;
    using System.Collections.Generic;

    using PokerTell.UnitTests;

    /// <summary>
    /// Allows to build stub objects
    /// </summary>
    public class StubBuilder
    {
        #region Constants and Fields

        readonly IDictionary<For, ValueInfo> _stubValues;

        #endregion

        #region Constructors and Destructors

        public StubBuilder()
        {
            _stubValues = new Dictionary<For, ValueInfo>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Stubs out a parameter with an integer
        /// </summary>
        /// <param name="parameter">Specifies parameter name</param>
        /// <returns>The value previously defined for the parameter via Value().Is()
        /// If no value has been defined for the parameter it returns default(T)
        /// </returns>
        public T Out<T>(For parameter)
        {
            if (_stubValues.ContainsKey(parameter))
            {
                if (_stubValues[parameter].Type != typeof(T))
                {
                    throw new ArgumentException(
                        string.Format(
                            "Target type {1} for {0} does not match stubbed type {2}.", 
                            parameter, 
                            typeof(T), 
                            _stubValues[parameter].Type));
                }

                return (T)_stubValues[parameter].Value;
            }

            return default(T);
        }

        /// <summary>
        /// Returns a stub for the desired type. 
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[TestMethodWith(Stub.Out<IDependency>());]]>
        /// </code>
        /// </example>
        /// <typeparam name="TInterface">Type to stub out</typeparam>
        /// <returns>The Object of the Mock of the TInterface </returns>
        public TInterface Out<TInterface>()
            where TInterface : class
        {
            return new Mock<TInterface>().Object;
        }

        /// <summary>
        /// Used to define retrun values for some getters for the stub before returning it via Out
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[TestMethodWith(Stub.Setup<IDependency>().Get(dependency => dependency.Property, someValue));]]>
        /// </code>
        /// </example>
        /// <typeparam name="TInterface">Type to stub out</typeparam>
        /// <returns>StubWith object that allows to define the getters </returns>
        public StubWith<TInterface> Setup<TInterface>()
            where TInterface : class
        {
            return new StubWith<TInterface>();
        }

        /// <summary>
        /// Specifies an arbitrary value for type T
        /// Use Out when default(T) suffices
        /// </summary>
        /// <typeparam name="T">type of the value</typeparam>
        /// <param name="value">The value to use as a stub</param>
        /// <returns><see cref="value"/></returns>
        public T Some<T>(T value)
            where T : struct
        {
            return value;
        }

        /// <summary>
        /// Specifies an arbitrary value for type T
        /// Use Some() when default(T) suffices
        /// </summary>
        /// <typeparam name="T">type of the value</typeparam>
        /// <returns>default(T)</returns>
        public T Some<T>()
            where T : struct
        {
            return default(T);
        }

        /// <summary>
        /// Used to specify a valid value for a parameter to be used as a stub
        /// </summary>
        /// <typeparam name="T">type of the value</typeparam>
        /// <param name="parameter">name of the parameter</param>
        /// <param name="value">valid value to be used</param>
        /// <returns><see cref="value"/></returns>
        public T Valid<T>(For parameter, T value)
        {
            return value;
        }

        /// <summary>
        /// Used to define values to be used via the Out(key) method
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[_stub.Value(For.Cards).Is("My Cards");
        /// // Use "My Cards" as parameter in the following call of Mehod
        /// Method(_stub.Out<string>(For.HoleCards));]]>
        /// </code>
        /// </example>
        /// <param name="key">Name of the parameter to be defined</param>
        /// <returns>A ValueSetup to define the value to be used for the key</returns>
        public ValueSetup Value(For key)
        {
            return new ValueSetup(this, _stubValues, key);
        }

        public T Get<T>(For parameter)
        {
            if (_stubValues.ContainsKey(parameter))
            {
                return Out<T>(parameter);
            }

            throw new KeyNotFoundException(string.Format("A value for {0} has not been stubbed.", parameter));
        }

        #endregion
    }
}