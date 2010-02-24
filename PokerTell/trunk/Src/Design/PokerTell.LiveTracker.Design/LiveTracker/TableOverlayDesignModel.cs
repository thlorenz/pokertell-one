namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using System.Collections.Generic;

    using PokerHand.ViewModels.Design;

    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.ViewModels.Overlay;

    using Tools.Interfaces;

    using ViewModels;

    public class TableOverlayDesignModel : TableOverlayViewModel
    {
        public TableOverlayDesignModel()
            : base(new OverlayBoardViewModel(new BoardViewModel(), new DispatcherTimerAdapter()))
        {
            OverlaySettings = TableOverlaySettingsDesign.Model;
            PlayerOverlays = new List<IPlayerOverlayViewModel>
                {
                    new PlayerOverlayDesignModel(1, PlayerStatusWith(true, 5, 550, 20), OverlaySettings), 
                    new PlayerOverlayDesignModel(2, PlayerStatusWith(true, 6, 600, 120), OverlaySettings), 
                    new PlayerOverlayDesignModel(3, PlayerStatusWith(true, 7, 550, 220), OverlaySettings), 
                    new PlayerOverlayDesignModel(4, PlayerStatusWith(true, 8, 250, 220), OverlaySettings), 
                    new PlayerOverlayDesignModel(5, PlayerStatusWith(true, 9, 50, 120), OverlaySettings), 
                    new PlayerOverlayDesignModel(6, PlayerStatusWith(true, 10, 250, 20), OverlaySettings), 
                };
        }

        static IPlayerStatusViewModel PlayerStatusWith(bool isPresent, int harringtonM, int left, int top)
        {
            return new PlayerStatusViewModel { IsPresent = isPresent, HarringtonM = new HarringtonMDesignModel(harringtonM, left, top) };
        }
    }

    public static class TableOverlayDesign
    {
        public static readonly TableOverlayViewModel Model = new TableOverlayDesignModel();
    }
}