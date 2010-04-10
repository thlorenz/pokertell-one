namespace PokerTell.Statistics.Design
{
    using System.Collections.Generic;

    using DetailedStatistics;

    using Moq;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.Interfaces;
    using PokerTell.Statistics.ViewModels;

    public class RepositoryPlayersStatisticsDesignModel : RepositoryPlayersStatisticsViewModel
    {
        public RepositoryPlayersStatisticsDesignModel()
            : base(
                RepositoryStub, 
                PlayerStatisticsMakeStub, 
                PlayerStatisticsUpdaterStub, 
                PlayerStatisticsViewModelStub, 
                DetailedStatisticsAnalyzerViewModelStub, 
                ActiveAnalyzablePlayersSelectorStub, 
                FilterPopupViewModelStub)
        {
            SelectedPlayer = PlayerStatisticsDesign.Model;
        }

        static IActiveAnalyzablePlayersSelector ActiveAnalyzablePlayersSelectorStub
        {
            get
            {
                var activeAnalyzablePlayersSelectorStub = new Mock<IActiveAnalyzablePlayersSelector>();
                return activeAnalyzablePlayersSelectorStub.Object;
            }
        }

        static IDetailedStatisticsAnalyzerViewModel DetailedStatisticsAnalyzerViewModelStub
        {
            get
            {
                var detailedStatisticsAnalyzerStub = new Mock<IDetailedStatisticsAnalyzerViewModel>();
                detailedStatisticsAnalyzerStub
                    .SetupGet(dsa => dsa.CurrentViewModel)
                    .Returns(new DetailedPreFlopStatisticsDesignModel());

                return detailedStatisticsAnalyzerStub.Object;
            }
        }

        static IFilterPopupViewModel FilterPopupViewModelStub
        {
            get { return new Mock<IFilterPopupViewModel>().Object; }
        }

        static IConstructor<IPlayerStatistics> PlayerStatisticsMakeStub
        {
            get
            {
                var playerStatisticsStub = new Mock<IPlayerStatistics>();
                playerStatisticsStub
                    .Setup(ps => ps.InitializePlayer(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(playerStatisticsStub.Object);

                var playerStatisticsMakeStub = new Mock<IConstructor<IPlayerStatistics>>();
                playerStatisticsMakeStub
                    .SetupGet(psm => psm.New)
                    .Returns(playerStatisticsStub.Object);

                return playerStatisticsMakeStub.Object;
            }
        }

        static IPlayerStatisticsUpdater PlayerStatisticsUpdaterStub
        {
            get { return new Mock<IPlayerStatisticsUpdater>().Object; }
        }

        static IPlayerStatisticsViewModel PlayerStatisticsViewModelStub
        {
            get { return new Mock<IPlayerStatisticsViewModel>().Object; }
        }

        static IRepository RepositoryStub
        {
            get
            {
                var playerIdentitiesStub = new List<IPlayerIdentity>();
                for (int i = 0; i < 20; i++)
                {
                    playerIdentitiesStub.Add(PlayerIdentityStubFor("player" + i, "PokerStars"));
                }

                var repositoryStub = new Mock<IRepository>();
                repositoryStub
                    .Setup(r => r.RetrieveAllPlayerIdentities())
                    .Returns(playerIdentitiesStub);

                return repositoryStub.Object;
            }
        }

        static IPlayerIdentity PlayerIdentityStubFor(string name, string site)
        {
            var identityStub = new Mock<IPlayerIdentity>();
            identityStub
                .SetupGet(pi => pi.Name)
                .Returns(name);
            identityStub
                .SetupGet(pi => pi.Site)
                .Returns(site);

            return identityStub.Object;
        }
    }

    public static class RepositoryPlayersStatisticsDesign
    {
        public static IRepositoryPlayersStatisticsViewModel Model
        {
            get { return new RepositoryPlayersStatisticsDesignModel(); }
        }
    }
}