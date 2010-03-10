namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Tools.FunctionalCSharp;
    using Tools.WPF;

    public class TableOverlayControllerViewModel
    {
        readonly ITableOverlayViewModel _tableOverlayViewModel;

        public TableOverlayControllerViewModel(ITableOverlayViewModel tableOverlayViewModel)
        {
            _tableOverlayViewModel = tableOverlayViewModel;
        }

        ICommand _updateWithCommand;

        public ICommand UpdateWithCommand
        {
            get
            {
                return _updateWithCommand ?? (_updateWithCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => _tableOverlayViewModel.UpdateWith(GetUpdatedPlayers(), "Ad Kd Td"),
                    });
            }
        }

        static IEnumerable<IConvertedPokerPlayer> GetUpdatedPlayers()
        {
            var playerStubs = AutoWiring_TableOverlay.GetPokerPlayerStubs();
            playerStubs.ForEach(ps => ps.SetupGet(p => p.Holecards).Returns("As Ah"));
            return playerStubs.Map(ps => ps.Object);
        }
    }
}