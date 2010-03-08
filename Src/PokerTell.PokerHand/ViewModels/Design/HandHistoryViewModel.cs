namespace PokerTell.PokerHand.ViewModels.Design
{
    using System;

    [Serializable]
    public class HandHistoryViewModel : ViewModels.HandHistoryViewModel
    {
        public HandHistoryViewModel()
        {
            UpdateWith(new DesignHelper().CreateSamplePokerHand(12));
        }
    }

    
    public static class HandHistoryDesign
    {
        public static HandHistoryViewModel Model = new HandHistoryViewModel();
    }
}
