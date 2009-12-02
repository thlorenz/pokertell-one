namespace PokerTell.PokerHand.Tests.Services.PokerRoundsConverterTests
{
    using System;

    using Fakes;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Services;

    using Microsoft.Practices.Unity;

    using Moq;

    using NUnit.Framework;

    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Aquisition;
    using PokerTell.PokerHand.Services;

    using UnitTests;

    public class Postflop_ThreePlayers
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
        public void ConvertFlop_Player1IsSmallBlind__SetsSequencesCorrectly()
        {
            RelativeRatioResult result = ConvertPreflopThreePlayersHand();
            IConvertedPokerHand convHand = result.ConvertedHand;
            IConvertedPokerPlayer smallBlindPlayer = convHand[0];
            IConvertedPokerPlayer bigBlindPlayer = convHand[1];
            IConvertedPokerPlayer buttonPlayer = convHand[2];

            var action1 = new ConvertedPokerActionWithId(
                smallBlindPlayer[Streets.Flop][0], smallBlindPlayer.Position);
            var action2 = new ConvertedPokerActionWithId(bigBlindPlayer[Streets.Flop][0], bigBlindPlayer.Position);
            var action3 = new ConvertedPokerActionWithId(buttonPlayer[Streets.Flop][0], buttonPlayer.Position);
            var action4 = new ConvertedPokerActionWithId(
                smallBlindPlayer[Streets.Flop][1], smallBlindPlayer.Position);
            var action5 = new ConvertedPokerActionWithId(bigBlindPlayer[Streets.Flop][1], bigBlindPlayer.Position);
            var action6 = new ConvertedPokerActionWithId(buttonPlayer[Streets.Flop][1], buttonPlayer.Position);

            IConvertedPokerRound expectedPreflopSequence = new ConvertedPokerRound()
                .Add(action1)
                .Add(action2)
                .Add(action3)
                .Add(action4)
                .Add(action5)
                .Add(action6);

            Assert.That(expectedPreflopSequence, Is.EqualTo(convHand.Sequences[(int)Streets.Flop]));
        }

        [Test]
        public void ConvertFlop_Player1IsSmallBlind_CalculatesBigBlindsRatio1Correctly()
        {
            RelativeRatioResult result = ConvertPreflopThreePlayersHand();

            Assert.That(result.Player2FlopRound[0].Ratio, Is.EqualTo(result.RelativeRatio2));
        }

        [Test]
        public void ConvertFlop_Player1IsSmallBlind_CalculatesBigBlindsRatio2Correctly()
        {
            RelativeRatioResult result = ConvertPreflopThreePlayersHand();

            Assert.That(result.Player2FlopRound[1].Ratio, Is.EqualTo(result.RelativeRatio5));
        }

        [Test]
        public void ConvertFlop_Player1IsSmallBlind_CalculatesButtonsRatio1Correctly()
        {
            RelativeRatioResult result = ConvertPreflopThreePlayersHand();

            Assert.That(result.Player3FlopRound[0].Ratio, Is.EqualTo(result.RelativeRatio3));
        }

        [Test]
        public void ConvertFlop_Player1IsSmallBlind_CalculatesSmallBlindsRatio1Correctly()
        {
            RelativeRatioResult result = ConvertPreflopThreePlayersHand();

            Assert.That(result.Player1FlopRound[0].Ratio, Is.EqualTo(result.RelativeRatio1));
        }

        [Test]
        public void ConvertFlop_Player1IsSmallBlind_CalculatesSmallBlindsRatio2Correctly()
        {
            RelativeRatioResult result = ConvertPreflopThreePlayersHand();

            Assert.That(result.Player1FlopRound[1].Ratio, Is.EqualTo(result.RelativeRatio4));
        }

        [Test]
        public void ConvertFlop_Player1IsSmallBlind_SetsButtonsSecondActionToFold()
        {
            RelativeRatioResult result = ConvertPreflopThreePlayersHand();

            Assert.That(result.Player3FlopRound[1].What, Is.EqualTo(ActionTypes.F));
        }

        [Test]
        public void ConvertFlop_Player1IsSmallBlind_SetsSmallBlindsPreflopSequence()
        {
            // small blind bet 0.7 
            const string expectedSequence = "7";

            var result = ConvertPreflopThreePlayersHand();
            var smallBlindPreflopSequence = result.ConvertedHand[0].SequenceStrings[(int)Streets.Flop];

            Assert.That(smallBlindPreflopSequence, Is.EqualTo(expectedSequence));
        }

        [Test]
        public void ConvertFlop_Player1IsSmallBlind_SetsBigBlindsPreflopSequence()
        {
            // small blind bet 0.7 - big blind called
            const string expectedSequence = "7C";

            var result = ConvertPreflopThreePlayersHand();
            var bigBlindPreflopSequence = result.ConvertedHand[1].SequenceStrings[(int)Streets.Flop];

            Assert.That(bigBlindPreflopSequence, Is.EqualTo(expectedSequence));
        }

        [Test]
        public void ConvertFlop_Player1IsSmallBlind_SetsButtonsPreflopSequence()
        {
            // small blind bet 0.7 - big blind called (ignored - not special) -> Button raised
            const string expectedSequence = "7R";

            var result = ConvertPreflopThreePlayersHand();
            var buttonPreflopSequence = result.ConvertedHand[2].SequenceStrings[(int)Streets.Flop];

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
            
            const int totalPlayers = 2;
            const int smallBlindPosition = 0;
            const int bigBlindPosition = 1;
            const int buttonPosition = 2;

            var action1 = new AquiredPokerAction(ActionTypes.B, bigBlind);
            const double relativeRatio1 = bigBlind / pot;
            const double pot1 = pot + bigBlind;

            var action2 = new AquiredPokerAction(ActionTypes.C, bigBlind);
            const double relativeRatio2 = bigBlind / pot1;
            const double pot2 = pot1 + bigBlind;

            var action3 = new AquiredPokerAction(ActionTypes.R, bigBlind * 3);
            const double relativeRatio3 = 3;
            const double pot3 = pot2 + (bigBlind * 3);

            var action4 = new AquiredPokerAction(ActionTypes.R, bigBlind * 3 * 2);
            const double relativeRatio4 = 2;
            const double pot4 = pot3 + (bigBlind * 3 * 2);

            var action5 = new AquiredPokerAction(ActionTypes.C, bigBlind * 3 * 2);
            const double relativeRatio5 = (bigBlind * 2 * 3) / pot4;

            var action6 = new AquiredPokerAction(ActionTypes.F, 1.0);

            // Small Blind
            IAquiredPokerPlayer player1 = new AquiredPokerPlayer(
                _stub.Some<long>(), smallBlindPosition, _stub.Out<string>(For.HoleCards))

                 // Preflop
                .AddRound(
                new AquiredPokerRound()
                    .Add(new AquiredPokerAction(ActionTypes.C, 0.5)))

                // Flop
                .AddRound(
                new AquiredPokerRound()
                    .Add(action1)
                    .Add(action4));
            player1.Name = "player1";
            player1.Position = smallBlindPosition;

            // Big Blind
            IAquiredPokerPlayer player2 = new AquiredPokerPlayer(
                _stub.Some<long>(), bigBlindPosition, _stub.Out<string>(For.HoleCards))

                 // Preflop
                .AddRound(
                new AquiredPokerRound()
                    .Add(new AquiredPokerAction(ActionTypes.X, 1.0)))

                // Flop
                .AddRound(
                new AquiredPokerRound()
                    .Add(action2)
                    .Add(action5));
            player2.Name = "player2";
            player2.Position = bigBlindPosition;

            // Button
            IAquiredPokerPlayer player3 = new AquiredPokerPlayer(
                _stub.Some<long>(), buttonPosition, _stub.Out<string>(For.HoleCards))
               
                // Preflop
                .AddRound(
                new AquiredPokerRound()
                    .Add(new AquiredPokerAction(ActionTypes.C, 1.0)))

                // Flop
                .AddRound(
                new AquiredPokerRound()
                    .Add(action3)
                    .Add(action6));

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
                .InitializeWith(aquiredHand, convertedHand, pot, bigBlind)
                .ConvertPreflop();
            
            // Reset Values
            _converter.PotProp = pot;
            _converter.ToCallProp = bigBlind;

            _converter.ConvertFlopTurnAndRiver();

            IConvertedPokerRound player1FlopRound = convertedHand[smallBlindPosition][Streets.Flop];
            IConvertedPokerRound player2FlopRound = convertedHand[bigBlindPosition][Streets.Flop];
            IConvertedPokerRound player3FlopRound = convertedHand[buttonPosition][Streets.Flop];

            return new RelativeRatioResult(
                convertedHand,
                player1FlopRound,
                player2FlopRound,
                player3FlopRound,
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

            public readonly IConvertedPokerRound Player1FlopRound;

            public readonly IConvertedPokerRound Player2FlopRound;

            public readonly IConvertedPokerRound Player3FlopRound;

            public readonly double RelativeRatio1;

            public readonly double RelativeRatio2;

            public readonly double RelativeRatio3;

            public readonly double RelativeRatio4;

            public readonly double RelativeRatio5;

            #endregion

            #region Constructors and Destructors

            public RelativeRatioResult(
                IConvertedPokerHand convertedHand,
                IConvertedPokerRound player1FlopRound,
                IConvertedPokerRound player2FlopRound,
                IConvertedPokerRound player3FlopRound,
                double relativeRatio1,
                double relativeRatio2,
                double relativeRatio3,
                double relativeRatio4,
                double relativeRatio5)
            {
                ConvertedHand = convertedHand;
                Player1FlopRound = player1FlopRound;
                Player2FlopRound = player2FlopRound;
                Player3FlopRound = player3FlopRound;
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