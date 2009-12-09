namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    public interface IActionSequenceSetPercentagesCalculator
    {
        IActionSequenceSetPercentagesCalculator CalculatePercenagesOf(IEnumerable<IActionSequenceStatistic> statistics);
    }
}