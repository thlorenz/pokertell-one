namespace PokerTell.Repository.PerformanceTests
{
    using IntegrationTests;

    using NUnit.Framework;

    using PokerHand.Dao;

    [TestFixture]
    public class QueryPerformanceTests : DatabaseConnectedPerformanceTests
    {
        [Test]
        public void FindConvertedPokerPlayerById_MySql()
        {
            // Took 10.5s for 22,000 hands
            SetupMySqlConnection("data source = localhost; user id = root; database=firstnh;");
            var convertedPokerPlayerDao = new ConvertedPokerPlayerDao(_sessionFactoryManagerStub.Object);

            Timed("FindConvertedPokerPlayerById_MySql",
                  () => convertedPokerPlayerDao.FindByPlayerIdentity(8));
        }

        [Test]
        public void FindAnalyzablePlayersWithLegacy_MySql()
        {
            // Took 2.6s for 22,000 hands
            // Takes about 4.3s when getting Values from PokerHand as well
            SetupMySqlConnection("data source = localhost; user id = root; database=firstnh;");
            var convertedPokerPlayerDao = new ConvertedPokerPlayerDao(_sessionFactoryManagerStub.Object);

            Timed("FindHandIdsFromPlayerById_MySql",
                  () => convertedPokerPlayerDao.FindAnalyzablePlayersWithLegacy(8, 0));
        }

        [Test]
        public void FindAnalyzablePlayersWith_MySql()
        {
            // Takes 6.2s when getting Values from PokerHand including Sequences as well 
            SetupMySqlConnection("data source = localhost; user id = root; database=firstnh;");
            var convertedPokerPlayerDao = new ConvertedPokerPlayerDao(_sessionFactoryManagerStub.Object);

            Timed("FindHandIdsFromPlayerById_MySql",
                  () => convertedPokerPlayerDao.FindAnalyzablePlayersWith(7, 0));
        }

        [Test]
        public void UsingRawSqlQueryConvertedPokerPlayersWith_MySql()
        {
            // Took 3.1s for 22,000 hands
            SetupMySqlConnection("data source = localhost; user id = root; database=firstnh;");
            var convertedPokerPlayerDao = new ConvertedPokerPlayerDao(_sessionFactoryManagerStub.Object);

            Timed("FindStatValuesByPlayerIdentity_MySql", 
                  () => convertedPokerPlayerDao.UsingRawSqlQueryConvertedPokerPlayersWith(8));
        }
    }
}