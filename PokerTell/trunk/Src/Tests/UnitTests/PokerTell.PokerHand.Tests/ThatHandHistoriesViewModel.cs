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
                .RegisterType<IHandHistoriesViewModel, HandHistoriesViewModel>();

            _viewModel = _container.Resolve<IHandHistoriesViewModel>();
        }
        
        [Test]
        public void ExecutingApplyFilter_HasRegisteredCommands_ExecutesRegisteredCommands()
        {
            bool action1Executed = false;
            bool action2Executed = false;

            var action1 = new Action<IPokerHandCondition>(c => action1Executed = true);
            var action2 = new Action<IPokerHandCondition>(c => action2Executed = true);

             _viewModel.ApplyFilterCompositeAction
                .RegisterAction(action1)
                .RegisterAction(action2);

            _viewModel.ApplyFilterCompositeAction.Invoke(null);

            bool bothCommandsExecuted = action1Executed && action2Executed;

            Assert.That(bothCommandsExecuted, Is.True);
        }

        [Test]
        public void ExecutingApplyFilter_WithPokerHandCondition_PassesConditionToChildCommands()
        {
            const string player = "player1";

            IPokerHandCondition condition = new StubCondition().AppliesTo(player);
            IPokerHandCondition passedCondition = null;

            var action1 = new Action<IPokerHandCondition>(c => passedCondition = c);

            _viewModel.ApplyFilterCompositeAction
                .RegisterAction(action1)
                .Invoke(condition);

            Assert.That(passedCondition, Is.EqualTo(condition));
        }

        [Test]
        public void InitializeWith_EmptyHands_KeepsHandHistoryViewModelsEmpty()
        {
            IList<IConvertedPokerHand> hands = new List<IConvertedPokerHand>();

            _viewModel.InitializeWith(hands);

            Assert.That(_viewModel.HandHistoryViewModels.Count(), Is.EqualTo(0));
        }

        [Test]
        public void InitializeWith_OneHand_AddsOneHandHistoryViewModel()
        {
            var hand = new ConvertedPokerHand();
                
            var hands = new List<IConvertedPokerHand> { hand };

            _viewModel.InitializeWith(hands);

            Assert.That(_viewModel.HandHistoryViewModels.Count(), Is.EqualTo(1));
        }

        [Test]
        public void InitializeWith_TwoHands_AddsTwoHandHistoryViewModel()
        {
            var hand1 = new ConvertedPokerHand();
            var hand2 = new ConvertedPokerHand();

            var hands = new List<IConvertedPokerHand> { hand1, hand2 };

            _viewModel.InitializeWith(hands);

            Assert.That(_viewModel.HandHistoryViewModels.Count(), Is.EqualTo(2));
        }

        [Test]
        public void ApplyFilter_ContainsOneHandHistoryViewModel_InvokesAdjustToConditionActionOnIt()
        {
            bool wasInvoked = false;
            var modelMock = new MockHandHistoryViewModel();
            modelMock.SetAdjustToConditionAction(p => wasInvoked = true);

            var mockConstructor = new Constructor<IHandHistoryViewModel>(() => modelMock);
            var viewModel = new HandHistoriesViewModel(mockConstructor);
            viewModel.InitializeWith(new[] { new ConvertedPokerHand() });

            viewModel.ApplyFilterCompositeAction.Invoke(_stub.Out<IPokerHandCondition>());

            Assert.That(wasInvoked);
        }

        #endregion
    }
}