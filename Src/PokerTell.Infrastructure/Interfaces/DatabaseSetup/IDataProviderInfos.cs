namespace PokerTell.Infrastructure.Interfaces.DatabaseSetup
{
    using System.Collections.Generic;

    public interface IDataProviderInfos : IFluentInterface
    {
        IEnumerable<IDataProviderInfo> Supported { get; }

        IDataProviderInfo MySqlProviderInfo { get; }

        IDataProviderInfo SQLiteProviderInfo { get; }

        IDataProviderInfo PostgresProviderInfo { get; }

        IDataProviderInfos Support(IDataProviderInfo dataProviderInfo);
    }
}