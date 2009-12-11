namespace PokerTell.PokerHand.Tests.Analyzation
{
    using System;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using NUnit.Framework;

    using PokerTell.PokerHand.Analyzation;

    using UnitTests.Tools;

    public class AnalyzablePokerPlayerTests
    {
        AnalyzablePokerPlayerMock _sut;

        const string sequencePreFlop = "[1]R3.0,[0]C0.4";
        const string sequenceFlop = "[0]X,[1]B0.8,[0]C0.4";
        const string sequenceTurn = "[0]X,[1]B0.4,[0]C0.3,[1]A";
        const string sequenceRiver = null;

        [SetUp]
        public void _Init()
        {
            _sut = new AnalyzablePokerPlayerMock(1, 
                                             1, 
                                             string.Empty, 
                                             0, 
                                             0, 
                                             StrategicPositions.BB, 
                                             false, 
                                             false, 
                                             false, 
                                             false, 
                                             ActionSequences.HeroC, 
                                             ActionSequences.HeroC, 
                                             ActionSequences.HeroC, 
                                             ActionSequences.HeroC, 
                                             0,
                                             0,
                                             0,
                                             1.0, 
                                             0, 
                                             DateTime.MinValue, 
                                             2, 
                                             2,
                                             sequencePreFlop, 
                                             sequenceFlop, 
                                             sequenceTurn, 
                                             sequenceRiver);
        }

        [Test]
        public void NHibernateConstructor_Never_InitializesSequences()
        {
            _sut.SequencesAreInitialized.IsFalse();
        }
        
        [Test]
        public void AccessSequences_FirstTime_InitializesThemWithSequenceStrings()
        {
            var access = _sut.Sequences[0];
            _sut.SequencesAreInitialized.IsTrue();
        }

        [Test]
        public void AccessSequences_FirstTime_ReturnsCorrectSequenceValues()
        {
            _sut.Sequences[(int)Streets.PreFlop].Actions.HasCount(2);
            _sut.Sequences[(int)Streets.Flop].Actions.HasCount(3);
            _sut.Sequences[(int)Streets.Turn].Actions.HasCount(4);
            _sut.Sequences[(int)Streets.River].IsNull();
        }
    }

    class AnalyzablePokerPlayerMock : AnalyzablePokerPlayer
    {
        public AnalyzablePokerPlayerMock(long id, int handId, string holecards, int mBefore, int position, StrategicPositions strategicPosition, bool? inPositionPreFlop, bool? inPositionFlop, bool? inPositionTurn, bool? inPositionRiver, ActionSequences actionSequencePreFlop, ActionSequences actionSequenceFlop, ActionSequences actionSequenceTurn, ActionSequences actionSequenceRiver, int betSizeIndexFlop, int betSizeIndexTurn, int betSizeIndexRiver, double bb, double ante, DateTime timeStamp, int totalPlayers, int playersInFlop, string sequencePreFlop, string sequenceFlop, string sequenceTurn, string sequenceRiver)
            : base(
                id,
                handId,
                holecards,
                mBefore,
                position,
                strategicPosition,
                inPositionPreFlop,
                inPositionFlop,
                inPositionTurn,
                inPositionRiver,
                actionSequencePreFlop,
                actionSequenceFlop,
                actionSequenceTurn,
                actionSequenceRiver,
                betSizeIndexFlop,
                betSizeIndexTurn,
                betSizeIndexRiver,
                bb,
                ante,
                timeStamp,
                totalPlayers,
                playersInFlop,
                sequencePreFlop,
                sequenceFlop,
                sequenceTurn,
                sequenceRiver)
        {
        }

        public bool SequencesAreInitialized
        {
            get { return _sequences != null; }
        }
    }
}