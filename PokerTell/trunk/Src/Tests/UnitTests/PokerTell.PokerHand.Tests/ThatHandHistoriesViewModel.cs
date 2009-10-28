namespace PokerTell.PokerHand.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Analyzation;

    using Fakes;

    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHand.ViewModels;

    using Services;

    using Tools;
    using Tools.Interfaces;

    using UnitTests.Tools;

    public class ThatHandHistoriesViewModel
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

        #endregion
    }
}