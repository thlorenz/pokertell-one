namespace PokerTell.LiveTracker.Interfaces
{
    public interface IPokerRoomInfo
    {
        string Site { get; }

        string TableClass { get; }

        string ProcessName { get; }

        string FileExtension { get; }
    }
}