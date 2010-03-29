namespace PokerTell.Plugins.InstantHandHistoryReader
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Interfaces;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;

    using PokerTell.Infrastructure.Interfaces.Repository;

    /// <summary>
    /// Initializes a HandHistoryReader for PokerStars (currently only works for it)
    /// </summary>
    public class InstantHandHistoryReaderService
    {

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const string pokerStarsHandHistoryStartingValue = "PokerStars Game #";

        const string processName = "pokerstars";

        readonly IRepository _repository;

        readonly IHandHistoryReader _handHistoryReader;

        public Timer Timer { get; set; }

        public InstantHandHistoryReaderService(IRepository repository, IHandHistoryReader handHistoryReader)
        {
            _repository = repository;
            _handHistoryReader = handHistoryReader;
        }

        public void ReadInstantHandHistoriesFromMemory(object state)
        {
            try
            {
                if (! _handHistoryReader.WasSuccessfullyInitialized)
                    _handHistoryReader.InitializeWith(processName, pokerStarsHandHistoryStartingValue);
                
                var allHandHistoriesInOneString =
                    _handHistoryReader
                    .FindNewInstantHandHistories()
                    .Aggregate(string.Empty, (collectedSoFar, currentHandHistory) => collectedSoFar + "\n\n" + currentHandHistory);

                var convertedHands = _repository.RetrieveHandsFromString(allHandHistoriesInOneString);

                _repository.InsertHands(convertedHands);

                Log.DebugFormat("Inserted {0} newly mined hands", convertedHands.Count());
            }
            catch (Exception excep)
            {
                Log.Error(excep);
            }
        }
    }
}