namespace PokerTell.LiveTracker.Interfaces
{
    using Tools.Interfaces;

    public interface IPokerRoomInfo : IFluentInterface
    {
        string Site { get; }

        string TableClass { get; }

        string ProcessName { get; }

        string FileExtension { get; }
    }
}