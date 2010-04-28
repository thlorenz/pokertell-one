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

        public MenuItem Create()
        {
            var menuItem = new MenuItem { Header = Resources.DatabaseSetupMenu_Header };

            menuItem.Items.Add(
                new MenuItem
                    {
                        Header = Resources.ChooseDatabaseViewModel_Title, 
                        Command = _viewModel.ChooseDatabaseCommand
                    });

            menuItem.Items.Add(
                new MenuItem
                    {
                        Header = Resources.CreateDatabaseViewModel_Title, 
                        Command = _viewModel.CreateDatabaseCommand
                    });

            menuItem.Items.Add(new Separator());

            menuItem.Items.Add(
                new MenuItem
                    {
                        Header = Resources.ClearDatabaseViewModel_Title, 
                        Command = _viewModel.ClearDatabaseCommand
                    });
            menuItem.Items.Add(
                new MenuItem
                    {
                        Header = Resources.DeleteDatabaseViewModel_Title, 
                        Command = _viewModel.DeleteDatabaseCommand
                    });

            menuItem.Items.Add(new Separator());

            menuItem.Items.Add(
                new MenuItem
                    {
                        Header = Resources.ChooseDataProviderViewModel_Title, 
                        Command = _viewModel.ChooseDataProviderCommand
                    });

            menuItem.Items.Add(new Separator());

            menuItem.Items.Add(
                new MenuItem
                    {
                        Header = Resources.DatabaseSetupMenu_ConfigureMySqlServer_Header, 
                        Command = _viewModel.ConfigureMySqlProviderCommand
                    });

            menuItem.Items.Add(
                new MenuItem
                    {
                        Header = Resources.DatabaseSetupMenu_ConfigurePostgreSqlServer_Header, 
                        Command = _viewModel.ConfigurePostgreSqlProviderCommand
                    });
            return menuItem;
        }
    }
}