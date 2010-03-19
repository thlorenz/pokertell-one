namespace Tools
{
    using System;

    using Interfaces;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Unity;

    using Validation;

    using WPF;
    using WPF.Interfaces;
    using WPF.ViewModels;

    public class ToolsModule : IModule
    {
        readonly IUnityContainer _container;

        public ToolsModule(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Only here to allow PokerTell to register its dependencies on tools here.
        /// </summary>
        public void Initialize()
        {
            _container

                .RegisterType<IDispatcherTimer, DispatcherTimerAdapter>()
                .RegisterType<ICollectionValidator, CollectionValidator>()

                // Tools.WPF
                .RegisterType<IPositionViewModel, PositionViewModel>();
        }
    }
}