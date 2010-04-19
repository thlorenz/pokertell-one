namespace PokerTell.Repository.Interfaces
{
    using Infrastructure.Interfaces.DatabaseSetup;

    public interface IDatabaseImporter
    {
        bool IsBusy { get; }

        IDatabaseImporter ImportFrom(PokerStatisticsApplications pokerStatisticsApplication, string databaseName, IDataProvider dataProvider);
    }
}