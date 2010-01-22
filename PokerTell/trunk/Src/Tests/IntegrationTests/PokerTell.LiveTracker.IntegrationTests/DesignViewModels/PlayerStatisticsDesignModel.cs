namespace PokerTell.LiveTracker.IntegrationTests.DesignViewModels
{
    using System;
    using System.Linq;

    using Infrastructure.Interfaces.Statistics;

    using Moq;

    using PokerHand.Analyzation;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Statistics.ViewModels;

    using StatisticsIntegrationTests.DesignViewModels;

    public class PlayerStatisticsDesignModel : PlayerStatisticsViewModel
    {
        #region Constructors and Destructors

        public PlayerStatisticsDesignModel(string playerName, int flopOutCount, int turnOutCount, int riverOutCount, int flopInCount, int turnInCount, int riverInCount)
        {
            PlayerStatistics = new StubBuilder()
                .Setup<IPlayerStatistics>()
                .Get(p => p.PlayerIdentity).Returns(new PlayerIdentity(playerName, "PokerStars"))
                .Out;

            PreFlopStatisticsSets = new PreFlopStatisticsSetsDesignModel();

            FlopStatisticsSets = new PostFlopStatisticsSetsDesignModel(Streets.Flop)
                {
                    TotalCountOutOfPositionSet = flopOutCount, 
                    TotalCountInPositionSet = flopInCount
                };

            TurnStatisticsSets = new PostFlopStatisticsSetsDesignModel(Streets.Turn)
                {
                    TotalCountOutOfPositionSet = turnOutCount, 
                    TotalCountInPositionSet = turnInCount
                };
            RiverStatisticsSets = new PostFlopStatisticsSetsDesignModel(Streets.River)
                {
                    TotalCountOutOfPositionSet = riverOutCount, 
                    TotalCountInPositionSet = riverInCount
                };
            
            RegisterEvents();

            SelectedStatisticsSetEvent += sequenceStatisticsSet => 
                Console.WriteLine("{0} acted on the {1} {2} position", 
                    sequenceStatisticsSet.PlayerName, 
                    sequenceStatisticsSet.Street,
                    sequenceStatisticsSet.InPosition ? "in" : "out of");
        }

        #endregion
    }
}