namespace PokerTell.Repository.Interfaces
{
    using Infrastructure.Interfaces.DatabaseSetup;

    public interface IDatabaseImporter
    {
        int BatchSize { get; set; }

        bool IsBusy { get; }

        IDatabaseImporter ImportFrom(PokerStatisticsApplications pokerStatisticsApplication, string databaseName, IDataProvider dataProvider);
    }
}