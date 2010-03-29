namespace PokerTell.PokerHandParsers.IntegrationTests
{
    using System;
    using System.IO;
    using System.Linq;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.PokerHandParsers;

    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    using PokerHand.Aquisition;

    using PokerStars;

    using Properties;

    using UnitTests;

    using UnitTests.Tools;

    public class PokerHandParserTests : TestWithLog
    {
        IUnityContainer _container;

        IPokerHandParser _parser;

        [SetUp]
        public void _Init()
        {
            _container = new UnityContainer();
            _container
                .RegisterConstructor<IAquiredPokerAction, AquiredPokerAction>()
                .RegisterConstructor<IAquiredPokerRound, AquiredPokerRound>()
                .RegisterConstructor<IAquiredPokerPlayer, AquiredPokerPlayer>()
                .RegisterConstructor<IAquiredPokerHand, AquiredPokerHand>();
            _parser = _container.Resolve<PokerHandParser>();
            _parser.LogVerbose = true;
        }

        [Test]
        public void PokerStarsParser_PlayerWasAllinPreflop_PlayersWinningActionWillBeAddedToFlop()
        {
            string handHistory = Resources.PokerStars_RenniwegIsAllInPreflopAndWins1885;

            var renniweg = _parser
                .ParseHand(handHistory)
                .AquiredPokerHand.Players.First(p => p.Name == "renniweg");

            var firstFlopAction = renniweg.Rounds[(int)Streets.Flop].Actions.First();

            firstFlopAction.What.ShouldBeEqualTo(ActionTypes.W);
            firstFlopAction.Ratio.ShouldBeEqualTo(1885);
        }
    }
}