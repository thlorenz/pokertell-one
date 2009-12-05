namespace PokerTell.PokerHand.Tests.Services
{
    using Moq;

    using NUnit.Framework;

    using PokerTell.PokerHand.Services;

    using UnitTests;
    using UnitTests.Tools;

    [TestFixture]
    public class HandHistoriesFilterTests : TestWithLog
    {
        HandHistoriesFilter _historiesFilter;

        StubBuilder _stub;

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();

            _historiesFilter = new HandHistoriesFilter();
        }

        [Test]
        public void BinaryDeserialize_Serialized_RestoresHeroName()
        {
            _historiesFilter.HeroName = "hero";
            Assert.That((object)_historiesFilter.BinaryDeserializedInMemory().HeroName, Is.EqualTo(_historiesFilter.HeroName));
        }

        [Test]
        [Sequential]
        public void BinaryDeserialize_Serialized_RestoresSelectHero(
            [Values(true, false)] bool parameter)
        {
            _historiesFilter.SelectHero = parameter;
            Assert.That(
                (object)_historiesFilter.BinaryDeserializedInMemory().SelectHero, Is.EqualTo(_historiesFilter.SelectHero));
        }

        [Test]
        [Sequential]
        public void BinaryDeserialize_Serialized_RestoresShowAll(
            [Values(true, false)] bool parameter)
        {
            _historiesFilter.ShowAll = parameter;
            Assert.That(
                (object)_historiesFilter.BinaryDeserializedInMemory().ShowAll, Is.EqualTo(_historiesFilter.ShowAll));
        }

        [Test]
        [Sequential]
        public void BinaryDeserialize_Serialized_RestoresShowMoneyInvested(
            [Values(true, false)] bool parameter)
        {
            _historiesFilter.ShowMoneyInvested = parameter;
            Assert.That(
                (object)_historiesFilter.BinaryDeserializedInMemory().ShowMoneyInvested, Is.EqualTo(_historiesFilter.ShowMoneyInvested));
        }

        [Test]
        [Sequential]
        public void BinaryDeserialize_Serialized_RestoresShowPreflopFolds(
            [Values(true, false)] bool parameter)
        {
            _historiesFilter.ShowPreflopFolds = parameter;
            Assert.That(
                (object)_historiesFilter.BinaryDeserializedInMemory().ShowPreflopFolds, Is.EqualTo(_historiesFilter.ShowPreflopFolds));
        }

        [Test]
        [Sequential]
        public void BinaryDeserialize_Serialized_RestoresShowSawFlop(
            [Values(true, false)] bool parameter)
        {
            _historiesFilter.ShowSawFlop = parameter;
            Assert.That(
                (object)_historiesFilter.BinaryDeserializedInMemory().ShowSawFlop, Is.EqualTo(_historiesFilter.ShowSawFlop));
        }

        [Test]
        [Sequential]
        public void BinaryDeserialize_Serialized_RestoresShowSelectedOnly(
            [Values(true, false)] bool parameter)
        {
            _historiesFilter.ShowSelectedOnly = parameter;
            Assert.That(
                (object)_historiesFilter.BinaryDeserializedInMemory().ShowSelectedOnly, Is.EqualTo(_historiesFilter.ShowSelectedOnly));
        }

        [Test]
        public void Set_HeroName_FiresHeroNameChanged()
        {
            bool eventFired = false;
            _historiesFilter.HeroNameChanged += () => eventFired = true;

            _historiesFilter.HeroName = "newName";

            Assert.That(eventFired);
        }

        [Test]
        public void Set_SelectHero_FiresSelectHeroChanged()
        {
            bool eventFired = false;
            _historiesFilter.SelectHeroChanged += () => eventFired = true;

            _historiesFilter.SelectHero = false;

            Assert.That(eventFired);
        }

        [Test]
        public void Set_ShowAll_FiresShowAllChanged()
        {
            bool eventFired = false;
            _historiesFilter.ShowAllChanged += () => eventFired = true;

            _historiesFilter.ShowAll = false;

            Assert.That(eventFired);
        }

        [Test]
        public void Set_ShowMoneyInvested_FiresShowMoneyInvestedChanged()
        {
            bool eventFired = false;
            _historiesFilter.ShowMoneyInvestedChanged += () => eventFired = true;

            _historiesFilter.ShowMoneyInvested = false;

            Assert.That(eventFired);
        }

        [Test]
        public void Set_ShowPreflopFolds_FiresShowPreflopFoldsChanged()
        {
            bool eventFired = false;
            _historiesFilter.ShowPreflopFoldsChanged += () => eventFired = true;

            _historiesFilter.ShowPreflopFolds = false;

            Assert.That(eventFired);
        }

        [Test]
        public void Set_ShowSawFlop_ShowSawFlopChanged()
        {
            bool eventFired = false;
            _historiesFilter.ShowSawFlopChanged += () => eventFired = true;

            _historiesFilter.ShowSawFlop = false;

            Assert.That(eventFired);
        }

        [Test]
        public void Set_ShowSelectedOnly_ShowSelectedOnlyChanged()
        {
            bool eventFired = false;
            _historiesFilter.ShowSelectedOnlyChanged += () => eventFired = true;

            _historiesFilter.ShowSelectedOnly = false;

            Assert.That(eventFired);
        }
    }
}