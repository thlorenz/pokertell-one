namespace PokerTell.Statistics.Filters
{
    using System;

    using Infrastructure.Events;

    using Microsoft.Practices.Composite.Events;

    using ViewModels;

    using Views;

    public class PlayerStatisticsService
    {
        public PlayerStatisticsService(IEventAggregator eventAggregator)
        {
            const bool keepMeAlive = true;
            
            eventAggregator
                .GetEvent<AdjustAnalyzablePokerPlayersFilterEvent>()
                .Subscribe(HandleAdjustAnalyzablePokerPlayersFilterEvent, keepMeAlive);
        }

        static void HandleAdjustAnalyzablePokerPlayersFilterEvent(AdjustAnalyzablePokerPlayersFilterEventArgs args)
        {
            var viewModel = new AnalyzablePokerPlayersFilterAdjustmentViewModel(
                args.PlayerName,
                args.CurrentFilter,
                args.ApplyTo,
                args.ApplyToAll);

            var view = new AnalyzablePokerPlayersFilterAdjustmentView(viewModel) { Topmost = true };
            view.ShowDialog();
        }
    }
}