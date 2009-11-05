using System.Collections.Generic;

namespace PokerTell.DatabaseSetup.Interfaces
{
    public interface IDataProviderInfos
    {
        IDataProviderInfos Support(IDataProviderInfo dataProviderInfo);

        IEnumerable<IDataProviderInfo> Supported { get; }
    }
}