namespace MicrosoftPractices.Tests
{
    using System;

    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Services;

    [TestFixture]
    internal class ThatUnity
    {
        #region Constants and Fields

        IUnityContainer _container;

        #endregion

        #region Public Methods

        [SetUp]
        public void Init()
        {
            _container = new UnityContainer();
        }

        [Test]
        public void ResolvedFuncInvoke_PointingAtContainerResolve_ReturnsNewInstances()
        {
            _container
                .RegisterType<IModel, Model>()
                .RegisterInstance<Func<IModel>>(() => _container.Resolve<IModel>())
                .RegisterType<IConsumer, Consumer>();

            var consumer = _container.Resolve<IConsumer>();
           
            Assert.That(consumer.ResolvedModelsAreUnique);
        }

        [Test]
        public void ResolvedFuncInvoke_PointingAtContainerResolveViaConstructorExtension_ReturnsNewInstances()
        {
            _container
                .RegisterConstructor<IModel, Model>()
                .RegisterType<IConsumer, Consumer2>();

            var consumer = _container.Resolve<IConsumer>();
           
            Assert.That(consumer.ResolvedModelsAreUnique);
        }

        [Test]
        public void ResolvedFuncInvoke_PointingAtContainerResolveViaFunctionWrapper_ReturnsNewInstances()
        {
            _container
                .RegisterType<IModel, Model>()
                .RegisterInstance<IConstructor<IModel>>(new Constructor<IModel>(() => _container.Resolve<IModel>()))
                .RegisterType<IConsumer, Consumer2>();

            var consumer = _container.Resolve<IConsumer>();

            Assert.That(consumer.ResolvedModelsAreUnique);
        }

        [Test]
        public void ResolvedFuncInvoke_PointingAtInstantiatingCodeBlock_ReturnsNewInstances()
        {
            _container
                .RegisterType<IModel, Model>()
                .RegisterInstance<Func<IModel>>(() => new Model())
                .RegisterType<IConsumer, Consumer>();

            var consumer = _container.Resolve<IConsumer>();

            Assert.That(consumer.ResolvedModelsAreUnique);
        }

        #endregion
    }

    internal interface IConsumer
    {
        #region Properties

        bool ResolvedModelsAreUnique { get; }

        #endregion

        #region Public Methods

        string ToString();

        #endregion
    }

    internal class Consumer : IConsumer
    {
        #region Constants and Fields

        IModel _model1;

        IModel _model2;

        #endregion

        #region Constructors and Destructors

        public Consumer(Func<IModel> newModel)
        {
            CreateModels(newModel);
        }

        #endregion

        #region Properties

        public bool ResolvedModelsAreUnique
        {
            get { return (_model1 != null) && (_model2 != null) && (! _model1.Id.Equals(_model2.Id)); }
        }

        #endregion

        #region Implemented Interfaces

        #region IConsumer

        public override string ToString()
        {
            return string.Format("Consumer holds models:\n{0}\n{1}", _model1, _model2);
        }

        #endregion

        #endregion

        #region Methods

        void CreateModels(Func<IModel> newModel)
        {
            _model1 = newModel.Invoke().InitializeWith(1);
            _model2 = newModel.Invoke().InitializeWith(2);
        }

        #endregion
    }

    internal class Consumer2 : IConsumer
    {
        IModel _model1;

        IModel _model2;

        public Consumer2(IConstructor<IModel> model)
        {
            CreateModels(model);
        }

        public bool ResolvedModelsAreUnique
        {
            get { return (_model1 != null) && (_model2 != null) && (!_model1.Id.Equals(_model2.Id)); }
        }

        public override string ToString()
        {
            return string.Format("Consumer holds models:\n{0}\n{1}", _model1, _model2);
        }

        void CreateModels(IConstructor<IModel> model)
        {
            _model1 = model.New.InitializeWith(1);
            _model2 = model.New.InitializeWith(2);
        }
    }

    internal interface IModel
    {
        #region Properties

        int Id { get; set; }

        #endregion

        #region Public Methods

        IModel InitializeWith(int id);

        string ToString();

        #endregion
    }

    internal class Model : IModel
    {
        #region Properties

        public int Id { get; set; }

        #endregion

        #region Implemented Interfaces

        #region IModel

        public IModel InitializeWith(int id)
        {
            Id = id;
            return this;
        }

        public override string ToString()
        {
            return "Model #" + Id;
        }

        #endregion

        #endregion
    }
}