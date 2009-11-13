namespace PokerTell.Repository.ViewModels
{
    using System.Windows.Input;

    using Microsoft.Practices.Unity;

    using Tools.WPF;

    using Views;

    public class RepositoryMenuItemsViewModel
    {
        #region Constants and Fields

        readonly IUnityContainer _container;

        ICommand _importHandHistoriesCommand;

        #endregion

        #region Constructors and Destructors

        public RepositoryMenuItemsViewModel(IUnityContainer container)
        {
            _container = container;
        }

        #endregion

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

    }
}