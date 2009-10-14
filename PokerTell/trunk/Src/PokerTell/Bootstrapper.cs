namespace PokerTell
{
    using System.Windows;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.UnityExtensions;

    using PokerHand;

    public class Bootstrapper : UnityBootstrapper
    {
        protected override IModuleCatalog GetModuleCatalog()
        {
            var catalog = new ModuleCatalog();

            catalog
                .AddModule(typeof(PokerHandModule));
         
            return catalog;
        }
        
        protected override DependencyObject CreateShell()
        {
            var shell = Container.Resolve<Shell>();
            shell.Show();
            return shell;
        }
    }
}