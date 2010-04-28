namespace PokerTell.DatabaseSetup.ViewModels
{
    using System;

    using Base;

    using Infrastructure.Interfaces.DatabaseSetup;

    using Microsoft.Practices.Composite.Events;

    public class ConfigurePostgreSqlDataProviderViewModel : ConfigureDataProviderViewModelBase
    {
        readonly IDataProviderInfo _dataProviderInfo;
        
        public ConfigurePostgreSqlDataProviderViewModel(IEventAggregator eventAggregator, IDatabaseSettings databaseSettings, IDatabaseConnector databaseConnector)
            : base(eventAggregator, databaseSettings, databaseConnector)
        {
            _dataProviderInfo = new PostgreSqlInfo();

            Initialize();
        }

        protected override IDataProviderInfo DataProviderInfo
        {
            get { return _dataProviderInfo; }
        }
    }
}