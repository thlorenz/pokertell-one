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

    public class PostFlop_HeadsUp
    {
        IUnityContainer _container;

        Constructor<IConvertedPokerPlayer> _convertedPlayerMake;

        MockPokerRoundsConverter _converter;

        StubBuilder _stub;

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
        public void ConvertFlop_Player1IsSmallBlind_CalculatesSmallBlindsRatio1Correctly()
        {
            var result = ConvertPostFlopHeadsUpHand();

            Assert.That(result.Player1FlopRound[0].Ratio, Is.EqualTo(result.RelativeRatio2));
        }

        [Test]
        public void ConvertFlop_Player1IsSmallBlind_CalculatesSmallBlindsRatio2Correctly()
        {
            var result = ConvertPostFlopHeadsUpHand();

            Assert.That(result.Player1FlopRound[1].Ratio, Is.EqualTo(result.RelativeRatio4));
        }

        [Test]
        public void ConvertFlop_Player1IsSmallBlind_CalculatesBigBlindsRatio1Correctly()
        {
            var result = ConvertPostFlopHeadsUpHand();

            Assert.That(result.Player2FlopRound[0].Ratio, Is.EqualTo(result.RelativeRatio1));
        }

        [Test]
        public void ConvertFlop_Player1IsSmallBlind_CalculatesBigBlindsRatio2Correctly()
        {
            var result = ConvertPostFlopHeadsUpHand();

            Assert.That(result.Player2FlopRound[1].Ratio, Is.EqualTo(result.RelativeRatio3));
        }

        [Test]
        public void ConvertFlop_Player1IsSmallBlind__SetsOverallFlopSequenceCorrectly()
        {
            RelativeRatioResult result = ConvertPostFlopHeadsUpHand();
            IConvertedPokerHand convHand = result.ConvertedHand;
            IConvertedPokerPlayer smallBlindPlayer = convHand[0];
            IConvertedPokerPlayer bigBlindPlayer = convHand[1];
            
            var action1 = new ConvertedPokerActionWithId(bigBlindPlayer[Streets.Flop][0], bigBlindPlayer.Position);
            var action2 = new ConvertedPokerActionWithId(
                smallBlindPlayer[Streets.Flop][0], smallBlindPlayer.Position);
            var action3 = new ConvertedPokerActionWithId(bigBlindPlayer[Streets.Flop][1], bigBlindPlayer.Position);
            var action4 = new ConvertedPokerActionWithId(
                smallBlindPlayer[Streets.Flop][1], smallBlindPlayer.Position);

            IConvertedPokerRound expectedPreflopSequence = new ConvertedPokerRound()
                .Add(action1)
                .Add(action2)
                .Add(action3)
                .Add(action4);

            Assert.That(expectedPreflopSequence, Is.EqualTo(convHand.Sequences[(int)Streets.Flop]));
        }

        [Test]
        public void ConvertFlop_Player1IsSmallBlind_SetsSmallBlindsFlopSequence()
        {
            var result = ConvertPostFlopHeadsUpHand();

            var smallBlindFlopSequence = result.ConvertedHand[0].Sequence[(int)Streets.Flop];
            
            // big blind bet 0.5 -> small blind raised
            const string expectedSequence = "5R"; 

            Assert.That(smallBlindFlopSequence, Is.EqualTo(expectedSequence));
        }

        [Test]
        public void ConvertFlop_Player1IsSmallBlind_SetsBigBlindsFlopSequence()
        {
            var result = ConvertPostFlopHeadsUpHand();
            var bigBlindFlopSequence = result.ConvertedHand[1].Sequence[(int)Streets.Flop];

            // big blind bet 0.5 -> action set to "5" -> contains number -> no further actions added
            const string expectedSequence = "5"; 

            Assert.That(bigBlindFlopSequence, Is.EqualTo(expectedSequence));
        }

        RelativeRatioResult ConvertPostFlopHeadsUpHand()
        {
            _stub
                .Value(For.HoleCards).Is(string.Empty);

            const double smallBlind = 1.0;
            const double bigBlind = 2.0;
            const double pot = smallBlind + smallBlind + bigBlind;
            const double toCall = bigBlind;
            const int totalPlayers = 2;
            const int smallBlindPosition = 0;
            const int bigBlindPosition = 1;

            // Headsup the BigBlind acts first PostFlop and small Blind acts as Button
            var action1 = new AquiredPokerAction(ActionTypes.B, bigBlind);
            const double relativeRatio1 = bigBlind / pot;
            const double pot1 = pot + bigBlind;

            var action2 = new AquiredPokerAction(ActionTypes.R, toCall * 2);
            const double relativeRatio2 = 2;
            const double pot2 = pot1 + (toCall * 2);

            var action3 = new AquiredPokerAction(ActionTypes.R, toCall * 2 * 3);
            const double relativeRatio3 = 3;
            const double pot3 = pot2 + (toCall * 2 * 3);

            var action4 = new AquiredPokerAction(ActionTypes.C, toCall * 2 * 3);
            const double relativeRatio4 = (toCall * 2 * 3) / pot3;

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
                    .Add(action2)
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
                    .Add(action1)
                    .Add(action3));
            player2.Name = "player2";
            player2.Position = bigBlindPosition;

            IAquiredPokerHand aquiredHand =
                new AquiredPokerHand(
                    _stub.Valid(For.Site, "site"),
                    _stub.Out<ulong>(For.GameId),
                    _stub.Out<DateTime>(For.TimeStamp),
                    smallBlind,
                    bigBlind,
                    totalPlayers)
                    .AddPlayer(player1)
                    .AddPlayer(player2);

            IConvertedPokerHand convertedHand =
                new ConvertedPokerHand(aquiredHand)
                    .InitializeWith(aquiredHand)
                    .AddPlayersFrom(aquiredHand, pot, _convertedPlayerMake);

            _converter
                .InitializeWith(aquiredHand, convertedHand, pot, toCall)
                .ConvertPreflop();
           
            // Reset Values
            _converter.PotProp = pot;
            _converter.ToCallProp = toCall;

            _converter.ConvertFlopTurnAndRiver();

            var player1FlopRound = convertedHand[smallBlindPosition][Streets.Flop];
            var player2FlopRound = convertedHand[bigBlindPosition][Streets.Flop];

            return new RelativeRatioResult(convertedHand, player1FlopRound, player2FlopRound, relativeRatio1, relativeRatio2, relativeRatio3, relativeRatio4);
        }

        class RelativeRatioResult
        {
            public readonly double RelativeRatio1;

            public readonly double RelativeRatio2;

            public readonly double RelativeRatio3;

            public readonly double RelativeRatio4;

            public readonly IConvertedPokerRound Player2FlopRound;

            public readonly IConvertedPokerRound Player1FlopRound;

            public readonly IConvertedPokerHand ConvertedHand;

            public RelativeRatioResult(IConvertedPokerHand convertedHand, IConvertedPokerRound player1FlopRound, IConvertedPokerRound player2FlopRound, double relativeRatio1, double relativeRatio2, double relativeRatio3, double relativeRatio4)
            {
                ConvertedHand = convertedHand;
                Player1FlopRound = player1FlopRound;
                Player2FlopRound = player2FlopRound;
                RelativeRatio4 = relativeRatio4;
                RelativeRatio3 = relativeRatio3;
                RelativeRatio2 = relativeRatio2;
                RelativeRatio1 = relativeRatio1;
            }
        }
    }
}