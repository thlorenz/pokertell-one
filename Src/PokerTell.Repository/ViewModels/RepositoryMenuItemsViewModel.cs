namespace PokerTell.Repository.ViewModels
{
    using System.Windows.Input;

    using Microsoft.Practices.Unity;

    using PokerTell.Repository.Views;

    using Tools.WPF;

    public class RepositoryMenuItemsViewModel
    {
        readonly IUnityContainer _container;

        ICommand _importHandHistoriesCommand;

        public RepositoryMenuItemsViewModel(IUnityContainer container)
        {
            _container = container;
        }

        public ICommand ImportHandHistoriesCommand
        {
            get
            {
                return _importHandHistoriesCommand ?? (_importHandHistoriesCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => _container.Resolve<ImportHandHistoriesView>().ShowDialog(), 
                    });
            }
        }

        ICommand _importDatabaseCommand;

        public ICommand ImportDatabaseCommand
        {
            get
            {
                return _importDatabaseCommand ?? (_importDatabaseCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => _container.Resolve<DatabaseImportView>().ShowDialog(),
                    });
            }
        }
    }
}