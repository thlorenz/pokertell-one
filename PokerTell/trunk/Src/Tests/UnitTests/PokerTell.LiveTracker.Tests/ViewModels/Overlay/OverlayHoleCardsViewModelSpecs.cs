namespace PokerTell.LiveTracker.Tests.ViewModels.Overlay
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using LiveTracker.ViewModels.Overlay;

    using Machine.Specifications;

    using Moq;

    using Tools.Interfaces;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class OverlayHoleCardsViewModelSpecs
    {
        protected static IOverlayHoleCardsViewModel _sut;
        
        protected static Mock<IHoleCardsViewModel> _holeCardsVM_Mock;

        protected static Mock<IDispatcherTimer> _timer_Stub;

        Establish specContext = () => {
            _holeCardsVM_Mock = new Mock<IHoleCardsViewModel>();
            _timer_Stub = new Mock<IDispatcherTimer>();
            _sut = new OverlayHoleCardsViewModel(_holeCardsVM_Mock.Object, _timer_Stub.Object);
        };

        [Subject(typeof(OverlayHoleCardsViewModel), "HideHoleCardsAfter")]
        public class when_told_to_hide_holecards_after_2_seconds : OverlayHoleCardsViewModelSpecs
        {
            Because of = () => _sut.HideHoleCardsAfter(2);

            It shoud_set_timer_interval_to_2s = () => _timer_Stub.VerifySet(t => t.Interval = TimeSpan.FromSeconds(2));

            It should_start_the_timer = () => _timer_Stub.Verify(t => t.Start());
        }

        [Subject(typeof(OverlayHoleCardsViewModel), "HideHoleCardsAfter")]
        public class when_the_timer_ticks : OverlayHoleCardsViewModelSpecs
        {
            Because of = () => _timer_Stub.Raise(t => t.Tick += null, null, null);

            It should_make_the_holecards_invisible = () => _holeCardsVM_Mock.VerifySet(b => b.Visible = false);

            It should_stop_the_timer = () => _timer_Stub.Verify(t => t.Stop());
        }

        [Subject(typeof(OverlayHoleCardsViewModel), "HideHoleCardsAfter")]
        public class when_it_is_disposed : OverlayHoleCardsViewModelSpecs
        {
            Because of = () => _sut.Dispose();

            It should_stop_the_timer_to_prevent_memory_leaks = () => _timer_Stub.Verify(t => t.Stop());
        }
    }
}