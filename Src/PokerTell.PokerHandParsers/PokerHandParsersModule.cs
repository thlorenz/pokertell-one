namespace PokerTell.PokerHandParsers
{
    using System.Reflection;

    using Interfaces;
    using Interfaces.Parsers;

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

                // PokerStars
                .RegisterType<IPokerStarsAnteParser, PokerStars.AnteParser>()
                .RegisterType<IPokerStarsBlindsParser, PokerStars.BlindsParser>()
                .RegisterType<IPokerStarsBoardParser, PokerStars.BoardParser>()
                .RegisterType<IPokerStarsGameTypeParser, PokerStars.GameTypeParser>()
                .RegisterType<IPokerStarsHandHeaderParser, PokerStars.HandHeaderParser>()
                .RegisterType<IPokerStarsHeroNameParser, PokerStars.HeroNameParser>()
                .RegisterType<IPokerStarsHoleCardsParser, PokerStars.HoleCardsParser>()
                .RegisterType<IPokerStarsPlayerActionsParser, PokerStars.PlayerActionsParser>()
                .RegisterType<IPokerStarsPlayerSeatsParser, PokerStars.PlayerSeatsParser>()
                .RegisterType<IPokerStarsSmallBlindPlayerNameParser, PokerStars.SmallBlindPlayerNameParser>()
                .RegisterType<IPokerStarsStreetsParser, PokerStars.StreetsParser>()
                .RegisterType<IPokerStarsTableNameParser, PokerStars.TableNameParser>()
                .RegisterType<IPokerStarsTimeStampParser, PokerStars.TimeStampParser>()
                .RegisterType<IPokerStarsTotalPotParser, PokerStars.TotalPotParser>()
                .RegisterType<IPokerStarsTotalSeatsParser, PokerStars.TotalSeatsParser>()

                // FullTiltPoker
                .RegisterType<IFullTiltPokerAnteParser, FullTiltPoker.AnteParser>()
                .RegisterType<IFullTiltPokerBlindsParser, FullTiltPoker.BlindsParser>()
                .RegisterType<IFullTiltPokerBoardParser, FullTiltPoker.BoardParser>()
                .RegisterType<IFullTiltPokerGameTypeParser, FullTiltPoker.GameTypeParser>()
                .RegisterType<IFullTiltPokerHandHeaderParser, FullTiltPoker.HandHeaderParser>()
                .RegisterType<IFullTiltPokerHeroNameParser, FullTiltPoker.HeroNameParser>()
                .RegisterType<IFullTiltPokerHoleCardsParser, FullTiltPoker.HoleCardsParser>()
                .RegisterType<IFullTiltPokerPlayerActionsParser, FullTiltPoker.PlayerActionsParser>()
                .RegisterType<IFullTiltPokerPlayerSeatsParser, FullTiltPoker.PlayerSeatsParser>()
                .RegisterType<IFullTiltPokerSmallBlindPlayerNameParser, FullTiltPoker.SmallBlindPlayerNameParser>()
                .RegisterType<IFullTiltPokerStreetsParser, FullTiltPoker.StreetsParser>()
                .RegisterType<IFullTiltPokerTableNameParser, FullTiltPoker.TableNameParser>()
                .RegisterType<IFullTiltPokerTimeStampParser, FullTiltPoker.TimeStampParser>()
                .RegisterType<IFullTiltPokerTotalPotParser, FullTiltPoker.TotalPotParser>()
                .RegisterType<IFullTiltPokerTotalSeatsParser, FullTiltPoker.TotalSeatsParser>()

                .RegisterType<ITotalSeatsForTournamentsRecordKeeper, TotalSeatsForTournamentsRecordKeeper>(new ContainerControlledLifetimeManager())

                .RegisterType<FullTiltPoker.PokerHandParser>()
                .RegisterType<PokerStars.PokerHandParser>()
                .RegisterType<IPokerHandParsers, AvailablePokerHandParsers>();

            Log.Info("got initialized.");
        }
    }
}