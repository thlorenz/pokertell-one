namespace PokerTell.Repository.Views
{
    using System.Collections.Generic;
    using System.Windows.Controls;

    using PokerTell.Repository.Properties;
    using PokerTell.Repository.ViewModels;

    public class RepositoryMenuItemFactory
    {
        readonly RepositoryMenuItemsViewModel _viewModel;

        public RepositoryMenuItemFactory(RepositoryMenuItemsViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public List<MenuItem> Create()
        {
            var menuItems = new List<MenuItem>
                {
                    new MenuItem
                        {
                            Header = Resources.ImportHandHistoriesViewModel_Title, 
                            Command = _viewModel.ImportHandHistoriesCommand
                        },
                    new MenuItem
                        {
                            Header = Resources.DatabaseImportView_Title, 
                            Command = _viewModel.ImportDatabaseCommand
                        }
                };

            return menuItems;
        }
    }
}