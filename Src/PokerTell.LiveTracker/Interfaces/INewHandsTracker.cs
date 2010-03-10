namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;

    using Tools.Interfaces;

    public interface INewHandsTracker : IFluentInterface
    {
        INewHandsTracker InitializeWith(IEnumerable<IFileSystemWatcher> fileSystemWatchers);
    }
}