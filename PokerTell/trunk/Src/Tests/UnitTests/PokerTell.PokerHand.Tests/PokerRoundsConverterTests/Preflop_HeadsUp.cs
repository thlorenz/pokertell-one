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

    public class Preflop_HeadsUp
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
        public void ConvertPreflop_Player1IsSmallBlind_CalculatesSmallBlindsRatio1Correctly()
        {
            var result = ConvertPreflopHeadsUpHand();

            Assert.That(result.Player1FirstRound[0].Ratio, Is.EqualTo(result.RelativeRatio1));
        }

        [Test]
        public void ConvertPreflop_Player1IsSmallBlind_CalculatesSmallBlindsRatio2Correctly()
        {
            var result = ConvertPreflopHeadsUpHand();

            Assert.That(result.Player1FirstRound[1].Ratio, Is.EqualTo(result.RelativeRatio3));
        }

        [Test]
        public void ConvertPreflop_Player1IsSmallBlind_CalculatesBigBlindsRatio1Correctly()
        {
            var result = ConvertPreflopHeadsUpHand();

            Assert.That(result.Player2FirstRound[0].Ratio, Is.EqualTo(result.RelativeRatio2));
        }

        [Test]
        public void ConvertPreflop_Player1IsSmallBlind_CalculatesBigBlindsRatio2Correctly()
        {
            var result = ConvertPreflopHeadsUpHand();

            Assert.That(result.Player2FirstRound[1].Ratio, Is.EqualTo(result.RelativeRatio4));
        }

        [Test]
        public void ConvertPreflop_Player1IsSmallBlind_SetsSmallBlindsPreflopSequence()
        {
            const string expectedSequence = "C";
            
            var result = ConvertPreflopHeadsUpHand();
            var smallBlindPreflopSequence = result.ConvertedHand[0].Sequence[(int)Streets.PreFlop];
            
            Assert.That(smallBlindPreflopSequence, Is.EqualTo(expectedSequence));
        }

        [Test]
        public void ConvertPreflop_Player1IsSmallBlind_SetsBigBlindsPreflopSequence()
        {
            const string expectedSequence = "R";

            var result = ConvertPreflopHeadsUpHand();
            var bigBlindPreflopSequence = result.ConvertedHand[1].Sequence[(int)Streets.PreFlop];

            Assert.That(bigBlindPreflopSequence, Is.EqualTo(expectedSequence));
        }

        [Test]
        public void ConvertPreflop_Player1IsSmallBlind__SetsOverallPreflopSequenceCorrectly()
        {
            RelativeRatioResult result = ConvertPreflopHeadsUpHand();
            IConvertedPokerHand convHand = result.ConvertedHand;
            IConvertedPokerPlayer smallBlindPlayer = convHand[0];
            IConvertedPokerPlayer bigBlindPlayer = convHand[1];

            var action1 = new ConvertedPokerActionWithId(
                smallBlindPlayer[Streets.PreFlop][0], smallBlindPlayer.Position);
            var action2 = new ConvertedPokerActionWithId(bigBlindPlayer[Streets.PreFlop][0], bigBlindPlayer.Position);
            var action3 = new ConvertedPokerActionWithId(
                smallBlindPlayer[Streets.PreFlop][1], smallBlindPlayer.Position);
            var action4 = new ConvertedPokerActionWithId(bigBlindPlayer[Streets.PreFlop][1], bigBlindPlayer.Position);

            IConvertedPokerRound expectedPreflopSequence = new ConvertedPokerRound()
                .Add(action1)
                .Add(action2)
                .Add(action3)
                .Add(action4);

            Assert.That(expectedPreflopSequence, Is.EqualTo(convHand.Sequences[(int)Streets.PreFlop]));
        }

        RelativeRatioResult ConvertPreflopHeadsUpHand()
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

            var action1 = new AquiredPokerAction(ActionTypes.C, smallBlind);
            const double relativeRatio1 = smallBlind / pot;
            const double pot1 = pot + smallBlind;

            var action2 = new AquiredPokerAction(ActionTypes.R, toCall * 2);
            const double relativeRatio2 = 2;
            const double pot2 = pot1 + (toCall * 2);

            var action3 = new AquiredPokerAction(ActionTypes.R, toCall * 2 * 3);
            const double relativeRatio3 = 3;
            const double pot3 = pot2 + (toCall * 2 * 3);

            var action4 = new AquiredPokerAction(ActionTypes.C, toCall * 2 * 3);
            const double relativeRatio4 = (toCall * 2 * 3) / pot3;

            IAquiredPokerPlayer player1 = new AquiredPokerPlayer(
                _stub.Some<long>(), smallBlindPosition, _stub.Out<string>(For.HoleCards))
                .AddRound(
                new AquiredPokerRound()
                    .Add(action1)
                    .Add(action3));
            player1.Name = "player1";
            player1.Position = smallBlindPosition;

            IAquiredPokerPlayer player2 = new AquiredPokerPlayer(
                _stub.Some<long>(), bigBlindPosition, _stub.Out<string>(For.HoleCards))
                .AddRound(
                new AquiredPokerRound()
                    .Add(action2)
                    .Add(action4));
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

            var player1FirstRound = convertedHand[smallBlindPosition][Streets.PreFlop];
            var player2FirstRound = convertedHand[bigBlindPosition][Streets.PreFlop];

            return new RelativeRatioResult(convertedHand, player1FirstRound, player2FirstRound, relativeRatio1, relativeRatio2, relativeRatio3, relativeRatio4);
        }
        
        class RelativeRatioResult
        {
            public readonly double RelativeRatio1;

            public readonly double RelativeRatio2;

            public readonly double RelativeRatio3;

            public readonly double RelativeRatio4;

            public readonly IConvertedPokerRound Player2FirstRound;

            public readonly IConvertedPokerRound Player1FirstRound;

            public readonly IConvertedPokerHand ConvertedHand;

            public RelativeRatioResult(IConvertedPokerHand convertedHand, IConvertedPokerRound player1FirstRound, IConvertedPokerRound player2FirstRound, double relativeRatio1, double relativeRatio2, double relativeRatio3, double relativeRatio4)
            {
                ConvertedHand = convertedHand;
                Player1FirstRound = player1FirstRound;
                Player2FirstRound = player2FirstRound;
                RelativeRatio4 = relativeRatio4;
                RelativeRatio3 = relativeRatio3;
                RelativeRatio2 = relativeRatio2;
                RelativeRatio1 = relativeRatio1;
            }
        }
    }
}