namespace PokerTell.PokerHandParsers
{
    using System.Reflection;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Unity;

    using PokerTell.Infrastructure.Interfaces.PokerHandParsers;

    public class PokerHandParsersModule : IModule
    {
        static readonly ILog Log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        public PokerHandParsersModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container
                .RegisterType<FullTiltPoker.PokerHandParser>()
                .RegisterType<PokerStars.PokerHandParser>()
                .RegisterType<IPokerHandParsers, AvailablePokerHandParsers>();

            Log.Info("got initialized.");
        }
    }
}