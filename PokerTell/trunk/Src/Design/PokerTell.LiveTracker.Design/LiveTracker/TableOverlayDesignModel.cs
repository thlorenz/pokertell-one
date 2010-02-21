namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using System.Collections.Generic;

    using Interfaces;

    using ViewModels;
    using ViewModels.Overlay;

    public class TableOverlayDesignModel : TableOverlayViewModel
    {
        public TableOverlayDesignModel()
        {
            PlayerOverlays = new List<IPlayerOverlayViewModel>
                {
                    new PlayerOverlayDesignModel(1, PlayerStatusWith(true, 5, 550, 20), 600, 20),
                    new PlayerOverlayDesignModel(2, PlayerStatusWith(true, 6, 650, 120), 700, 120),
                    new PlayerOverlayDesignModel(3, PlayerStatusWith(true, 7, 550, 220), 600, 220),
                    new PlayerOverlayDesignModel(4, PlayerStatusWith(true, 8, 250, 220), 300, 220),
                    new PlayerOverlayDesignModel(5, PlayerStatusWith(true, 9, 50, 120), 100, 120),
                    new PlayerOverlayDesignModel(6, PlayerStatusWith(true, 10, 250, 20), 300, 20),
                };
        }

        static IPlayerStatusViewModel PlayerStatusWith(bool isPresent, int harringtonM, int left, int top)
        {
            return new PlayerStatusViewModel { IsPresent = isPresent, HarringtonM = new HarringtonMDesignModel(harringtonM, left, top) };
        }
    }

    public static class TableOverlayDesign
    {
        public static TableOverlayViewModel Model = new TableOverlayDesignModel();
    }
}