namespace PokerTell.PokerHand.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Analyzation;

    using Infrastructure.Services;

    using Microsoft.Practices.Unity;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHand.Tests.Fakes;
    using PokerTell.PokerHand.ViewModels;

    using Tools;
    using Tools.Interfaces;

    using Views;

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
                .RegisterType<IHandHistoriesViewModel, HandHistoriesViewModel>();

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

        #endregion
    }
}