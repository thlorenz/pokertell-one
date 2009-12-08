namespace PokerTell.PokerHand.Dao
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Analyzation;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.Repository;

    using NHibernate;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.Extensions;

    public class ConvertedPokerPlayerDao : IConvertedPokerPlayerDao
    {
        #region Constants and Fields

        ISession _session;

        readonly ISessionFactoryManager _sessionFactoryManager;

        #endregion

        ISession Session
        {
            get { return _session ?? (_session = _sessionFactoryManager.CurrentSession); }
        }

        #region Constructors and Destructors

        public ConvertedPokerPlayerDao(ISessionFactoryManager sessionFactoryManager)
        {
            _sessionFactoryManager = sessionFactoryManager;
        }

        #endregion

        public IEnumerable<IConvertedPokerPlayer> FindByPlayerIdentity(int playerIdentity)
        {
            const string query = "from ConvertedPokerPlayer player " +
                                 "where player.PlayerIdentity = :playerIdentity";
            return Session.CreateQuery(query)
                .SetInt32("playerIdentity", playerIdentity)
                .List<IConvertedPokerPlayer>();
        }

        public IEnumerable<IAnalyzablePokerPlayer> FindAnalyzablePlayersWith(int playerIdentity, long lastQueriedId)
        {
            const string queryString =
                "select " +
                "player.Id as item0, " +
                "player.ParentHand.Id as item1, " +
                "player.MBefore as item2, " +
                "player.StrategicPosition as item3, " +
                "player.InPositionPreFlop as item4, " +
                "player.InPositionFlop as item5, " +
                "player.InPositionTurn as item6, " +
                "player.InPositionRiver as item7, " +
                "player.SequencePreFlop as item8, " +
                "player.SequenceFlop as item9, " +
                "player.SequenceTurn as item10, " +
                "player.SequenceRiver as item11, " +
                "player.BetSizeIndexFlop as item12, " +
                "player.BetSizeIndexTurn as item13, " +
                "player.BetSizeIndexRiver as item14 " +
                "from ConvertedPokerPlayer player " +
                "where player.Id > :lastQueriedId " +
                "and player.PlayerIdentity = :playerIdentity";

            IList<object[]> result =
                Session.CreateQuery(queryString)
                    .SetInt32("playerIdentity", playerIdentity)
                    .SetInt64("lastQueriedId", lastQueriedId)
                    .List<object[]>();

            var analyzablePlayers = new List<IAnalyzablePokerPlayer>();
            foreach (object[] columns in result)
            {
                var analyzablePlayer = new AnalyzablePokerPlayer
                    {
                        Id = (long)columns[0],
                        HandId = (int)columns[1],
                        MBefore = (int)columns[2],
                        StrategicPosition = (StrategicPositions)columns[3],
                        InPosition =
                            new[] { (bool?)columns[4], (bool?)columns[5], (bool?)columns[6], (bool?)columns[7] },
                        ActionSequences = new[]
                            {
                                (ActionSequences)columns[8], (ActionSequences)columns[9], (ActionSequences)columns[10],
                                (ActionSequences)columns[11]
                            },
                        BetSizeIndexes = new[] { (int)columns[12], (int)columns[13], (int)columns[14] }
                    };

                analyzablePlayers.Add(analyzablePlayer);
            }

            return analyzablePlayers;
        }

        public IList UsingRawSqlQueryConvertedPokerPlayersWith(int playerIdentity)
        {
            string query =
                "SELECT "
                + "player.HandId as HandId2_0_, "
                + "player.Position as Position2_0_, "
                + "player.M as M2_0_,"
                + "player.StrategicPosition as Strategi7_2_0_, "
                + "player.InPositionPreFlop as InPositi8_2_0_, "
                + "player.InPositionFlop as InPositi9_2_0_, "
                + "player.InPositionTurn as InPosit10_2_0_, "
                + "player.InPositionRiver as InPosit11_2_0_, "
                + "player.ActionsPreflop, player.ActionsFlop, player.ActionsTurn, player.ActionsRiver,"
                + "player.SequencePreFlop as Sequenc16_2_0_, "
                + "player.SequenceFlop as Sequenc17_2_0_, "
                + "player.BetSizeIndexFlop as BetSize18_2_0_, "
                + "player.SequenceTurn as Sequenc19_2_0_, "
                + "player.BetSizeIndexTurn as BetSize20_2_0_, "
                + "player.SequenceRiver as Sequenc21_2_0_, "
                + "player.BetSizeIndexRiver as BetSize22_2_0_ "
                + "FROM ConvertedPokerPlayers player "
                + "WHERE player.PlayerIdentity = " + playerIdentity;

            return Session.CreateSQLQuery(query)
                .List();
        }
    }
}