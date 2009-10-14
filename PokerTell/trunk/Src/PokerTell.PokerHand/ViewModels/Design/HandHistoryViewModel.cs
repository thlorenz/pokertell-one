namespace PokerTell.PokerHand.ViewModels.Design
{
    public class HandHistoryViewModel : ViewModels.HandHistoryViewModel
    {
        public HandHistoryViewModel()
        {
            UpdateWith(new DesignHelper().CreateSamplePokerHand(12));
        }
    }
}
