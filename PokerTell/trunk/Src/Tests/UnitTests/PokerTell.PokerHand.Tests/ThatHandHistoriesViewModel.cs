namespace PokerTell.PokerHand.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Analyzation;

    using Conditions;

    using Fakes;

    using Microsoft.Practices.Unity;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHand.ViewModels;

    using Tools;
    using Tools.Interfaces;

    using UnitTests.Tools;

    public class ThatHandHistoriesViewModel
    {
        IHandHistoriesViewModel _viewModel;

        StubBuilder _stub;

        IUnityContainer _container;

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();

            _container = new UnityContainer()
                .RegisterConstructor<IHandHistoryViewModel, HandHistoryViewModel>()
                .RegisterType<IItemsPagesManager<IHandHistoryViewModel>, ItemsPagesManager<IHandHistoryViewModel>>()
                .RegisterType<IHandHistoriesViewModel, FakeHandHistoriesViewModel>();

            _viewModel = _container.Resolve<IHandHistoriesViewModel>();
        }
        
        [Test]
        public void InitializeWith_EmptyHands_KeepsHandHistoryViewModelsEmpty()
        {
            IList<IConvertedPokerHand> hands = new List<IConvertedPokerHand>();

            _viewModel.InitializeWith(hands);

            Assert.That(_viewModel.HandHistoriesOnPage.Count(), Is.EqualTo(0));
        }

        [Test]
        public void InitializeWith_OneHand_AddsOneHandHistoryViewModel()
        {
            var hand = new ConvertedPokerHand();
                
            var hands = new List<IConvertedPokerHand> { hand };

            _viewModel.InitializeWith(hands);

            Assert.That(_viewModel.HandHistoriesOnPage.Count(), Is.EqualTo(1));
        }

        [Test]
        public void InitializeWith_TwoHands_AddsTwoHandHistoryViewModels()
        {
            var hand1 = new ConvertedPokerHand();
            var hand2 = new ConvertedPokerHand();

            var hands = new List<IConvertedPokerHand> { hand1, hand2 };

            _viewModel.InitializeWith(hands);

            Assert.That(_viewModel.HandHistoriesOnPage.Count(), Is.EqualTo(2));
        }

        [Test]
        [Sequential]
        public void BinaryDeserialize_Serialized_RestoresShowSelectOption(
            [Values(true, false)] bool parameter)
        {
            ((FakeHandHistoriesViewModel)_viewModel).InterceptOnSetMethods = true;
            _viewModel.InitializeWith(new List<IConvertedPokerHand>());

            _viewModel.ShowSelectOption = parameter;
            Assert.That(
                _viewModel.BinaryDeserializedInMemory().ShowSelectOption, Is.EqualTo(_viewModel.ShowSelectOption));
        }

        [Test]
        [Sequential]
        public void BinaryDeserialize_Serialized_RestoresShowSelectedOnly(
            [Values(true, false)] bool parameter)
        {
            ((FakeHandHistoriesViewModel)_viewModel).InterceptOnSetMethods = true;
            _viewModel.InitializeWith(new List<IConvertedPokerHand>());

            _viewModel.ShowSelectedOnly = parameter;
            Assert.That(
                _viewModel.BinaryDeserializedInMemory().ShowSelectedOnly, Is.EqualTo(_viewModel.ShowSelectedOnly));
        }

        [Test]
        [Sequential]
        public void BinaryDeserialize_Serialized_RestoresShowPreflopFolds(
            [Values(true, false)] bool parameter)
        {
            ((FakeHandHistoriesViewModel)_viewModel).InterceptOnSetMethods = true;
            _viewModel.InitializeWith(new List<IConvertedPokerHand>());

            _viewModel.ShowPreflopFolds = parameter;
            Assert.That(
                _viewModel.BinaryDeserializedInMemory().ShowPreflopFolds, Is.EqualTo(_viewModel.ShowPreflopFolds));
        }

        [Test]
        public void BinaryDeserialize_Serialized_RestoresItemsHandHistoriesOnPage()
        {
            var hand1 = new ConvertedPokerHand();
            var hand2 = new ConvertedPokerHand();
            var hands = new List<IConvertedPokerHand> { hand1, hand2 };

            _viewModel.InitializeWith(hands);
           
            Assert.That(_viewModel.BinaryDeserializedInMemory().HandHistoriesOnPage, 
                Is.EqualTo(_viewModel.HandHistoriesOnPage));
        }

        [Test]
        public void BinaryDeserialize_Serialized_RestoresSelectedHandHistories()
        {
            var hand1 = new ConvertedPokerHand();
            var hand2 = new ConvertedPokerHand();
           
            var hands = new List<IConvertedPokerHand> { hand1, hand2 };

            _viewModel.InitializeWith(hands);
           
            Assert.That(_viewModel.BinaryDeserializedInMemory().SelectedHandHistories,
                Is.EqualTo(_viewModel.SelectedHandHistories));
        }

        [Test]
        public void BinaryDeserialize_Serialized_RestoresHeroName()
        {
            _viewModel.InitializeWith(new List<IConvertedPokerHand>());
            _viewModel.HeroName = "hero";
            Assert.That(_viewModel.BinaryDeserializedInMemory().HeroName, Is.EqualTo(_viewModel.HeroName));
        }

        #endregion
    }
}