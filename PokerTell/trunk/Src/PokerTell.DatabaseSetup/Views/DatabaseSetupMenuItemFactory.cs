namespace PokerTell.DatabaseSetup.Views
{
    using System.Windows.Controls;

    using PokerTell.DatabaseSetup.Properties;
    using PokerTell.DatabaseSetup.ViewModels;

    public class DatabaseSetupMenuItemFactory
    {
        readonly DatabaseSetupMenuItemViewModel _viewModel;

        public DatabaseSetupMenuItemFactory(DatabaseSetupMenuItemViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        #region Public Methods

        public MenuItem Create()
        {
            var menuItem = new MenuItem { Header = Resources.DatabaseSetupMenu_Header };
            menuItem.Items.Add(new MenuItem { Header = Resources.DatabaseSetupMenu_ConfigureMySqlServer_Header, Command = _viewModel.ConfigureMySqlProviderCommand });
            menuItem.Items.Add(new MenuItem { Header = Resources.ChooseDatabaseViewModel_Title, Command = _viewModel.ChooseDatabaseCommand });

            return menuItem;
        }

        #endregion
    }
}