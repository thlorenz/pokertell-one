namespace PokerTell
{
    using System.Windows;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.UnityExtensions;

    using PokerHand;

    using SessionReview;

    public class Bootstrapper : UnityBootstrapper
    {
        protected override IModuleCatalog GetModuleCatalog()
        {
            var catalog = new ModuleCatalog();

            catalog
                .AddModule(typeof(PokerHandModule))
                .AddModule(typeof(SessionReviewModule), typeof(PokerHandModule).Name);
         
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