namespace PokerTell.Infrastructure.Events
{
    using Interfaces.DatabaseSetup;

    using Microsoft.Practices.Composite.Presentation.Events;

    public class DatabaseInUseChangedEvent : CompositePresentationEvent<IDataProvider>
    {
        public DatabaseInUseChangedEvent() 
        {
        }
    }
}