namespace PokerTell.DatabaseSetup.DatabaseVersioning
{
    using Infrastructure;

    using Interfaces;

    /// <summary>
    /// This class existsonly  to be able to use a mapping for NHibernate to create a DatabaseVersion Table
    /// NHibernate is never used to write to or read from the table at least for right now
    /// Instead writing this is done with a nonQuery command after the Database is created or cleared.
    /// This solution was chosen b/c when we create/clear a database NHibernate might not have a session for it.
    /// </summary>
    public class DatabaseVersion : IDatabaseVersion
    {
        public DatabaseVersion()
        {
            Number = ApplicationProperties.VersionNumber;
        }

        public double Number { get; protected set; }

        public int Id { get; protected set; }
    }
}