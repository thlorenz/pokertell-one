namespace PokerTell.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;
    using System.Threading;

    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using log4net;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;

    public class PlayerStatisticsUpdater : IPlayerStatisticsUpdater
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        IBackgroundWorker _workerForCollectionUpdate;

        IBackgroundWorker _workerForSingleStatistic;

        readonly IConstructor<IBackgroundWorker> _backgroundWorkerMake;

        public PlayerStatisticsUpdater(IConstructor<IBackgroundWorker> backgroundWorkerMake)
        {
            _backgroundWorkerMake = backgroundWorkerMake;

            CreateWorkerForCollectionUpdate();
        }

        void CreateWorkerForCollectionUpdate()
        {
            _workerForCollectionUpdate = _backgroundWorkerMake.New;

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
            PlayerThatIsCurrentlyUpdated = playerStatistics.PlayerIdentity;
            CreateWorkerForSingleStatisticUpdate();

            _workerForSingleStatistic.RunWorkerAsync(playerStatistics);
        }

        public IPlayerIdentity PlayerThatIsCurrentlyUpdated { get; protected set; }

        void CreateWorkerForSingleStatisticUpdate()
        {
            _workerForSingleStatistic = _backgroundWorkerMake.New;
            _workerForSingleStatistic.DoWork += (_, e) => {
                // Make sure we parse doubles correctly
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);

                var playerStatistics = (IPlayerStatistics)e.Argument;
                playerStatistics.UpdateStatistics();
                e.Result = playerStatistics;
            };
            _workerForSingleStatistic.RunWorkerCompleted += (_, e) => {
                var playerStatistics = (IPlayerStatistics)e.Result;
                if (playerStatistics.PlayerIdentity.Equals(PlayerThatIsCurrentlyUpdated))
                    FinishedUpdatingPlayerStatistics(playerStatistics);
            };
        }

        public event Action<IEnumerable<IPlayerStatistics>> FinishedUpdatingMultiplePlayerStatistics = delegate { };

        public event Action<IPlayerStatistics> FinishedUpdatingPlayerStatistics = delegate { };
    }
}