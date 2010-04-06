namespace PokerTell.PokerHand.Tests.Services
{
    using System;

    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Aquisition;
    using PokerTell.PokerHand.Interfaces;
    using PokerTell.PokerHand.Services;
    using PokerTell.UnitTests;

    [TestFixture]
    public class PokerHandConverterTests : TestWithLog
    {
        const double BigBlind = 10.0;

        const double SmallBlind = 5.0;

        IAquiredPokerHand _aquiredHand;

        UnityContainer _container;

        MockPokerHandConverter _converter;

        [SetUp]
        public void _Init()
        {
            const int someGameId = 1;
            DateTime someDateTime = DateTime.MinValue;
            const int someTotalPlayers = 6;

            _aquiredHand = new AquiredPokerHand(
                "SomeSite", someGameId, someDateTime, BigBlind, SmallBlind, someTotalPlayers);
        }

        [TestFixtureSetUp]
        public void _InitConverter()
        {
            _container = new UnityContainer();

            _container
                .RegisterConstructor<IConvertedPokerAction, ConvertedPokerAction>()
                .RegisterConstructor<IConvertedPokerActionWithId, ConvertedPokerActionWithId>()
                .RegisterConstructor<IConvertedPokerRound, ConvertedPokerRound>()
                .RegisterConstructor<IConvertedPokerPlayer, ConvertedPokerPlayer>()
                .RegisterConstructor<IConvertedPokerHand, ConvertedPokerHand>()
                .RegisterType<IPokerActionConverter, PokerActionConverter>()
                .RegisterType<IPokerRoundsConverter, PokerRoundsConverter>();

            _converter = _container.Resolve<MockPokerHandConverter>();
        }

        [Test]
        public void RemovePostingActionsAndCalculatePotAfterPosting_NoPlayers_ReturnsZero()
        {
            const double expectedPot = 0;

            double calculatedPot = _converter.CallRemovePostingActionsAndCalculatePotAfterPosting(ref _aquiredHand);

            Assert.That(calculatedPot, Is.EqualTo(expectedPot));
        }

        [Test]
        public void
            RemovePostingActionsAndCalculatePotAfterPosting_OneActivePostingPlayer_RemovesPostingActionFromPlayer()
        {
            const double postedAmount = 1.0;
            IAquiredPokerPlayer postingPlayer = CreatePostingPlayer("someName", postedAmount);
            postingPlayer[Streets.PreFlop].Add(new AquiredPokerAction(ActionTypes.F, 1.0));
            _aquiredHand.AddPlayer(postingPlayer);

            _converter.CallRemovePostingActionsAndCalculatePotAfterPosting(ref _aquiredHand);

            bool firstActionIsNotPostingAction = ! postingPlayer[Streets.PreFlop][0].What.Equals(ActionTypes.P);
            Assert.That(firstActionIsNotPostingAction);
        }

        [Test]
        public void RemovePostingActionsAndCalculatePotAfterPosting_OneInactivePostingPlayer_RemovesPlayerFromHand()
        {
            const double postedAmount = 1.0;
            IAquiredPokerPlayer postingPlayer = CreatePostingPlayer("someName", postedAmount);
            _aquiredHand.AddPlayer(postingPlayer);

            _converter.CallRemovePostingActionsAndCalculatePotAfterPosting(ref _aquiredHand);

            bool playerWasRemoved = !_aquiredHand.Players.Contains(postingPlayer);
            Assert.That(playerWasRemoved);
        }

        [Test]
        public void RemovePostingActionsAndCalculatePotAfterPosting_OneNonPostingPlayer_ReturnsZero()
        {
            _aquiredHand.AddPlayer(CreateNonPostingActivePlayer("someName"));
            const double expectedPot = 0;

            double calculatedPot = _converter.CallRemovePostingActionsAndCalculatePotAfterPosting(ref _aquiredHand);

            Assert.That(calculatedPot, Is.EqualTo(expectedPot));
        }

        [Test]
        public void RemovePostingActionsAndCalculatePotAfterPosting_OnePlayerWithoutRound_ReturnsZero()
        {
            _aquiredHand.AddPlayer(CreateAquiredPlayer("someName"));
            const double expectedPot = 0;

            double calculatedPot = _converter.CallRemovePostingActionsAndCalculatePotAfterPosting(ref _aquiredHand);

            Assert.That(calculatedPot, Is.EqualTo(expectedPot));
        }

        [Test]
        public void RemovePostingActionsAndCalculatePotAfterPosting_OnePostingPlayer_ReturnsPostedAmount()
        {
            const double postedAmount = 1.0;
            _aquiredHand.AddPlayer(CreatePostingPlayer("someName", postedAmount));
            const double expectedPot = postedAmount;

            double calculatedPot = _converter.CallRemovePostingActionsAndCalculatePotAfterPosting(ref _aquiredHand);

            Assert.That(calculatedPot, Is.EqualTo(expectedPot));
        }

        [Test]
        public void RemovePostingActionsAndCalculatePotAfterPosting_TwoPostingPlayers_ReturnsSumOfPostedAmounts()
        {
            const double postedAmount1 = 1.0;
            const double postedAmount2 = 2.0;
            _aquiredHand.AddPlayer(CreatePostingPlayer("someName", postedAmount1));
            _aquiredHand.AddPlayer(CreatePostingPlayer("someName", postedAmount2));
            const double expectedPot = postedAmount1 + postedAmount2;

            double calculatedPot = _converter.CallRemovePostingActionsAndCalculatePotAfterPosting(ref _aquiredHand);

            Assert.That(calculatedPot, Is.EqualTo(expectedPot));
        }

        static IAquiredPokerPlayer CreateAquiredPlayer(string someName)
        {
            const int someStack = 1;
            return new AquiredPokerPlayer().InitializeWith(someName, someStack);
        }

        static IAquiredPokerPlayer CreateNonPostingActivePlayer(string someName)
        {
            return CreateNonPostingActivePlayer(someName, ActionTypes.C, 1.0);
        }

        static IAquiredPokerPlayer CreateNonPostingActivePlayer(string someName, ActionTypes action, double ratio)
        {
            IAquiredPokerPlayer aquiredPlayer = CreateAquiredPlayer(someName);
            var round = new AquiredPokerRound();
            round.Add(new AquiredPokerAction(action, ratio));
            aquiredPlayer.AddRound(round);

            return aquiredPlayer;
        }

        static IAquiredPokerPlayer CreatePostingPlayer(string someName, double postedAmount)
        {
            IAquiredPokerPlayer aquiredPlayer = CreateAquiredPlayer(someName);
            var round = new AquiredPokerRound();
            round.Add(new AquiredPokerAction(ActionTypes.P, postedAmount));
            aquiredPlayer.AddRound(round);

            return aquiredPlayer;
        }
    }

    internal class MockPokerHandConverter : PokerHandConverter
    {
        public MockPokerHandConverter(
            IConstructor<IConvertedPokerPlayer> convertedPlayer, 
            IConstructor<IConvertedPokerHand> convertedHand, 
            IPokerRoundsConverter roundsConverter)
            : base(convertedPlayer, convertedHand, roundsConverter)
        {
        }

        public double CallRemovePostingActionsAndCalculatePotAfterPosting(ref IAquiredPokerHand aquiredHand)
        {
            return RemovePostingActionsAndCalculatePotAfterPosting(ref aquiredHand);
        }
    }
}