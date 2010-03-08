namespace PokerTell.Infrastructure.Interfaces.DatabaseSetup
{
    using System.Collections.Generic;

    public interface IDataProviderInfos
    {
        #region Properties

        IEnumerable<IDataProviderInfo> Supported { get; }

        #endregion

        #region Public Methods

        IDataProviderInfos Support(IDataProviderInfo dataProviderInfo);

        #endregion
    }
}