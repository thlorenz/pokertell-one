namespace PokerTell.PokerHandParsers
{
    using System.Reflection;

    using Infrastructure.Interfaces.PokerHandParsers;
    using Infrastructure.Interfaces.Repository;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Unity;

    public class PokerHandParsersModule : IModule
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        #endregion

        #region Constructors and Destructors

        public PokerHandParsersModule(IUnityContainer container)
        {
            _container = container;
        }

        #endregion

        #region Implemented Interfaces

        #region IModule

        public void Initialize()
        {
            var pokerHandParsers = new AvaliablePokerHandParsers(true)
                .Add(_container.Resolve<FullTiltPoker.PokerHandParser>())
                .Add(_container.Resolve<PokerStars.PokerHandParser>());

            _container
                .RegisterInstance<IPokerHandParsers>(pokerHandParsers);
                
            Log.Info("got initialized.");
        }

        #endregion

        #endregion
    }



}