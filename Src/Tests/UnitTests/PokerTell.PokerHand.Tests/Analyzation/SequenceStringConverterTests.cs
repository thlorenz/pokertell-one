namespace PokerTell.PokerHand.Tests.Analyzation
{
    using System.Linq;

    using Infrastructure;
    using Infrastructure.Enumerations.PokerHand;

    using NUnit.Framework;

    using PokerTell.PokerHand.Analyzation;

    using UnitTests;
    using UnitTests.Tools;

    public class SequenceStringConverterTests : TestWithLog
    {
        SequenceStringConverter _sut;

        [SetUp]
        public void _Init()
        {
            _sut = new SequenceStringConverter();
        }

        [Test]
        public void Constructor_UsingApplicationPropertiesBetSizeKeys_InitializesStandardizedBetSizes()
        {
            _sut.StandardizedBetSizes.Count.ShouldBeEqualTo(ApplicationProperties.BetSizeKeys.Length);
            _sut.StandardizedBetSizes.First().ShouldBeEqualTo((int)(ApplicationProperties.BetSizeKeys.First() * 10));
            _sut.StandardizedBetSizes.Last().ShouldBeEqualTo((int)(ApplicationProperties.BetSizeKeys.Last() * 10));
        }

        [Test]
        public void Convert_NullString_ActionSequenceIsNonStandard()
        {
            _sut.Convert(null);
            _sut.ActionSequence.ShouldBeEqualTo(ActionSequences.NonStandard);
        }

        [Test]
        public void Convert_EmptyString_ActionSequenceIsNonStandard()
        {
            _sut.Convert(string.Empty);
            _sut.ActionSequence.ShouldBeEqualTo(ActionSequences.NonStandard);
        }

        [Test]
        public void Convert_EmptyString_BetSizeIndexIsZero()
        {
            _sut.Convert(string.Empty);
            _sut.BetSizeIndex.ShouldBeEqualTo(0);
        }

        [Test]
        public void Convert_X_ActionSequenceIsHeroX()
        {
            _sut.Convert("X");
            _sut.ActionSequence.ShouldBeEqualTo(ActionSequences.HeroX);
        }

        [Test]
        public void Convert_X_BetSizeIndexIsZero()
        {
            _sut.Convert("X");
            _sut.BetSizeIndex.ShouldBeEqualTo(0);
        }

        [Test]
        public void Convert_F_ActionSequenceIsHeroF()
        {
            _sut.Convert("F");
            _sut.ActionSequence.ShouldBeEqualTo(ActionSequences.HeroF);
        }

        [Test]
        public void Convert_C_ActionSequenceIsHeroC()
        {
            _sut.Convert("C");
            _sut.ActionSequence.ShouldBeEqualTo(ActionSequences.HeroC);
        }

        [Test]
        public void Convert_R_ActionSequenceIsHeroR()
        {
            _sut.Convert("R");
            _sut.ActionSequence.ShouldBeEqualTo(ActionSequences.HeroR);
        }

        [Test]
        public void Convert_RF_ActionSequenceIsOppRHeroF()
        {
            _sut.Convert("RF");
            _sut.ActionSequence.ShouldBeEqualTo(ActionSequences.OppRHeroF);
        }

        [Test]
        public void Convert_RC_ActionSequenceIsOppRHeroC()
        {
            _sut.Convert("RC");
            _sut.ActionSequence.ShouldBeEqualTo(ActionSequences.OppRHeroC);
        }

        [Test]
        public void Convert_RR_ActionSequenceIsOppRHeroR()
        {
            _sut.Convert("RR");
            _sut.ActionSequence.ShouldBeEqualTo(ActionSequences.OppRHeroR);
        }

        [Test]
        public void Convert_9_ActionSequenceIsHeroB()
        {
            _sut.Convert("9");
            _sut.ActionSequence.ShouldBeEqualTo(ActionSequences.HeroB);
        }

        [Test]
        public void Convert_9_BetSizeIndexIsIndexOf9()
        {
            _sut.Convert("9");

            _sut.BetSizeIndex.ShouldBeEqualTo(_sut.StandardizedBetSizes.IndexOf(9));
        }

        [Test]
        public void Convert_15_BetSizeIndexIsIndexOf15()
        {
            _sut.Convert("15");

            _sut.BetSizeIndex.ShouldBeEqualTo(_sut.StandardizedBetSizes.IndexOf(15));
        }

        [Test]
        public void Convert_9F_ActionSequenceIsOppBHeroF()
        {
            _sut.Convert("9F");
            _sut.ActionSequence.ShouldBeEqualTo(ActionSequences.OppBHeroF);
        }

        [Test]
        public void Convert_9F_BetSizeIndexIsIndexOf9()
        {
            _sut.Convert("9F");

            _sut.BetSizeIndex.ShouldBeEqualTo(_sut.StandardizedBetSizes.IndexOf(9));
        }

        [Test]
        public void Convert_15F_BetSizeIndexIsIndexOf15()
        {
            _sut.Convert("15F");

            _sut.BetSizeIndex.ShouldBeEqualTo(_sut.StandardizedBetSizes.IndexOf(15));
        }

        [Test]
        public void Convert_C9F_ActionSequenceIsNonStandard()
        {
            _sut.Convert("C9F");
            _sut.ActionSequence.ShouldBeEqualTo(ActionSequences.NonStandard);
        }

        [Test]
        public void Convert_5C_ActionSequenceIsOppBHeroC()
        {
            _sut.Convert("5C");
            _sut.ActionSequence.ShouldBeEqualTo(ActionSequences.OppBHeroC);
        }

        [Test]
        public void Convert_5C_BetSizeIndexIsIndexOf5()
        {
            _sut.Convert("5C");

            _sut.BetSizeIndex.ShouldBeEqualTo(_sut.StandardizedBetSizes.IndexOf(5));
        }

        [Test]
        public void Convert_15C_BetSizeIndexIsIndexOf15()
        {
            _sut.Convert("15C");

            _sut.BetSizeIndex.ShouldBeEqualTo(_sut.StandardizedBetSizes.IndexOf(15));
        }

        [Test]
        public void Convert_2R_ActionSequenceIsOppBHeroR()
        {
            _sut.Convert("2R");
            _sut.ActionSequence.ShouldBeEqualTo(ActionSequences.OppBHeroR);
        }

        [Test]
        public void Convert_2R_BetSizeIndexIsIndexOf2()
        {
            _sut.Convert("2R");

            _sut.BetSizeIndex.ShouldBeEqualTo(_sut.StandardizedBetSizes.IndexOf(2));
        }

        [Test]
        public void Convert_15R_BetSizeIndexIsIndexOf15()
        {
            _sut.Convert("15R");

            _sut.BetSizeIndex.ShouldBeEqualTo(_sut.StandardizedBetSizes.IndexOf(15));
        }

        [Test]
        public void Convert_X9F_ActionSequenceIsHeroXOppBHeroF()
        {
            _sut.Convert("X9F");
            _sut.ActionSequence.ShouldBeEqualTo(ActionSequences.HeroXOppBHeroF);
        }

        [Test]
        public void Convert_X9F_BetSizeIndexIsIndexOf9()
        {
            _sut.Convert("X9F");

            _sut.BetSizeIndex.ShouldBeEqualTo(_sut.StandardizedBetSizes.IndexOf(9));
        }

        [Test]
        public void Convert_X15F_BetSizeIndexIsIndexOf15()
        {
            _sut.Convert("X15F");

            _sut.BetSizeIndex.ShouldBeEqualTo(_sut.StandardizedBetSizes.IndexOf(15));
        }

        [Test]
        public void Convert_CX9F_ActionSequenceIsNonStandard()
        {
            _sut.Convert("CX9F");
            _sut.ActionSequence.ShouldBeEqualTo(ActionSequences.NonStandard);
        }

        [Test]
        public void Convert_X5C_ActionSequenceIsHeroXOppBHeroC()
        {
            _sut.Convert("X5C");
            _sut.ActionSequence.ShouldBeEqualTo(ActionSequences.HeroXOppBHeroC);
        }

        [Test]
        public void Convert_X5C_BetSizeIndexIsIndexOf5()
        {
            _sut.Convert("X5C");

            _sut.BetSizeIndex.ShouldBeEqualTo(_sut.StandardizedBetSizes.IndexOf(5));
        }

        [Test]
        public void Convert_X15C_BetSizeIndexIsIndexOf15()
        {
            _sut.Convert("X15C");

            _sut.BetSizeIndex.ShouldBeEqualTo(_sut.StandardizedBetSizes.IndexOf(15));
        }

        [Test]
        public void Convert_X2R_ActionSequenceIsHeroXOppBHeroR()
        {
            _sut.Convert("X2R");
            _sut.ActionSequence.ShouldBeEqualTo(ActionSequences.HeroXOppBHeroR);
        }

        [Test]
        public void Convert_X2R_BetSizeIndexIsIndexOf2()
        {
            _sut.Convert("X2R");

            _sut.BetSizeIndex.ShouldBeEqualTo(_sut.StandardizedBetSizes.IndexOf(2));
        }

        [Test]
        public void Convert_X15R_BetSizeIndexIsIndexOf15()
        {
            _sut.Convert("X15R");

            _sut.BetSizeIndex.ShouldBeEqualTo(_sut.StandardizedBetSizes.IndexOf(15));
        }
    }
}