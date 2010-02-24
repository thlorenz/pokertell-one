namespace PokerTell.LiveTracker.Tests.Utilities
{
    using Infrastructure.Interfaces.Statistics;

    using Moq;

    internal static class Utils
    {
        internal static Mock<IPlayerStatisticsViewModel> PlayerStatisticsVM_MockFor(string name)
        {
            var playerViewModelMock = new Mock<IPlayerStatisticsViewModel>();
            playerViewModelMock.SetupGet(pvm => pvm.PlayerName).Returns(name);
            return playerViewModelMock;
        }
    }
}