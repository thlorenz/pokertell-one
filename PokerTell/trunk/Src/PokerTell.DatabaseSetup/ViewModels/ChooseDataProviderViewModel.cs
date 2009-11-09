namespace PokerTell.DatabaseSetup.ViewModels
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.DatabaseSetup;

    public class ChooseDataProviderViewModel
    {
        #region Constructors and Destructors

        public ChooseDataProviderViewModel(IDatabaseSettings databaseSettings)
        {
            AvailableProviders = new List<IDataProviderInfo>(databaseSettings.GetAvailableProviders());
        }

        #endregion

        #region Properties

        public IList<IDataProviderInfo> AvailableProviders { get; private set; }

        #endregion
    }
}