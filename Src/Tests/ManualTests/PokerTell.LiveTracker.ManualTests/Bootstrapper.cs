namespace PokerTell.LiveTracker.ManualTests
{
    using System.Windows;

    using DatabaseSetup;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.UnityExtensions;

    using NewHandCreator;

    using PokerHand;

    using PokerHandParsers;

    using Repository;

    using Statistics;

    using User;

    public class Bootstrapper : UnityBootstrapper
    {
        protected override IModuleCatalog GetModuleCatalog()
        {
            var catalog = new ModuleCatalog();

            return catalog
                .AddModule(typeof(UserModule))
                .AddModule(typeof(PokerHandModule), typeof(UserModule).Name)
                .AddModule(typeof(DatabaseSetupModule), typeof(UserModule).Name)
                .AddModule(typeof(PokerHandParsersModule), typeof(PokerHandModule).Name, typeof(UserModule).Name)
                .AddModule(typeof(RepositoryModule), typeof(PokerHandParsersModule).Name, typeof(PokerHandModule).Name, typeof(UserModule).Name)
                .AddModule(typeof(StatisticsModule), typeof(RepositoryModule).Name, typeof(PokerHandModule).Name, typeof(UserModule).Name)
                .AddModule(typeof(LiveTrackerModuleMock), typeof(StatisticsModule).Name, typeof(RepositoryModule).Name, typeof(PokerHandModule).Name, typeof(UserModule).Name);
        }
        
        protected override DependencyObject CreateShell()
        {
            Container
                .RegisterType<IShellViewModel, ShellViewModel>();
            
            var shell = Container.Resolve<Shell>();
           
            Application.Current.MainWindow = shell;
            
            shell.Show();
            return shell;
        }
    }
}