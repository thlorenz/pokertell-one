namespace PokerTell.DatabaseSetup.ViewModels
{
    using System;
    
    using System.Windows.Input;

    using Microsoft.Practices.Unity;

    using Tools.WPF;

    using Views;

    public class DatabaseSetupMenuItemViewModel
    {
        
        
        public DatabaseSetupMenuItemViewModel(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        ICommand _configureMySqlProviderCommand;

        readonly IUnityContainer _unityContainer;

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

                                _unityContainer.Resolve<ConfigureMySqlDataProviderView>().ShowDialog();
                            }
                            catch (Exception excep)
                            {
                                Console.WriteLine(excep.ToString());
                            }
                            }
                });
            }
        }
    }
}