namespace PokerTell.Repository.Interfaces
{
    using System;

    using Infrastructure.Interfaces.DatabaseSetup;

    public interface IDatabaseImporter
    {
        int BatchSize { get; set; }

        bool IsBusy { get; }

        IDatabaseImporter ImportFrom(PokerStatisticsApplications pokerStatisticsApplication, string databaseName, IDataProvider dataProvider);

        event Action<bool> IsBusyChanged;
    }
}