namespace Tools
{
    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Unity;

    using Tools.Interfaces;
    using Tools.Validation;
    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    public class ToolsModule : IModule
    {
        readonly IUnityContainer _container;

        public ToolsModule(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Only here to allow consuming Prism applications to register its dependencies on tools.
        /// </summary>
        public void Initialize()
        {
            _container
                .RegisterType<IDispatcherTimer, DispatcherTimerAdapter>()
                .RegisterType<ICollectionValidator, CollectionValidator>()

                // Tools.WPF
                .RegisterType<IDimensionsViewModel, DimensionsViewModel>(new InjectionConstructor())
                .RegisterType<IPositionViewModel, PositionViewModel>(new InjectionConstructor());
        }
    }
}