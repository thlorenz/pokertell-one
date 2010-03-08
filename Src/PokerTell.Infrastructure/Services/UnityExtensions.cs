namespace Microsoft.Practices.Unity
{
    using System;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Services;

    public static class UnityExtensions
    {
        #region Public Methods

        /// <summary>
        /// Registers the default constructor for the given concrete type as
        /// <code><![CDATA[IConstructor<TInterface>]]></code>
        /// with the container.
        /// </summary>
        /// <example>
        /// Assuming consumer needs to create objects of type IModel
        /// <code>
        /// <![CDATA[_container
        ///       .RegisterConstructor<IModel, Model>()
        ///        .RegisterType<IConsumer, Consumer>();
        /// var consumer = _container.Resolve<IConsumer>();]]>
        /// </code>
        /// </example>
        /// <typeparam name="TInterface">Interface that represents type</typeparam>
        /// <typeparam name="TConcreteType">Concrete type to be constructed</typeparam>
        /// <param name="container"></param>
        /// <returns>IUnityContainer with registered constructor mapping</returns>
        public static IUnityContainer RegisterConstructor<TInterface, TConcreteType>(this IUnityContainer container)
            where TConcreteType : TInterface, new()
        {
            return container.RegisterConstructor<TInterface>(() => new TConcreteType());
        }

        /// <summary>
        /// Registers the supplied constructor for the given type.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="container"></param>
        /// <param name="constructor">Function containing Construction code e.g. () => _container.Resolve<I>();</I></param>
        /// <returns>IUnityContainer with registered constructor mapping</returns>
        public static IUnityContainer RegisterConstructor<TInterface>(this IUnityContainer container, Func<TInterface> constructor)
        {
            if (container != null)
            {
                container
                    .RegisterInstance<IConstructor<TInterface>>(new Constructor<TInterface>(constructor));
            }
            else
            {
                throw new ArgumentNullException("container");
            }

            return container;
        }

        /// <summary>
        /// Registers the default constructor for the given concrete type as
        /// <code><![CDATA[IConstructor<TInterface>]]></code>
        /// with the container.
        /// It also performs
        /// <code><![CDATA[RegisterType<TInterface, TConcreteType>()]]></code>
        /// on the container.
        /// </summary>
        /// <example>
        /// Assuming consumer needs to create objects of type IModel
        /// <code>
        /// <![CDATA[_container
        ///       .RegisterConstructor<IModel, Model>()
        ///        .RegisterTypeAndConstructor<IConsumer, Consumer>();
        /// var consumer = _container.Resolve<IConsumer>();]]>
        /// </code>
        /// </example>
        /// <typeparam name="TInterface">Interface that represents type</typeparam>
        /// <typeparam name="TConcreteType">Concrete type to be constructed</typeparam>
        /// <param name="container"></param>
        /// <returns>IUnityContainer with registered typemapping and constructor</returns>
        public static IUnityContainer RegisterTypeAndConstructor<TInterface, TConcreteType>(
            this IUnityContainer container)
            where TConcreteType : TInterface, new()
        {
            if (container != null)
            {
                container
                    .RegisterType<TInterface, TConcreteType>()
                    .Configure<InjectedMembers>()
                    .ConfigureInjectionFor<TConcreteType>(new InjectionConstructor())
                    .Container
                    .RegisterInstance<IConstructor<TInterface>>(new Constructor<TInterface>(container.Resolve<TInterface>));
            }
            else
            {
                throw new ArgumentNullException("container");
            }

            return container;
        }

        public static IUnityContainer RegisterTypeAndConstructor<TInterface, TConcreteType>(
            this IUnityContainer container, Func<TInterface> constructor)
            where TConcreteType : TInterface
        {
            return container
                .RegisterType<TInterface, TConcreteType>()
                .RegisterConstructor(constructor);
        }


        #endregion
    }
}