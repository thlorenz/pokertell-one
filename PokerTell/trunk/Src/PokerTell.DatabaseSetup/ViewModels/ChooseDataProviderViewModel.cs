using System;

using PokerTell.DatabaseSetup.Interfaces;

namespace PokerTell.DatabaseSetup.ViewModels
{
    using System.Collections.Generic;

    public class ChooseDataProviderViewModel
    {
        public ChooseDataProviderViewModel(IDatabaseSettings databaseSettings)
        {
            AvailableProviders = new List<IDataProviderInfo>(databaseSettings.GetAvailableProviders());
        }

        public IList<IDataProviderInfo> AvailableProviders { get; private set; }
    }
}