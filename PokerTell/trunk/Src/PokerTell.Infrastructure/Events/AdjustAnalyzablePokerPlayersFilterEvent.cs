namespace PokerTell.Infrastructure.Events
{
    using System;

    using Interfaces.Statistics;

    using Microsoft.Practices.Composite.Presentation.Events;

    public class AdjustAnalyzablePokerPlayersFilterEvent : CompositePresentationEvent<AdjustAnalyzablePokerPlayersFilterEventArgs>
    {
    }

    public class AdjustAnalyzablePokerPlayersFilterEventArgs 
    {
        #region Constructors and Destructors

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

        #endregion

        #region Properties

        public Action<string, IAnalyzablePokerPlayersFilter> ApplyTo { get; protected set; }

        public Action<IAnalyzablePokerPlayersFilter> ApplyToAll { get; protected set; }

        public string PlayerName { get; protected set; }

        public IAnalyzablePokerPlayersFilter CurrentFilter { get; protected set; }

        #endregion
    }
}