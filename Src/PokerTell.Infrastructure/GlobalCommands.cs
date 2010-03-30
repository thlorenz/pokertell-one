namespace PokerTell.Infrastructure
{
    using Microsoft.Practices.Composite.Presentation.Commands;

    public static class GlobalCommands
    {
        public static readonly CompositeCommand StartServicesCommand = new CompositeCommand();
        public static readonly CompositeCommand ConfigureServicesForFirstTimeCommand = new CompositeCommand();

    }
}