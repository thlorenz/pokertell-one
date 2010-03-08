namespace PokerTell.Repository.Views
{
    using System.Collections.Generic;
    using System.Windows.Controls;

    using Properties;

    using ViewModels;

    public class RepositoryMenuItemFactory
    {
        #region Constants and Fields

        readonly RepositoryMenuItemsViewModel _viewModel;

        #endregion

        #region Constructors and Destructors

        public RepositoryMenuItemFactory(RepositoryMenuItemsViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        #endregion

        #region Public Methods

        public List<MenuItem> Create()
        {
            var menuItems = new List<MenuItem>
                {
                    new MenuItem
                        {
                            Header = Resources.ImportHandHistoriesViewModel_Title,
                            Command = _viewModel.ImportHandHistoriesCommand
                        }
                };

            return menuItems;
        }

        #endregion
    }
}