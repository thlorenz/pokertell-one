namespace PokerTell.DatabaseSetup
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.DatabaseSetup;

    public class DataProviderInfos : IDataProviderInfos
    {
        readonly List<IDataProviderInfo> _supportedDataProviders;

        public DataProviderInfos()
        {
            _supportedDataProviders = new List<IDataProviderInfo>();
        }

        public IDataProviderInfos Support(IDataProviderInfo dataProviderInfo)
        {
            if (!_supportedDataProviders.Contains(dataProviderInfo))
            {
                _supportedDataProviders.Add(dataProviderInfo);
            }

            return this;
        }

        public IEnumerable<IDataProviderInfo> Supported
        {
           get
            {
                return _supportedDataProviders;
            }
        }
    }
}