namespace PokerTell.Repository.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.DatabaseSetup;

    public interface IHandHistoryRetriever : IFluentInterface
    {
        bool IsDone { get; }

        int HandHistoriesCount { get; }

        IEnumerable<string> GetNext(int numberOfHandHistories);

        IHandHistoryRetriever Using(IDataProvider dataProvider);
    }

    public interface IPokerTellHandHistoryRetriever : IHandHistoryRetriever
    {
    }

    public interface IPokerOfficeHandHistoryRetriever : IHandHistoryRetriever
    {
    }

    public interface IPokerTrackerHandHistoryRetriever : IHandHistoryRetriever
    {
    }
}