namespace PokerTell.LiveTracker.Interfaces
{
    public interface IPlayerStatusViewModel
    {
        bool IsPresent { get; set; }

        IHarringtonMViewModel HarringtonM { get; set; }

        IPlayerStatusViewModel ShowHoleCardsFor(int duration, string holecards);
    }
}