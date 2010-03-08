namespace Moq
{
    using System;
    using System.Linq.Expressions;

    using Language.Flow;

    public class StubSetup<TInterface, TProperty>
        where TInterface : class
    {
        #region Constants and Fields

        readonly ISetupGetter<TInterface, TProperty> _getter;

        readonly Mock<TInterface> _mock;

        #endregion

        #region Constructors and Destructors

        internal StubSetup(Mock<TInterface> mock, ISetupGetter<TInterface, TProperty> getter)
        {
            _getter = getter;
            _mock = mock;
        }

        #endregion

        #region Properties

        public TInterface Out
        {
            get { return _mock.Object; }
        }

        #endregion

        #region Public Methods

        public StubSetup<TInterface, TNextProperty> Get<TNextProperty>(
            Expression<Func<TInterface, TNextProperty>> expression)
        {
            ISetupGetter<TInterface, TNextProperty> getter = _mock.SetupGet(expression);

            return new StubSetup<TInterface, TNextProperty>(_mock, getter);
        }

        public StubSetup<TInterface, TProperty> Returns(TProperty value)
        {
            _getter.Returns(value);
            return this;
        }

        #endregion
    }
}