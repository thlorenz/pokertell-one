namespace Moq
{
    using System;
    using System.Linq.Expressions;

    using Language.Flow;

    public class StubWith<TInterface>
        where TInterface : class
    {
        #region Constants and Fields

        Mock<TInterface> _mock;

        #endregion

        #region Constructors and Destructors

        internal StubWith()
        {
        }

        #endregion

        #region Public Methods

        public StubSetup<TInterface, TProperty> Get<TProperty>(
            Expression<Func<TInterface, TProperty>> expression)
        {
            _mock = _mock ?? new Mock<TInterface>();
            ISetupGetter<TInterface, TProperty> getter = _mock.SetupGet(expression);

            return new StubSetup<TInterface, TProperty>(_mock, getter);
        }

        #endregion
    }
}