namespace PokerTell.DatabaseSetup.ViewModels
{
    using System;
    
    using System.Windows.Input;

    using Infrastructure.Interfaces.DatabaseSetup;

    using Microsoft.Practices.Unity;

    using Tools.WPF;

    using Views;

    public class DatabaseSetupMenuItemViewModel
    {
        public DatabaseSetupMenuItemViewModel(IUnityContainer container)
        {
            _container = container;
        }

        ICommand _configureMySqlProviderCommand;

        readonly IUnityContainer _container;

        public ICommand ConfigureMySqlProviderCommand
        {
            get
            {

                return _configureMySqlProviderCommand ?? (_configureMySqlProviderCommand = new SimpleCommand
                {
                    ExecuteDelegate = arg =>
                        {
                            try
                            {
                                _container.Resolve<ConfigureMySqlDataProviderView>().ShowDialog();
                            }
                            catch (Exception excep)
                            {
                                Console.WriteLine(excep.ToString());
                            }
                            }
                });
            }
        }

        ICommand _chooseDatabaseCommand;

        public ICommand ChooseDatabaseCommand
        {
            get
            {
                return _chooseDatabaseCommand ?? (_chooseDatabaseCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => 
                        {
                            var databaseConnector = _container.Resolve<IDatabaseConnector>();
                            var databaseManager =
                                databaseConnector
                                    .InitializeFromSettings()
                                    .ConnectToServer()
                                    .CreateDatabaseManager();
                            if (databaseManager != null)
                            {
                                _container
                                 .RegisterInstance(databaseManager);

                                new ComboBoxDialogView(_container.Resolve<ChooseDatabaseViewModel>()).ShowDialog(); 
                            }
                        },
                    });
            }
        }
    }
}