namespace PokerTell.Repository.Interfaces
{
    using System;

    using Database;

    public interface IHandHistoriesDirectoryImporter
    {
        HandHistoriesDirectoryImporter ImportDirectory(string directoryPath);

        IHandHistoriesDirectoryImporter InitializeWith(
            Action<int> reportProgress,
            Action<int> reportCompletion);
    }
}