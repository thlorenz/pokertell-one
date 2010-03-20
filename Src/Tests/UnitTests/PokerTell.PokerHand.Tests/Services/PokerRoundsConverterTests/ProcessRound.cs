namespace PokerTell.PokerHand.Tests.Services.PokerRoundsConverterTests
{
    using Fakes;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Microsoft.Practices.Unity;

    using Moq;

    using NUnit.Framework;

    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Aquisition;
    using PokerTell.PokerHand.Services;

    using UnitTests;

    public class ProcessRound : TestWithLog
    {
        #region Constants and Fields

        IUnityContainer _container;

        MockPokerRoundsConverter _converter;

        StubBuilder _stub;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();

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
        [Sequential]
        public void ProcessRound_InvalidActionType_SetsFoundActionToTrue(
            [Values(Streets.PreFlop, Streets.Flop, Streets.Turn, Streets.River)] Streets street)
        {
            IAquiredPokerRound aquiredPokerRoundStub = new AquiredPokerRound()
                .Add(new AquiredPokerAction(ActionTypes.E, _stub.Some<double>()));

            // This will log an error (illegal action) after found action was set to true and not rethrow
            NotLogged(
                () => _converter.InvokeProcessRound(
                          street, 
                          aquiredPokerRoundStub, 
                          _stub.Out<IConvertedPokerPlayer>()));

            Assert.That(_converter.FoundActionProp, Is.True);
        }

        [Test]
        public void ProcessRound_PokerPlayerWithNoRound_DoesNotReThrowNullReferenceException(
            [Values(Streets.PreFlop, Streets.Flop, Streets.Turn, Streets.River)] Streets street)
        {
            var convertedPlayer = new ConvertedPokerPlayer();

            IAquiredPokerRound aquiredRound =
                new AquiredPokerRound()
                    .Add(new AquiredPokerAction(_stub.Valid(For.ActionType, ActionTypes.C), _stub.Some<double>()));
 
            // Logs the exception and goes on
            TestDelegate invokeProcessRound = () =>
                                              NotLogged(() => _converter.InvokeProcessRound(street, aquiredRound, convertedPlayer));

            Assert.DoesNotThrow(invokeProcessRound);
        }

        [Test]
        public void ProcessRound_Preflop_SetsActionSequenceOnConvertedPlayer()
        {
            IAquiredPokerRound aquiredRound = new AquiredPokerRound()
                .Add(
                new AquiredPokerAction(_stub.Valid(For.ActionType, ActionTypes.C), _stub.Out<double>(For.Ratio)));

            const Streets street = Streets.PreFlop;

            var convertedPlayerMock = new Mock<IConvertedPokerPlayer>();
            convertedPlayerMock
                .SetupGet(p => p[street])
                .Returns(_stub.Out<IConvertedPokerRound>());

            _converter.InvokeProcessRound(street, aquiredRound, convertedPlayerMock.Object);
            string sequenceSoFar = _converter.SequenceSoFarProp;

            convertedPlayerMock.Verify(
                player => player.SetActionSequenceString(ref sequenceSoFar, It.IsAny<IConvertedPokerAction>(), street));
        }

        [Test]
        public void ProcessRound_PreflopAquPlayerWithOneRound_AddsActionAndPlayerIdToSequencesForCurrentRound()
        {
            const int playerPosition = 1;
            IConvertedPokerPlayer playerStub = new ConvertedPokerPlayer()
                .Add();
            playerStub.Position = playerPosition;

            IAquiredPokerRound aquiredRound =
                new AquiredPokerRound()
                    .Add(new AquiredPokerAction(_stub.Valid(For.ActionType, ActionTypes.C), _stub.Some<double>()));

            var expectedAction =
                new ConvertedPokerActionWithId(new ConvertedPokerAction(ActionTypes.C, 0), playerPosition);

            _converter.InvokeProcessRound(Streets.PreFlop, aquiredRound, playerStub);

            Assert.That(_converter.SequenceForCurrentRoundProp[0], Is.EqualTo(expectedAction));
        }

        [Test]
        public void
            ProcessRound_PreflopAquPlayerWithOneRoundConPlayerWithNoRound_AddsPreflopRoundAndActionToConvertedPokerPlayer
            ()
        {
            IAquiredPokerRound aquiredRound =
                new AquiredPokerRound()
                    .Add(new AquiredPokerAction(_stub.Valid(For.ActionType, ActionTypes.C), _stub.Some<double>()));
            IConvertedPokerPlayer convertedPlayer = new ConvertedPokerPlayer();

            _converter.InvokeProcessRound(Streets.PreFlop, aquiredRound, convertedPlayer);

            IConvertedPokerAction firstPreflopAction = convertedPlayer[Streets.PreFlop][0];
            Assert.That(firstPreflopAction, Is.EqualTo(aquiredRound[0]));
        }

        [Test]
        public void
            ProcessRound_PreflopAquPlayerWithOneRoundConPlayerWithOneRound_AddsPreflopActionToConvertedPokerPlayerFirstRound
            ()
        {
            const ActionTypes someActionType = ActionTypes.C;
            const double someRatio = 1.0;
            IAquiredPokerRound aquiredRound = new AquiredPokerRound()
                .Add(new AquiredPokerAction(someActionType, someRatio));

            IConvertedPokerPlayer convertedPlayer = new ConvertedPokerPlayer().Add();

            _converter.InvokeProcessRound(Streets.PreFlop, aquiredRound, convertedPlayer);

            IConvertedPokerAction firstPreflopAction = convertedPlayer[Streets.PreFlop][0];

            const int first = 0;
            Assert.That(firstPreflopAction, Is.EqualTo(aquiredRound[first]));
        }

        [Test]
        public void
            ProcessRound_PreflopAquPlayerWithTwoRoundsConPlayerWithOneRoundActionCountOne_AddsSecondPreflopActionAsFirstActionOfConvertedPokerPlayersFirstRound
            ()
        {
            const ActionTypes firstActionType = ActionTypes.C;
            const ActionTypes secondActionType = ActionTypes.F;
            const double someRatio = 1.0;
            IAquiredPokerRound aquiredRound = new AquiredPokerRound()
                .Add(new AquiredPokerAction(firstActionType, someRatio))
                .Add(new AquiredPokerAction(secondActionType, someRatio));

            IConvertedPokerPlayer convertedPlayer = new ConvertedPokerPlayer().Add();

            _converter.ActionCountProp = 1;
            _converter.InvokeProcessRound(Streets.PreFlop, aquiredRound, convertedPlayer);

            IConvertedPokerAction addedPreflopAction = convertedPlayer[Streets.PreFlop][0];

            const int second = 1;
            Assert.That(addedPreflopAction, Is.EqualTo(aquiredRound[second]));
        }

        #endregion
    }
}