namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using PokerHand;

    using Tools.Validation;

    using ViewModels.Overlay;

    public class GameHistoryDesignModel : GameHistoryViewModel
    {
        public GameHistoryDesignModel()
            : base(HandHistoryDesign.Model, new CollectionValidator())
        {
        }
    }
}