namespace PokerTell.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;
    using System.Threading;

    using Infrastructure.Interfaces.Statistics;

    using log4net;

    using Tools.FunctionalCSharp;

    public class PlayerStatisticsUpdater : IPlayerStatisticsUpdater
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly BackgroundWorker _workerForCollectionUpdate;

        BackgroundWorker _workerForSingleStatistic;

        public PlayerStatisticsUpdater()
        {
            _workerForCollectionUpdate = new BackgroundWorker();

            _workerForCollectionUpdate.DoWork += (_, e) => {
                // Make sure we parse doubles correctly
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);

                var playerStatistics = (IEnumerable<IPlayerStatistics>) e.Argument;
                playerStatistics.ForEach(ps => ps.UpdateStatistics());
                e.Result = playerStatistics;
            };

            _workerForCollectionUpdate.RunWorkerCompleted += (_, e) => {
                var updatedPlayerStatistics = (IEnumerable<IPlayerStatistics>) e.Result;
                FinishedUpdatingMultiplePlayerStatistics(updatedPlayerStatistics);
            };
        }

        /// <summary>
        /// Update the given collection of playerstatistics in the background, unless the worker is still busy in which case it just returns.
        /// </summary>
        /// <param name="playerStatistics"></param>
        public void Update(IEnumerable<IPlayerStatistics> playerStatistics)
        {
            if (playerStatistics == null)
            {
                Log.Debug("Passed playerStatistics were null -> couldn't update");
                return;
            }

            if (_workerForCollectionUpdate.IsBusy) return; 

            _workerForCollectionUpdate.RunWorkerAsync(playerStatistics);
        }

        /// <summary>
        /// Updates the given playerstatistics.
        /// If the worker is currently busy, it will unsubscribe from its completed event and create a new worker to perform the update.
        /// </summary>
        /// <param name="playerStatistics"></param>
        public void Update(IPlayerStatistics playerStatistics)
        {
            // If it was busy, unsubscribe from RunWorkerCompleted
            if (_workerForSingleStatistic != null && _workerForSingleStatistic.IsBusy)
                _workerForSingleStatistic.RunWorkerCompleted -= (_, e) => FinishedUpdatingPlayerStatistics((IPlayerStatistics)e.Result);

            RecreateWorkerToUpdate(playerStatistics);

            _workerForSingleStatistic.RunWorkerAsync(playerStatistics);
        }

        void RecreateWorkerToUpdate(IPlayerStatistics playerStatistics)
        {
            _workerForSingleStatistic = new BackgroundWorker();
            _workerForSingleStatistic.DoWork += (_, evs) => {
                // Make sure we parse doubles correctly
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
                playerStatistics.UpdateStatistics();
                evs.Result = playerStatistics;
            };
            _workerForSingleStatistic.RunWorkerCompleted += (_, e) => FinishedUpdatingPlayerStatistics((IPlayerStatistics)e.Result);
        }

        public event Action<IEnumerable<IPlayerStatistics>> FinishedUpdatingMultiplePlayerStatistics;

        public event Action<IPlayerStatistics> FinishedUpdatingPlayerStatistics = delegate { };
    }
}