namespace PokerTell.DatabaseSetup.Interfaces
{
    using System.Collections.Generic;

    public interface IDatabaseManager
    {
        #region Public Methods

        IDatabaseManager ChooseDatabase(string databaseName);

        IDatabaseManager ClearDatabase(string databaseName);

        IDatabaseManager CreateDatabase(string databaseName);

        IDatabaseManager DeleteDatabase(string databaseName);

        IEnumerable<string> GetAllPokerTellDatabases();

        #endregion
    }
}