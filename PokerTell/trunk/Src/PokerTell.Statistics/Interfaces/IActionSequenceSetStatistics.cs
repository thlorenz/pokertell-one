namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    public interface IActionSequenceSetStatistics
    {
        IActionSequenceSetStatistics Update();

        IActionSequenceSetStatistics Filter();

        IList<IActionSequenceStatistic> ActionSequenceStatistics { get; }
    }
}