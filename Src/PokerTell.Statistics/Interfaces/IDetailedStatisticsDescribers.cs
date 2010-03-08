namespace PokerTell.Statistics.Interfaces
{
    using PokerTell.Infrastructure.Enumerations.PokerHand;

    public interface IDetailedPostFlopHeroActsStatisticsDescriber : IDetailedStatisticsDescriber
    {
    }

    public interface IDetailedPostFlopHeroReactsStatisticsDescriber : IDetailedStatisticsDescriber
    {
    }

    public interface IDetailedPreFlopStatisticsDescriber : IDetailedStatisticsDescriber
    {
        string Describe(string playerName, ActionSequences actionSequence);
    }

    public interface IDetailedStatisticsDescriber
    {
        string Describe(string playerName, ActionSequences actionSequence, Streets street, bool inPosition);

        string Hint(string playerName);
    }
}