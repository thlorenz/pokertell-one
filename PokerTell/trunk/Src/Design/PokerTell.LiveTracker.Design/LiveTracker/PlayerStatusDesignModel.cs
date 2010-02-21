namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using Interfaces;

    using ViewModels.Overlay;

    public class PlayerStatusDesignModel : PlayerStatusViewModel
    {
        public PlayerStatusDesignModel(bool isPresent, IHarringtonMViewModel harringtonM)
        {
            IsPresent = isPresent;
            HarringtonM = harringtonM;
        }

    }
}