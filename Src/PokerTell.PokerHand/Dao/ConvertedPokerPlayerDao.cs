namespace PokerTell.PokerHand.Dao
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using NHibernate;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.PokerHand.Analyzation;

    public class ConvertedPokerPlayerDao : IConvertedPokerPlayerDao
    {
        const string FindAnalyzablePokerPlayer = "FindAnalyzablePokerPlayer";

        readonly ISessionFactoryManager _sessionFactoryManager;

        public ConvertedPokerPlayerDao(ISessionFactoryManager sessionFactoryManager)
        {
            _sessionFactoryManager = sessionFactoryManager;
        }

        ISession Session
        {
            get { return _sessionFactoryManager.CurrentSession; }
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

        public IEnumerable<IAnalyzablePokerPlayer> FindAnalyzablePlayersWith(int playerIdentity, long lastQueriedId)
        {
            return
                Session.GetNamedQuery(FindAnalyzablePokerPlayer)
                    .SetInt32("playerIdentity", playerIdentity)
                    .SetInt64("lastQueriedId", lastQueriedId)
                    .List<IAnalyzablePokerPlayer>();
        }

        public IEnumerable<IAnalyzablePokerPlayer> FindAnalyzablePlayersWithLegacy(
            int playerIdentity, long lastQueriedId)
        {
            const string queryString =
                "select " +
                "player.Id as item0, " +
                "player.ParentHand.Id as item1, " +
                "player.MBefore as item2, " +
                "player.Position as item3, " +
                "player.StrategicPosition as item4, " +
                "player.InPositionPreFlop as item5, " +
                "player.InPositionFlop as item6, " +
                "player.InPositionTurn as item7, " +
                "player.InPositionRiver as item9, " +
                "player.SequencePreFlop as item9, " +
                "player.SequenceFlop as item10, " +
                "player.SequenceTurn as item11, " +
                "player.SequenceRiver as item12, " +
                "player.BetSizeIndexFlop as item13, " +
                "player.BetSizeIndexTurn as item14, " +
                "player.BetSizeIndexRiver as item15, " +
                "player.ParentHand.BB as item16, " +
                "player.ParentHand.Ante as item17, " +
                "player.ParentHand.TimeStamp as item18, " +
                "player.ParentHand.TotalPlayers as item19, " +
                "player.ParentHand.SequencePreFlop as item20, " +
                "player.ParentHand.SequenceFlop as item21, " +
                "player.ParentHand.SequenceTurn as item22, " +
                "player.ParentHand.SequenceRiver as item23 " +
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
                        StrategicPosition = (StrategicPositions)columns[4], 
                        InPosition =
                            new[] { (bool?)columns[5], (bool?)columns[6], (bool?)columns[7], (bool?)columns[8] }, 
                        ActionSequences = new[]
                            {
                                (ActionSequences)columns[9], (ActionSequences)columns[10], (ActionSequences)columns[11], 
                                (ActionSequences)columns[12]
                            }, 
                        BetSizeIndexes = new[] { (int)columns[13], (int)columns[14], (int)columns[15], }, 
                        BB = (double)columns[16], 
                        Ante = (double)columns[17], 
                        TimeStamp = (DateTime)columns[18]
                    };

                analyzablePlayers.Add(analyzablePlayer);
            }

            return analyzablePlayers;
        }

        public IEnumerable<IConvertedPokerPlayer> FindByPlayerIdentity(int playerIdentity)
        {
            const string query = "from ConvertedPokerPlayer player " +
                                 "where player.PlayerIdentity = :playerIdentity";
            return Session.CreateQuery(query)
                .SetInt32("playerIdentity", playerIdentity)
                .List<IConvertedPokerPlayer>();
        }
    }
}