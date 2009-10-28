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
}
