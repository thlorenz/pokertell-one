namespace PokerTell.PokerHand.Tests
{
    using System;

    using Infrastructure.Interfaces;

    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Services;
    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Aquisition;
    using PokerTell.PokerHand.Services;

    [TestFixture]
    internal class ThatPokerHandConverter
    {
        #region Constants and Fields

        const double BigBlind = 10.0;

        const double SmallBlind = 5.0;

        IAquiredPokerHand _aquiredHand;

        IConvertedPokerHand _convertedHand;

        MockPokerHandConverter _converter;

        UnityContainer _container;

        #endregion

        #region Public Methods

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

//        [Test]
//        public void ConvertPreflop_AquiredHandWithoutPlayers_DoesntChangePot()
//        {
//            const double originalPot = 1.0;
//            double pot = originalPot;
//            double someToCall = 0;
//
//            _converter.CallConvertPreflop(_aquiredHand, ref _convertedHand, ref pot, ref someToCall);
//
//            Assert.That(pot, Is.EqualTo(originalPot));
//        }
//
//        [Test]
//        public void ConvertPreflop_AquiredHandWithoutPlayers_DoesntChangeToCall()
//        {
//            const double originalToCall = 1.0;
//            double somePot = 0;
//            double toCall = originalToCall;
//
//            _converter.CallConvertPreflop(_aquiredHand, ref _convertedHand, ref somePot, ref toCall);
//
//            Assert.That(toCall, Is.EqualTo(originalToCall));
//        }
//
//        [Test]
//        public void ConvertPreflop_AquiredHandWithThreePlayersWhoRaise_SetsToCallToBiggestRatio()
//        {
//            double somePot = 0;
//            const double originalToCall = 0;
//            double toCall = originalToCall;
//            const ActionTypes action = ActionTypes.R;
//
//            const double ratio1 = 1.0;
//            const double ratio2 = 4.0;
//            const double ratio3 = 2.0;
//            const double expectedToCall = ratio2;
//
//            _aquiredHand.AddPlayer(CreateNonPostingActivePlayer("someName1", action, ratio1));
//            _aquiredHand.AddPlayer(CreateNonPostingActivePlayer("someName2", action, ratio2));
//            _aquiredHand.AddPlayer(CreateNonPostingActivePlayer("someName2", action, ratio3));
//
//            _converter.CallConvertPreflop(_aquiredHand, ref _convertedHand, ref somePot, ref toCall);
//
//            Assert.That(toCall, Is.EqualTo(expectedToCall));
//        }
//
//        [Test]
//        [Sequential]
//        public void ConvertPreflop_AquiredHandWithTwoPlayersCallBetRaise_AddsSumOfRatiosToPot(
//            [Values(ActionTypes.C, ActionTypes.B, ActionTypes.R)] ActionTypes action)
//        {
//            const double originalPot = 1.0;
//            double someToCall = 0;
//
//            double pot = originalPot;
//            const double ratio1 = 1.0;
//            const double ratio2 = 2.0;
//            _aquiredHand.AddPlayer(CreateNonPostingActivePlayer("someName1", action, ratio1));
//            _aquiredHand.AddPlayer(CreateNonPostingActivePlayer("someName2", action, ratio2));
//            const double expectedPot = originalPot + ratio1 + ratio2;
//
//            _converter.CallConvertPreflop(_aquiredHand, ref _convertedHand, ref pot, ref someToCall);
//
//            Assert.That(pot, Is.EqualTo(expectedPot));
//        }
//
//        [Test]
//        public void ConvertPreflop_AquiredHandWithTwoPlayersWhoFoldCheckCallWinAllin_DoesntChangeToCall(
//            [Values(ActionTypes.F, ActionTypes.X, ActionTypes.C, ActionTypes.W, ActionTypes.A)] ActionTypes action)
//        {
//            double somePot = 0;
//            const double originalToCall = 0;
//            double toCall = originalToCall;
//
//            const double ratio1 = 1.0;
//            const double ratio2 = 2.0;
//            _aquiredHand.AddPlayer(CreateNonPostingActivePlayer("someName1", action, ratio1));
//            _aquiredHand.AddPlayer(CreateNonPostingActivePlayer("someName2", action, ratio2));
//            const double expectedToCall = originalToCall;
//
//            _converter.CallConvertPreflop(_aquiredHand, ref _convertedHand, ref somePot, ref toCall);
//
//            Assert.That(toCall, Is.EqualTo(expectedToCall));
//        }
//
//        [Test]
//        public void ConvertPreflop_AquiredHandWithTwoPlayersWhoFoldCheckWinAllin_AddsDoesntChangePot(
//            [Values(ActionTypes.F, ActionTypes.X, ActionTypes.W, ActionTypes.A)] ActionTypes action)
//        {
//            const double originalPot = 1.0;
//            double someToCall = 0;
//
//            double pot = originalPot;
//            const double ratio1 = 1.0;
//            const double ratio2 = 2.0;
//            _aquiredHand.AddPlayer(CreateNonPostingActivePlayer("someName1", action, ratio1));
//            _aquiredHand.AddPlayer(CreateNonPostingActivePlayer("someName2", action, ratio2));
//            const double expectedPot = originalPot;
//
//            _converter.CallConvertPreflop(_aquiredHand, ref _convertedHand, ref pot, ref someToCall);
//
//            Assert.That(pot, Is.EqualTo(expectedPot));
//        }
//
//        [Test]
//        public void ConvertPreflop_AquiredPlayerWithOneRound_AddsOneRoundToConvertedPlayer()
//        {
//            double somePot = 0;
//            double someToCall = 0;
//
//            IAquiredPokerPlayer aquiredPlayer = CreateNonPostingActivePlayer("Player");
//            _aquiredHand.AddPlayer(aquiredPlayer);
//            _aquiredHand.AddPlayer(CreateNonPostingActivePlayer("anotherPlayer"));
//
//            var convertedPlayer = new ConvertedPokerPlayer();
//            _convertedHand.AddPlayer(convertedPlayer);
//            _convertedHand.AddPlayer(new ConvertedPokerPlayer());
//
//            int expectedRoundCount = aquiredPlayer.Count;
//
//            _converter.CallConvertPreflop(_aquiredHand, ref _convertedHand, ref somePot, ref someToCall);
//
//            Assert.That(convertedPlayer.Count, Is.EqualTo(expectedRoundCount));
//        }
//
//        [Test]
//        public void ConvertPreflop_AquiredPlayerWithOneRound_AddsRoundWithSameActionToConvertedPlayer()
//        {
//            double somePot = 0;
//            double someToCall = 0;
//            const ActionTypes action = ActionTypes.B;
//            const double someRatio = 1.0;
//
//            IAquiredPokerPlayer aquiredPlayer = CreateNonPostingActivePlayer("Player", action, someRatio);
//            _aquiredHand.AddPlayer(aquiredPlayer);
//            _aquiredHand.AddPlayer(CreateNonPostingActivePlayer("anotherPlayer"));
//
//            var convertedPlayer = new ConvertedPokerPlayer();
//            _convertedHand.AddPlayer(convertedPlayer);
//            _convertedHand.AddPlayer(new ConvertedPokerPlayer());
//
//            int expectedRoundCount = aquiredPlayer.Count;
//
//            _converter.CallConvertPreflop(_aquiredHand, ref _convertedHand, ref somePot, ref someToCall);
//
//            Assert.That(convertedPlayer[0][0].What, Is.EqualTo(aquiredPlayer[0][0].What));
//        }

        [SetUp]
        public void _Init()
        {
            const int someGameId = 1;
            DateTime someDateTime = DateTime.MinValue;
            const int someTotalPlayers = 6;

            _aquiredHand = new AquiredPokerHand(
                "SomeSite", someGameId, someDateTime, BigBlind, SmallBlind, someTotalPlayers);

            _convertedHand = new ConvertedPokerHand();
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
            postingPlayer[Streets.PreFlop].AddAction(new AquiredPokerAction(ActionTypes.F, 1.0));
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

        #endregion

        // [Test]
        // public void ConvertPreflop_AquiredPlayerWithOneRound_AddsRelativeRatioActionOfToConvertedPlayer()
        // {
        // double pot = 2.0;
        // double someToCall = 0;
        // const ActionTypes action = ActionTypes.C;
        // const double ratio = 0.9;
        // double expectedRatio = ratio / pot;
        // IAquiredPokerPlayer aquiredPlayer = CreateNonPostingActivePlayer("Player", action, ratio);
        // _aquiredHand.AddPlayer(aquiredPlayer);
        // _aquiredHand.AddPlayer(CreateNonPostingActivePlayer("anotherPlayer"));
        // var convertedPlayer = new ConvertedPokerPlayer();
        // _convertedHand.AddPlayer(convertedPlayer);
        // _convertedHand.AddPlayer(new ConvertedPokerPlayer());
        // _converter.CallConvertPreflop(_aquiredHand, ref _convertedHand, ref pot, ref someToCall);
        // Assert.That(convertedPlayer[0][0].Ratio, Is.EqualTo(expectedRatio));
        // }
        #region Methods

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
            round.AddAction(new AquiredPokerAction(action, ratio));
            aquiredPlayer.AddRound(round);

            return aquiredPlayer;
        }

        static IAquiredPokerPlayer CreatePostingPlayer(string someName, double postedAmount)
        {
            IAquiredPokerPlayer aquiredPlayer = CreateAquiredPlayer(someName);
            var round = new AquiredPokerRound();
            round.AddAction(new AquiredPokerAction(ActionTypes.P, postedAmount));
            aquiredPlayer.AddRound(round);

            return aquiredPlayer;
        }

        #endregion
    }

    internal class MockPokerHandConverter : Services.PokerHandConverter
    {
        #region Constructors and Destructors

        public MockPokerHandConverter(
            IConstructor<IConvertedPokerPlayer> convertedPlayer, 
            IConstructor<IConvertedPokerHand> convertedHand,
            IPokerRoundsConverter roundsConverter)
            : base(convertedPlayer, convertedHand, roundsConverter)
        {
        }

        #endregion

        #region Public Methods

        public double CallRemovePostingActionsAndCalculatePotAfterPosting(ref IAquiredPokerHand aquiredHand)
        {
            return RemovePostingActionsAndCalculatePotAfterPosting(ref aquiredHand);
        }

        #endregion
    }
}