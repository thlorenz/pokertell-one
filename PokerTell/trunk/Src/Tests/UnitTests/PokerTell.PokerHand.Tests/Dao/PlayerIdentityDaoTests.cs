namespace PokerTell.PokerHand.Tests.Dao
{
    using Base;

    using Infrastructure.Interfaces.PokerHand;

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

            _sut = new PlayerIdentityDao(_session);
        }

        [Test]
        public void GetByName_DatabaseEmpty_ReturnsNull()
        {
            IPlayerIdentity returnedIdentity = _sut.GetPlayerIdentityFor("someName", "someSite");

            returnedIdentity.IsNull();
        }

        [Test]
        public void GetByNameAndSite_DatabaseContainsName_ReturnsIdentityWithThatNameAndSite()
        {
            const string someName = "someName";
            const string someSite = "PokerStars";
            IPlayerIdentity playerIdentity = new PlayerIdentity(someName, someSite);
            _session.SaveOrUpdate(playerIdentity);

            FlushAndClearSession();

            IPlayerIdentity returnedIdentity = _sut.GetPlayerIdentityFor(someName, someSite);

            returnedIdentity.IsEqualTo(playerIdentity);
        }

        [Test]
        public void GetOrInsert_PlayerIdentityIsInDatabase_AssignsPlayerIdentityIdFromDatabase()
        {
            const string someName = "someName";
            const string someSite = "PokerStars";
            var playerIdentity = new PlayerIdentity(someName, someSite);

            IPlayerIdentity insertedIdentity = _sut.Insert(playerIdentity);

            FlushAndClearSession();

            IPlayerIdentity samePlayerIdentity = _sut.GetOrInsert(someName, someSite);

            samePlayerIdentity.Id.IsEqualTo(insertedIdentity.Id);
        }

        [Test]
        public void GetOrInsert_PlayerIdentityNotInDatabase_InsertsPlayerIdentityIntoDatabase()
        {
            const string someName = "someName";
            const string someSite = "PokerStars";

            IPlayerIdentity insertedIdentity = _sut.GetOrInsert(someName, someSite);

            object retrievedIdentity = ClearedSession.Get<PlayerIdentity>(insertedIdentity.Id);

            retrievedIdentity.IsEqualTo(insertedIdentity);
        }
        
        #endregion
    }
}