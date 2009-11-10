namespace PokerTell.DatabaseSetup.ViewModels
{
    using System;
    using System.Windows.Input;

    using Microsoft.Practices.Unity;

    using PokerTell.DatabaseSetup.Views;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    using Tools.WPF;

    public class DatabaseSetupMenuItemViewModel
    {
        #region Constants and Fields

        readonly IUnityContainer _container;

        ICommand _chooseDatabaseCommand;

        ICommand _chooseDataProviderCommand;

        ICommand _configureMySqlProviderCommand;

        #endregion

        #region Constructors and Destructors

        public DatabaseSetupMenuItemViewModel(IUnityContainer container)
        {
            _container = container;
        }

        #endregion

        #region Properties

        public ICommand ChooseDatabaseCommand
        {
            get
            {
                return _chooseDatabaseCommand ?? (_chooseDatabaseCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            var databaseConnector = _container.Resolve<IDatabaseConnector>();
                            IDatabaseManager databaseManager =
                                databaseConnector
                                    .InitializeFromSettings()
                                    .ConnectToServer()
                                    .CreateDatabaseManager();
                           
                            if (databaseManager != null)
                            {
                                _container
                                    .RegisterInstance(databaseManager);

                                new ComboBoxDialogView(
                                    _container
                                    .Resolve<ChooseDatabaseViewModel>()
                                    .DetermineSelectedItem())
                                    .ShowDialog();
                            }
                        }, 
                    });
            }
        }

        public ICommand ChooseDataProviderCommand
        {
            get
            {
                return _chooseDataProviderCommand ?? (_chooseDataProviderCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            var chooseProviderViewModel = _container.Resolve<ChooseDataProviderViewModel>();
                               chooseProviderViewModel.DetermineSelectedItem();
                          
                            if (chooseProviderViewModel.IsValid)
                            {
                                new ComboBoxDialogView(chooseProviderViewModel).ShowDialog();
                            }
                        }
                    });
            }
        }

        public ICommand ConfigureMySqlProviderCommand
        {
            get
            {
                return _configureMySqlProviderCommand ?? (_configureMySqlProviderCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
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

        #endregion
    }
}