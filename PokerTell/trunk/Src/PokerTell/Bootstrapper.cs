namespace PokerTell
{
    using System.Windows;

    using DatabaseSetup;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.UnityExtensions;

    using PokerHand;

    using PokerHandParsers;

    using Repository;

    using SessionReview;

    using User;

    public class Bootstrapper : UnityBootstrapper
    {
        protected override IModuleCatalog GetModuleCatalog()
        {
            var catalog = new ModuleCatalog();

            catalog
                .AddModule(typeof(UserModule))
                .AddModule(typeof(PokerHandModule), typeof(UserModule).Name)
                .AddModule(typeof(DatabaseSetupModule), typeof(UserModule).Name)
                .AddModule(typeof(PokerHandParsersModule), typeof(PokerHandModule).Name, typeof(UserModule).Name)
                .AddModule(typeof(RepositoryModule), typeof(PokerHandParsersModule).Name, typeof(PokerHandModule).Name, typeof(UserModule).Name)
                .AddModule(typeof(SessionReviewModule), typeof(RepositoryModule).Name, typeof(PokerHandModule).Name, typeof(UserModule).Name);
         
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