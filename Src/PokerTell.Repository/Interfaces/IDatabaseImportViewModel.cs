namespace PokerTell.Repository.Interfaces
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using Infrastructure.Interfaces.DatabaseSetup;

    public interface IDatabaseImportViewModel
    {
        IList<string> DatabaseNames { get; }

        IList<IDataProviderInfo> DataProvidersInfos { get; }

        PokerStatisticsApplications SelectedApplication { get; set; }

        IDataProviderInfo SelectedDataProviderInfo { get; set; }

        IList<PokerStatisticsApplications> SupportedApplications { get; }

        ICommand ImportDatabaseCommand { get; }

        string SelectedDatabaseName { get; set; }
    }
}