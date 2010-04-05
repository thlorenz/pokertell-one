namespace PokerTell.Infrastructure.Events
{
    using System;

    using Microsoft.Practices.Composite.Presentation.Events;

    using PokerTell.Infrastructure.Interfaces.Statistics;

    public class AdjustAnalyzablePokerPlayersFilterEvent : CompositePresentationEvent<AdjustAnalyzablePokerPlayersFilterEventArgs>
    {
    }

    public class AdjustAnalyzablePokerPlayersFilterEventArgs
    {
        public AdjustAnalyzablePokerPlayersFilterEventArgs(
            string playerName, 
            IAnalyzablePokerPlayersFilter currentFilter, 
            Action<string, IAnalyzablePokerPlayersFilter> applyTo, 
            Action<IAnalyzablePokerPlayersFilter> applyToAll)
        {
            PlayerName = playerName;
            CurrentFilter = currentFilter;
            ApplyTo = applyTo;
            ApplyToAll = applyToAll;
        }

        public Action<string, IAnalyzablePokerPlayersFilter> ApplyTo { get; protected set; }

        public Action<IAnalyzablePokerPlayersFilter> ApplyToAll { get; protected set; }

        public IAnalyzablePokerPlayersFilter CurrentFilter { get; protected set; }

        public string PlayerName { get; protected set; }
    }
}