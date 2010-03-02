namespace PokerTell.LiveTracker.Design.PokerHand
{
    using System;

    [Serializable]
    public class HandHistoryDesignModel : PokerTell.PokerHand.ViewModels.HandHistoryViewModel
    {
        public HandHistoryDesignModel()
        {
            UpdateWith(new DesignHelper().CreateSamplePokerHand(12));
        }
    }

    public static class HandHistoryDesign
    {
        public static HandHistoryDesignModel Model = new HandHistoryDesignModel();
    }
}