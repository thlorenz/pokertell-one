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

    using Tools;

    using User;

    public class Bootstrapper : UnityBootstrapper
    {
        protected override IModuleCatalog GetModuleCatalog()
        {
            var catalog = new ModuleCatalog();

             // *************
             // Core Modules
             // *************
            catalog
                .AddModule(typeof(ToolsModule))
                .AddModule(typeof(UserModule))
                .AddModule(typeof(PokerHandModule), typeof(UserModule).Name)
                .AddModule(typeof(DatabaseSetupModule), typeof(UserModule).Name)
                .AddModule(typeof(PokerHandParsersModule), typeof(PokerHandModule).Name, typeof(UserModule).Name)
                .AddModule(typeof(RepositoryModule), typeof(PokerHandParsersModule).Name, typeof(PokerHandModule).Name, typeof(UserModule).Name)
                .AddModule(typeof(StatisticsModule), typeof(RepositoryModule).Name, typeof(PokerHandModule).Name, typeof(UserModule).Name)
                .AddModule(typeof(LiveTrackerModuleMock), typeof(StatisticsModule).Name, typeof(RepositoryModule).Name, typeof(PokerHandModule).Name, typeof(UserModule).Name);

            // *******************************
            // Plugins found in Plugins folder
            // *******************************
            var pluginsCatalog = new DirectoryModuleCatalog() { ModulePath = @".\Plugins" };
            
            // We need to load them here in order to add them to the main catalog
            // For this reason the plugin modules also need to explicitly declare all their dependencies, since Prism will otherwise
            // attempt to inititialize them before the main catalog was initialized.
            pluginsCatalog.Load();
           
            foreach (var moduleInfo in pluginsCatalog.Modules)
            {
                catalog.AddModule(moduleInfo);
            }

            return catalog;
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