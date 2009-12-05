namespace PokerTell.PokerHand.Tests.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using Fakes;

    using Infrastructure.Interfaces.PokerHand;

    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Services;
    using PokerTell.PokerHand.ViewModels;

    using Tools;
    using Tools.Interfaces;

    using UnitTests;
    using UnitTests.Tools;

    [TestFixture]
    public class HandHistoriesViewModelTests : TestWithLog
    {
        IHandHistoriesViewModel _viewModel;

        IUnityContainer _container;

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _container = new UnityContainer()
                .RegisterConstructor<IHandHistoryViewModel, HandHistoryViewModel>()
                .RegisterType<IItemsPagesManager<IHandHistoryViewModel>, ItemsPagesManager<IHandHistoryViewModel>>()
                .RegisterType<IHandHistoriesFilter, HandHistoriesFilter>()
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
        public void InitializeWith_OneHand_AddsOneHandHistoryViewModels()
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
        public void BinaryDeserialize_Serialized_RestoresPageNumbers()
        {
            var hand1 = new ConvertedPokerHand();
            var hand2 = new ConvertedPokerHand();

            var hands = new List<IConvertedPokerHand> { hand1, hand2 };

            _viewModel
                .InitializeWith(hands);
          
            Assert.That(_viewModel.BinaryDeserializedInMemory().PageNumbers, Is.EqualTo(_viewModel.PageNumbers));
        }

        [Test]
        public void BinaryDeserialize_Serialized_RestoresHandHistoriesFilter()
        {
            IList<IConvertedPokerHand> hands = new List<IConvertedPokerHand>();

            _viewModel.InitializeWith(hands);
            _viewModel.HandHistoriesFilter.HeroName = "someName";
            _viewModel.HandHistoriesFilter.ShowPreflopFolds = true;
            _viewModel.HandHistoriesFilter.ShowSawFlop = true;

            Affirm.That(_viewModel.BinaryDeserializedInMemory().HandHistoriesFilter)
                .IsEqualTo(_viewModel.HandHistoriesFilter);
        }

        [Test]
        public void SelectAllHandHistoriesOnPage_TwoHandsOnPage_SelectsTwoHands()

        {
            var hand1 = new ConvertedPokerHand();
            var hand2 = new ConvertedPokerHand();

            var hands = new List<IConvertedPokerHand> { hand1, hand2 };

            _viewModel
                .InitializeWith(hands)
                .SelectAllHandHistoriesOnPageCommand.Execute(null);

            Assert.That(_viewModel.SelectedHandHistories.Count(), Is.EqualTo(2));
        }

        [Test]
        public void SelectAllShownHandHistories_TwoHandsShownAndTwoOnPage_SelectsTwoHands()
        {
            var hand1 = new ConvertedPokerHand();
            var hand2 = new ConvertedPokerHand();

            var hands = new List<IConvertedPokerHand> { hand1, hand2 };

            _viewModel
                .InitializeWith(hands)
                .SelectAllShownHandHistoriesCommand.Execute(null);

            Assert.That(_viewModel.SelectedHandHistories.Count(), Is.EqualTo(2));
        }

        [Test]
        public void UnselectAllShownHandHistories_TwoSelectedHandsShownAndTwoOnPage_UnselectsTwoHands()
        {
            var hand1 = new ConvertedPokerHand();
            var hand2 = new ConvertedPokerHand();

            var hands = new List<IConvertedPokerHand> { hand1, hand2 };

            _viewModel
                .InitializeWith(hands)
                .SelectAllShownHandHistoriesCommand.Execute(null);

            _viewModel
                .UnselectAllShownHandHistoriesCommand.Execute(null);

            Assert.That(_viewModel.SelectedHandHistories.Count(), Is.EqualTo(0));
        }

        [Test]
        public void SelectAllHandHistoriesOnPage_TwoHandsShownButOnlyOneHandOnPage_SelectsOneHand()
        {
            var hand1 = new ConvertedPokerHand();
            var hand2 = new ConvertedPokerHand();

            var hands = new List<IConvertedPokerHand> { hand1, hand2 };

            _viewModel
                .InitializeWith(hands)
                .HandHistoriesOnPage.RemoveAt(0);
            _viewModel
                .SelectAllHandHistoriesOnPageCommand.Execute(null);

            Assert.That(_viewModel.SelectedHandHistories.Count(), Is.EqualTo(1));
        }

        [Test]
        public void UnselectAllHandHistoriesOnPage_TwoSelectedHandsShownButOnlyOneHandOnPage_UnselectsOneHand()
        {
            var hand1 = new ConvertedPokerHand();
            var hand2 = new ConvertedPokerHand();

            var hands = new List<IConvertedPokerHand> { hand1, hand2 };

            _viewModel
                .InitializeWith(hands)
                .SelectAllHandHistoriesOnPageCommand.Execute(null);
            _viewModel
                .HandHistoriesOnPage.RemoveAt(0);

            _viewModel
                .UnselectAllHandHistoriesOnPageCommand.Execute(null);

            Assert.That(_viewModel.SelectedHandHistories.Count(), Is.EqualTo(1));
        }

        [Test]
        public void SelectAllShownHandHistories_TwoHandsShownButOnlyOneHandOnPage_SelectsTwoHands()
        {
            var hand1 = new ConvertedPokerHand();
            var hand2 = new ConvertedPokerHand();

            var hands = new List<IConvertedPokerHand> { hand1, hand2 };

            _viewModel
                .InitializeWith(hands)
                .HandHistoriesOnPage.RemoveAt(0);
            _viewModel
                .SelectAllShownHandHistoriesCommand.Execute(null);

            Assert.That(_viewModel.SelectedHandHistories.Count(), Is.EqualTo(2));
        }

        [Test]
        public void UnselectAllShownHandHistories_TwoSelectedHandsShownButOnlyOneHandOnPage_UnselectsTwoHands()
        {
            var hand1 = new ConvertedPokerHand();
            var hand2 = new ConvertedPokerHand();

            var hands = new List<IConvertedPokerHand> { hand1, hand2 };

            _viewModel
                .InitializeWith(hands)
                .SelectAllShownHandHistoriesCommand.Execute(null);
            _viewModel
                .HandHistoriesOnPage.RemoveAt(0);

            _viewModel
                .UnselectAllShownHandHistoriesCommand.Execute(null);

            Assert.That(_viewModel.SelectedHandHistories.Count(), Is.EqualTo(0));
        }

        [Test]
        public void SelectAllShownHandHistories_TwoHandsTotalButOnlyOneHandShown_SelectsOneHand()
        {
            var hand1 = new ConvertedPokerHand { Id = 1 };
            var hand2 = new ConvertedPokerHand { Id = 2 };

            var hands = new List<IConvertedPokerHand> { hand1, hand2 };

            var filterCondition = new StubConditionWithHandId { HandIdToMatch = hand1.Id };

            _viewModel
                .InitializeWith(hands)
                .ApplyFilter(filterCondition)
                .SelectAllShownHandHistoriesCommand.Execute(null);

            Assert.That(_viewModel.SelectedHandHistories.Count(), Is.EqualTo(1));
        }

        [Test]
        public void UnselectAllShownHandHistories_TwoSelectedHandsTotalButOnlyOneHandShown_UnselectsOneHand()
        {
            var hand1 = new ConvertedPokerHand { Id = 1 };
            var hand2 = new ConvertedPokerHand { Id = 2 };

            var hands = new List<IConvertedPokerHand> { hand1, hand2 };
          
            var filterCondition = new StubConditionWithHandId { HandIdToMatch = hand1.Id };

            _viewModel
                .InitializeWith(hands)
                .SelectAllShownHandHistoriesCommand.Execute(null);
                
            _viewModel
                .ApplyFilter(filterCondition)
                .UnselectAllShownHandHistoriesCommand.Execute(null);
               
            Assert.That(_viewModel.SelectedHandHistories.Count(), Is.EqualTo(1));
        }

        #endregion
    }
}