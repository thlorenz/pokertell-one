namespace PokerTell.Plugins.InstantHandHistoryReader.Interfaces
{
    using System.Collections.Generic;

    public interface IHandHistoryReader
    {
        IHandHistoryReader InitializeWith(string processName, string valueThatHandHistoryStartsWith);

        IList<string> FindNewInstantHandHistories();

        bool WasSuccessfullyInitialized { get; }
    }
}