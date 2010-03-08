namespace PokerTell.LiveTracker.Tests.ViewModels.Overlay
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using Infrastructure.Interfaces;
    using Infrastructure.Services;

    using Interfaces;

    using LiveTracker.ViewModels.Overlay;

    using Machine.Specifications;

    using Moq;

    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class OverlaySettingsAidViewModelSpecs
    {
        protected static Mock<IOverlayBoardViewModel> _boardVM_Mock;

        protected static Mock<IOverlayHoleCardsViewModel> _holeCardsVM_Mock;

        protected static IConstructor<IOverlayHoleCardsViewModel> _holeCardsVM_Make_Stub;

        protected static IOverlaySettingsAidViewModel _sut;

        Establish specContext = () => {
           _boardVM_Mock = new Mock<IOverlayBoardViewModel>();
           _boardVM_Mock.Setup(b => b.InitializeWith(Moq.It.IsAny<IPositionViewModel>())).Returns(_boardVM_Mock.Object);

           _holeCardsVM_Mock = new Mock<IOverlayHoleCardsViewModel>();
           _holeCardsVM_Mock
               .Setup(h => h.InitializeWith(Moq.It.IsAny<IPositionViewModel>()))
               .Returns(_holeCardsVM_Mock.Object);

            _holeCardsVM_Make_Stub = new Constructor<IOverlayHoleCardsViewModel>(() => _holeCardsVM_Mock.Object);

            _sut = new OverlaySettingsAidViewModel(_holeCardsVM_Make_Stub, _boardVM_Mock.Object);
        };

        [Subject(typeof(OverlaySettingsAidViewModel), "InitializeWith")]
        public class when_initialized_with_overlay_settings_with_6_total_seats : OverlaySettingsAidViewModelSpecs
        {
            static Mock<ITableOverlaySettingsViewModel> overlaySettings_Stub;

            static IPositionViewModel[] holeCardsPositions;

            static readonly IPositionViewModel somePosition = new PositionViewModel(1, 1);

            const int seats = 6;
            Establish context = () => {
               overlaySettings_Stub = new Mock<ITableOverlaySettingsViewModel>();
                overlaySettings_Stub.SetupGet(os => os.TotalSeats).Returns(seats);
                holeCardsPositions = new[] { somePosition, somePosition, somePosition, somePosition, somePosition, somePosition };
                overlaySettings_Stub.SetupGet(os => os.HoleCardsPositions).Returns(holeCardsPositions);
            };

            Because of = () => _sut.InitializeWith(overlaySettings_Stub.Object);

            It should_update_the_board_to_show_a_royal_flush = () => _boardVM_Mock.Verify(b => b.UpdateWith("Ts Js Qs Ks As"));

            It should_create_6_holecards_viewmodels = () => _sut.HoleCards.Count().ShouldEqual(seats);

            It should_initialize_the_holecards_viewmodels_for_each_seat_with_holecards_positions_in_the_settings
                = () => _holeCardsVM_Mock.Verify(h => h.InitializeWith(somePosition));

            It should_update_the_holecards_viewmodels_for_each_seat_with_some_holecards
                = () => _holeCardsVM_Mock.Verify(h => h.UpdateWith(Moq.It.Is<string>(s => s.Length == 5)), Times.Exactly(seats));
        }
    }
}