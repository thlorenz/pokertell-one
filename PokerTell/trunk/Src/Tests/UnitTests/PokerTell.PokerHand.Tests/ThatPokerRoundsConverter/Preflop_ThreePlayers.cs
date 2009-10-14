namespace PokerTell.PokerHand.Tests.ThatPokerRoundsConverter
{
    using System;

    using Analyzation;

    using Aquisition;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Services;

    using Microsoft.Practices.Unity;

    using Mocks;

    using Moq;

    using NUnit.Framework;

    using Services;

    using UnitTests;

    public class Preflop_ThreePlayers
    {
        #region Constants and Fields

        IUnityContainer _container;

        Constructor<IConvertedPokerPlayer> _convertedPlayerMake;

        MockPokerRoundsConverter _converter;

        StubBuilder _stub;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();

            _convertedPlayerMake = new Constructor<IConvertedPokerPlayer>(() => new ConvertedPokerPlayer());

            _container = new UnityContainer();

            _container
                .RegisterConstructor<IConvertedPokerAction, ConvertedPokerAction>()
                .RegisterConstructor<IConvertedPokerActionWithId, ConvertedPokerActionWithId>()
                .RegisterConstructor<IConvertedPokerRound, ConvertedPokerRound>()
                .RegisterType<IPokerActionConverter, PokerActionConverter>()
                .RegisterType<IPokerRoundsConverter, MockPokerRoundsConverter>();

            _converter = (MockPokerRoundsConverter)_container.Resolve<IPokerRoundsConverter>();

            _converter
                .InitializeWith(
                _stub.Setup<IAquiredPokerHand>()
                    .Get(hand => hand.TotalPot).Returns(_stub.Valid(For.TotalPot, 1.0)).Out, 
                _stub.Out<IConvertedPokerHand>(), 
                _stub.Valid(For.Pot, 1.0), 
                _stub.Out<double>(For.ToCall));
        }

        [Test]
        public void ConvertPreflop_Player1IsSmallBlind__SetsSequencesCorrectly()
        {
            RelativeRatioResult result = ConvertPreflopThreePlayersHand();
            IConvertedPokerHand convHand = result.ConvertedHand;
            IConvertedPokerPlayer smallBlindPlayer = convHand[0];
            IConvertedPokerPlayer bigBlindPlayer = convHand[1];
            IConvertedPokerPlayer buttonPlayer = convHand[2];

            var action1 = new ConvertedPokerActionWithId(buttonPlayer[Streets.PreFlop][0], buttonPlayer.Position);
            var action2 = new ConvertedPokerActionWithId(
                smallBlindPlayer[Streets.PreFlop][0], smallBlindPlayer.Position);
            var action3 = new ConvertedPokerActionWithId(bigBlindPlayer[Streets.PreFlop][0], bigBlindPlayer.Position);
            var action4 = new ConvertedPokerActionWithId(buttonPlayer[Streets.PreFlop][1], buttonPlayer.Position);
            var action5 = new ConvertedPokerActionWithId(
                smallBlindPlayer[Streets.PreFlop][1], smallBlindPlayer.Position);
            var action6 = new ConvertedPokerActionWithId(bigBlindPlayer[Streets.PreFlop][1], bigBlindPlayer.Position);

            IConvertedPokerRound expectedPreflopSequence = new ConvertedPokerRound()
                .AddAction(action1)
                .AddAction(action2)
                .AddAction(action3)
                .AddAction(action4)
                .AddAction(action5)
                .AddAction(action6);

            Assert.That(expectedPreflopSequence, Is.EqualTo(convHand.Sequences[(int)Streets.PreFlop]));
        }

        [Test]
        public void ConvertPreflop_Player1IsSmallBlind_CalculatesBigBlindsRatio1Correctly()
        {
            RelativeRatioResult result = ConvertPreflopThreePlayersHand();

            Assert.That(result.Player2FirstRound[0].Ratio, Is.EqualTo(result.RelativeRatio3));
        }

        [Test]
        public void ConvertPreflop_Player1IsSmallBlind_CalculatesButtonsRatio1Correctly()
        {
            RelativeRatioResult result = ConvertPreflopThreePlayersHand();

            Assert.That(result.Player3FirstRound[0].Ratio, Is.EqualTo(result.RelativeRatio1));
        }

        [Test]
        public void ConvertPreflop_Player1IsSmallBlind_CalculatesButtonsRatio2Correctly()
        {
            RelativeRatioResult result = ConvertPreflopThreePlayersHand();

            Assert.That(result.Player3FirstRound[1].Ratio, Is.EqualTo(result.RelativeRatio4));
        }

        [Test]
        public void ConvertPreflop_Player1IsSmallBlind_CalculatesSmallBlindsRatio1Correctly()
        {
            RelativeRatioResult result = ConvertPreflopThreePlayersHand();

            Assert.That(result.Player1FirstRound[0].Ratio, Is.EqualTo(result.RelativeRatio2));
        }

        [Test]
        public void ConvertPreflop_Player1IsSmallBlind_CalculatesSmallBlindsRatio2Correctly()
        {
            RelativeRatioResult result = ConvertPreflopThreePlayersHand();

            Assert.That(result.Player1FirstRound[1].Ratio, Is.EqualTo(result.RelativeRatio5));
        }

        [Test]
        public void ConvertPreflop_Player1IsSmallBlind_SetsBigBlindsSecondActionToFold()
        {
            RelativeRatioResult result = ConvertPreflopThreePlayersHand();

            Assert.That(result.Player2FirstRound[1].What, Is.EqualTo(ActionTypes.F));
        }

        [Test]
        public void ConvertPreflop_Player1IsSmallBlind_SetsSmallBlindsPreflopSequence()
        {
            const string expectedSequence = "C";

            var result = ConvertPreflopThreePlayersHand();
            var smallBlindPreflopSequence = result.ConvertedHand[0].Sequence[(int)Streets.PreFlop];

            Assert.That(smallBlindPreflopSequence, Is.EqualTo(expectedSequence));
        }

        [Test]
        public void ConvertPreflop_Player1IsSmallBlind_SetsBigBlindsPreflopSequence()
        {
            const string expectedSequence = "R";

            var result = ConvertPreflopThreePlayersHand();
            var bigBlindPreflopSequence = result.ConvertedHand[1].Sequence[(int)Streets.PreFlop];

            Assert.That(bigBlindPreflopSequence, Is.EqualTo(expectedSequence));
        }

        [Test]
        public void ConvertPreflop_Player1IsSmallBlind_SetsButtonsPreflopSequence()
        {
            const string expectedSequence = "C";

            var result = ConvertPreflopThreePlayersHand();
            var buttonPreflopSequence = result.ConvertedHand[2].Sequence[(int)Streets.PreFlop];

            Assert.That(buttonPreflopSequence, Is.EqualTo(expectedSequence));
        }

        #endregion

        #region Methods

        RelativeRatioResult ConvertPreflopThreePlayersHand()
        {
            _stub
                .Value(For.HoleCards).Is(string.Empty);

            const double smallBlind = 1.0;
            const double bigBlind = 2.0;
            const double pot = smallBlind + bigBlind;
            const double toCall = bigBlind;
            const int totalPlayers = 2;
            const int smallBlindPosition = 0;
            const int bigBlindPosition = 1;
            const int buttonPosition = 2;

            var action1 = new AquiredPokerAction(ActionTypes.C, toCall);
            const double relativeRatio1 = toCall / pot;
            const double pot1 = pot + toCall;

            var action2 = new AquiredPokerAction(ActionTypes.C, smallBlind);
            const double relativeRatio2 = smallBlind / pot1;
            const double pot2 = pot1 + smallBlind;

            var action3 = new AquiredPokerAction(ActionTypes.R, bigBlind * 3);
            const double relativeRatio3 = 3;
            const double pot3 = pot2 + (bigBlind * 3);

            var action4 = new AquiredPokerAction(ActionTypes.R, toCall * 3 * 2);
            const double relativeRatio4 = 2;
            const double pot4 = pot3 + (toCall * 3 * 2);

            var action5 = new AquiredPokerAction(ActionTypes.C, toCall * 3 * 2);
            const double relativeRatio5 = (toCall * 2 * 3) / pot4;

            var action6 = new AquiredPokerAction(ActionTypes.F, 1.0);

            // Small Blind
            IAquiredPokerPlayer player1 = new AquiredPokerPlayer(
                _stub.Some<long>(), smallBlindPosition, _stub.Out<string>(For.HoleCards))
                .AddRound(
                new AquiredPokerRound()
                    .AddAction(action2)
                    .AddAction(action5));
            player1.Name = "player1";
            player1.Position = smallBlindPosition;

            // Big Blind
            IAquiredPokerPlayer player2 = new AquiredPokerPlayer(
                _stub.Some<long>(), bigBlindPosition, _stub.Out<string>(For.HoleCards))
                .AddRound(
                new AquiredPokerRound()
                    .AddAction(action3)
                    .AddAction(action6));
            player2.Name = "player2";
            player2.Position = bigBlindPosition;

            // Button
            IAquiredPokerPlayer player3 = new AquiredPokerPlayer(
                _stub.Some<long>(), buttonPosition, _stub.Out<string>(For.HoleCards))
                .AddRound(
                new AquiredPokerRound()
                    .AddAction(action1)
                    .AddAction(action4));
            player2.Name = "player3";
            player2.Position = buttonPosition;

            IAquiredPokerHand aquiredHand =
                new AquiredPokerHand(
                    _stub.Valid(For.Site, "site"), 
                    _stub.Out<ulong>(For.GameId), 
                    _stub.Out<DateTime>(For.TimeStamp), 
                    smallBlind, 
                    bigBlind, 
                    totalPlayers)
                    .AddPlayer(player1)
                    .AddPlayer(player2)
                    .AddPlayer(player3);

            IConvertedPokerHand convertedHand =
                new ConvertedPokerHand(aquiredHand)
                    .InitializeWith(aquiredHand)
                    .AddPlayersFrom(aquiredHand, pot, _convertedPlayerMake);

            _converter
                .InitializeWith(aquiredHand, convertedHand, pot, toCall)
                .ConvertPreflop();

            IConvertedPokerRound player1FirstRound = convertedHand[smallBlindPosition][Streets.PreFlop];
            IConvertedPokerRound player2FirstRound = convertedHand[bigBlindPosition][Streets.PreFlop];
            IConvertedPokerRound player3FirstRound = convertedHand[buttonPosition][Streets.PreFlop];

            return new RelativeRatioResult(
                convertedHand, 
                player1FirstRound, 
                player2FirstRound, 
                player3FirstRound, 
                relativeRatio1, 
                relativeRatio2, 
                relativeRatio3, 
                relativeRatio4, 
                relativeRatio5);
        }

        #endregion

        class RelativeRatioResult
        {
            #region Constants and Fields

            public readonly IConvertedPokerHand ConvertedHand;

            public readonly IConvertedPokerRound Player1FirstRound;

            public readonly IConvertedPokerRound Player2FirstRound;

            public readonly IConvertedPokerRound Player3FirstRound;

            public readonly double RelativeRatio1;

            public readonly double RelativeRatio2;

            public readonly double RelativeRatio3;

            public readonly double RelativeRatio4;

            public readonly double RelativeRatio5;

            #endregion

            #region Constructors and Destructors

            public RelativeRatioResult(
                IConvertedPokerHand convertedHand, 
                IConvertedPokerRound player1FirstRound, 
                IConvertedPokerRound player2FirstRound, 
                IConvertedPokerRound player3FirstRound, 
                double relativeRatio1, 
                double relativeRatio2, 
                double relativeRatio3, 
                double relativeRatio4, 
                double relativeRatio5)
            {
                ConvertedHand = convertedHand;
                Player1FirstRound = player1FirstRound;
                Player2FirstRound = player2FirstRound;
                Player3FirstRound = player3FirstRound;
                RelativeRatio5 = relativeRatio5;
                RelativeRatio4 = relativeRatio4;
                RelativeRatio3 = relativeRatio3;
                RelativeRatio2 = relativeRatio2;
                RelativeRatio1 = relativeRatio1;
            }

            #endregion
        }
    }
}