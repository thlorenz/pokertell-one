namespace PokerTell.LiveTracker.Interfaces
{
    using Infrastructure.Interfaces.PokerHand;

    public interface IOverlayBoardViewModel : IBoardViewModel
    {
        double Left { get; set; }

        double Top { get; set; }

        IOverlayBoardViewModel HideBoardAfter(int seconds);
    }
}