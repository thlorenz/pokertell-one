namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using PokerHand;

    using ViewModels.Overlay;

    public class GameHistoryDesignModel : GameHistoryViewModel
    {
        public GameHistoryDesignModel()
            : base(HandHistoryDesign.Model)
        {
        }
    }
}