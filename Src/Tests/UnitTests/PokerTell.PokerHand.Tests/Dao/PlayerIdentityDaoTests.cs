namespace PokerTell.PokerHand.Tests.Dao
{
    using Base;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Repository;

    using Moq;

    using NHibernate;
    using NHibernate.Tool.hbm2ddl;

    using NUnit.Framework;

    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Dao;

    using UnitTests.Tools;

    [TestFixture]
    public class PlayerIdentityDaoTests : InMemoryDatabaseTest
    {
        #region Constants and Fields

        PlayerIdentityDao _sut;

        #endregion

        #region Constructors and Destructors

        public PlayerIdentityDaoTests()
            : base(typeof(PlayerIdentity).Assembly)
        {
        }

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            new SchemaExport(_configuration).Execute(false, true, false, _session.Connection, null);
            FlushAndClearSession();
            
            var sessionFactoryManager = new Mock<ISessionFactoryManager>();
            sessionFactoryManager
                .SetupGet(sfm => sfm.CurrentSession)
                .Returns(_session);

            _sut = new PlayerIdentityDao(sessionFactoryManager.Object);
        }

        [Test]
        public void FindPlayerIdentityFor_DatabaseEmpty_ReturnsNull()
        {
            IPlayerIdentity returnedIdentity = _sut.FindPlayerIdentityFor("someName", "someSite");

            returnedIdentity.ShouldBeNull();
        }

        [Test]
        public void FindPlayerIdentityFor_DatabaseContainsName_ReturnsIdentityWithThatNameAndSite()
        {
            const string someName = "someName";
            const string someSite = "PokerStars";
            IPlayerIdentity playerIdentity = new PlayerIdentity(someName, someSite);
            _session.SaveOrUpdate(playerIdentity);

            FlushAndClearSession();

            IPlayerIdentity returnedIdentity = _sut.FindPlayerIdentityFor(someName, someSite);

            returnedIdentity.ShouldBeEqualTo(playerIdentity);
        }

        [Test]
        public void FindOrInsert_PlayerIdentityIsInDatabase_AssignsPlayerIdentityIdFromDatabase()
        {
            const string someName = "someName";
            const string someSite = "PokerStars";
            var playerIdentity = new PlayerIdentity(someName, someSite);

            IPlayerIdentity insertedIdentity = _sut.Insert(playerIdentity);

            FlushAndClearSession();

            IPlayerIdentity samePlayerIdentity = _sut.FindOrInsert(someName, someSite);

            samePlayerIdentity.Id.ShouldBeEqualTo(insertedIdentity.Id);
        }

        [Test]
        public void FindOrInsert_PlayerIdentityNotInDatabase_InsertsPlayerIdentityIntoDatabase()
        {
            const string someName = "someName";
            const string someSite = "PokerStars";

            IPlayerIdentity insertedIdentity = _sut.FindOrInsert(someName, someSite);

            var retrievedIdentity = ClearedSession.Get<PlayerIdentity>(insertedIdentity.Id);

            retrievedIdentity.ShouldBeEqualTo(insertedIdentity);
        }
        
        #endregion
    }
}