namespace PokerTell.Repository.Database
{
    using System;
    using System.Data;
    using System.Reflection;
    using System.Text;

    using Interfaces;

    using log4net;

    using PokerTell.Infrastructure.Enumerations.DatabaseSetup;
    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Provides all methods needed to insert a converted Pokerhand into the database
    /// </summary>
    internal class ConvertedPokerHandInserter : IConvertedPokerHandInserter
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        IDataProvider _dataProvider;

        readonly IPokerHandStringConverter _pokerHandStringConverter;

        readonly IDatabaseUtility _databaseUtility;

        #endregion

        #region Constructors and Destructors

        public ConvertedPokerHandInserter(IPokerHandStringConverter pokerHandStringConverter, IDatabaseUtility databaseUtility)
        {
            _databaseUtility = databaseUtility;
            _pokerHandStringConverter = pokerHandStringConverter;
        }

        #endregion

        #region Public Methods

        public IConvertedPokerHandInserter Use(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            return this;
        }

        public void Insert(IConvertedPokerHand convHand)
        {
            // Don't insert if null or hand contains no players
            if ((convHand != null) && (convHand.Players.Count > 0))
            {
                try
                {
                    if (!HandIsAlreadyInDatabase(convHand.Site, convHand.GameId))
                    {
                        bool successfullyUpdatedGameTable = UpdateGameTable(convHand);

                        if (successfullyUpdatedGameTable)
                        {
                            int handId = _databaseUtility
                                .Use(_dataProvider)
                                .GetIdentityOfLastInsertedHand();

                            // dd all active players in the hand to the Action Table
                            foreach (IConvertedPokerPlayer convPlayer in convHand)
                            {
                                int playerId = GetPlayerID(convHand, convPlayer);
                                UpdateActionTable(handId, playerId, convPlayer);
                            }
                        }
                    }
                }
                catch (Exception excep)
                {
                    Log.Error("Unexpected", excep);
                }
            }
        }

        #endregion

        #region Methods

        static string CreateUpdateGameTableNonQueryString(string paramPlaceHolder)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO {0} (", Tables.gamehhd);

            sb.AppendFormat("{0}, ", GameTable.identity);

            sb.AppendFormat("{0}, ", GameTable.sessionid);

            sb.AppendFormat("{0}, ", GameTable.gameid);
            sb.AppendFormat("{0}, ", GameTable.tournamentid);

            sb.AppendFormat("{0}, ", GameTable.tablename);
            sb.AppendFormat("{0}, ", GameTable.site);

            sb.AppendFormat("{0}, ", GameTable.bb);
            sb.AppendFormat("{0}, ", GameTable.sb);

            sb.AppendFormat("{0}, ", GameTable.timein);

            sb.AppendFormat("{0}, ", GameTable.totalplayers);
            sb.AppendFormat("{0}, ", GameTable.activeplayers);

            sb.AppendFormat("{0}, ", GameTable.inflop);
            sb.AppendFormat("{0}, ", GameTable.inturn);
            sb.AppendFormat("{0}, ", GameTable.inriver);

            sb.AppendFormat("{0}, ", GameTable.board);

            sb.AppendFormat("{0}, ", GameTable.sequence0);
            sb.AppendFormat("{0}, ", GameTable.sequence1);
            sb.AppendFormat("{0}, ", GameTable.sequence2);
            sb.AppendFormat("{0}) ", GameTable.sequence3);

            sb.AppendLine("VALUES (");

            sb.AppendFormat("{0}{1}, ", paramPlaceHolder, GameTable.identity);

            sb.AppendFormat("{0}{1}, ", paramPlaceHolder, GameTable.sessionid);

            sb.AppendFormat("{0}{1}, ", paramPlaceHolder, GameTable.gameid);
            sb.AppendFormat("{0}{1}, ", paramPlaceHolder, GameTable.tournamentid);

            sb.AppendFormat("{0}{1}, ", paramPlaceHolder, GameTable.tablename);
            sb.AppendFormat("{0}{1}, ", paramPlaceHolder, GameTable.site);

            sb.AppendFormat("{0}{1}, ", paramPlaceHolder, GameTable.bb);
            sb.AppendFormat("{0}{1}, ", paramPlaceHolder, GameTable.sb);

            sb.AppendFormat("{0}{1}, ", paramPlaceHolder, GameTable.timein);

            sb.AppendFormat("{0}{1}, ", paramPlaceHolder, GameTable.totalplayers);
            sb.AppendFormat("{0}{1}, ", paramPlaceHolder, GameTable.activeplayers);

            sb.AppendFormat("{0}{1}, ", paramPlaceHolder, GameTable.inflop);
            sb.AppendFormat("{0}{1}, ", paramPlaceHolder, GameTable.inturn);
            sb.AppendFormat("{0}{1}, ", paramPlaceHolder, GameTable.inriver);

            sb.AppendFormat("{0}{1}, ", paramPlaceHolder, GameTable.board);

            sb.AppendFormat("{0}{1}, ", paramPlaceHolder, GameTable.sequence0);
            sb.AppendFormat("{0}{1}, ", paramPlaceHolder, GameTable.sequence1);
            sb.AppendFormat("{0}{1}, ", paramPlaceHolder, GameTable.sequence2);
            sb.AppendFormat("{0}{1});", paramPlaceHolder, GameTable.sequence3);

            return sb.ToString();
        }

        IDbCommand CreateUpdateGameTableCommand(string nonQuery, IConvertedPokerHand convHand)
        {
            string[] theSequenceStrings = PrepareSequences(convHand);

            IDbCommand cmd = _dataProvider.Connection.CreateCommand();

            cmd.CommandText = nonQuery;

            // identity
            IDataParameter param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.identity);
            param.Value = null;
            param.DbType = DbType.Int32;
            cmd.Parameters.Add(param);

            // sessionid
            param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.sessionid);
            param.Value = 1;
            param.DbType = DbType.Int32;
            cmd.Parameters.Add(param);

            // gameid
            param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.gameid);
            param.Value = convHand.GameId;
            param.DbType = DbType.UInt64;
            cmd.Parameters.Add(param);

            // tournamentid
            param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.tournamentid);
            param.Value = convHand.TournamentId;
            param.DbType = DbType.UInt64;
            cmd.Parameters.Add(param);

            // tablename
            param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.tablename);
            param.Value = convHand.TableName;
            param.DbType = DbType.StringFixedLength;
            cmd.Parameters.Add(param);

            // site
            param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.site);
            param.Value = convHand.Site;
            param.DbType = DbType.StringFixedLength;
            cmd.Parameters.Add(param);

            // bb
            param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.bb);
            param.Value = convHand.BB;
            param.DbType = DbType.Double;
            cmd.Parameters.Add(param);

            // sb
            param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.sb);
            param.Value = convHand.SB;
            param.DbType = DbType.Double;
            cmd.Parameters.Add(param);

            // timein
            param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.timein);
            param.Value = convHand.TimeStamp;
            param.DbType = DbType.DateTime;
            cmd.Parameters.Add(param);

            // totalplayers
            param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.totalplayers);
            param.Value = convHand.TotalPlayers;
            param.DbType = DbType.Int32;
            cmd.Parameters.Add(param);

            // activeplayers
            param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.activeplayers);
            param.Value = convHand.Players.Count;
            param.DbType = DbType.Int32;
            cmd.Parameters.Add(param);

            // inflop
            param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.inflop);
            param.Value = convHand.PlayersInRound[(int)Streets.Flop];
            param.DbType = DbType.Int32;
            cmd.Parameters.Add(param);

            // inturn
            param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.inturn);
            param.Value = convHand.PlayersInRound[(int)Streets.Turn];
            param.DbType = DbType.Int32;
            cmd.Parameters.Add(param);

            // inriver
            param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.inriver);
            param.Value = convHand.PlayersInRound[(int)Streets.River];
            param.DbType = DbType.Int32;
            cmd.Parameters.Add(param);

            // board
            param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.board);
            param.Value = convHand.Board;
            param.DbType = DbType.StringFixedLength;
            cmd.Parameters.Add(param);

            // sequence0
            param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.sequence0);
            param.Value = theSequenceStrings[(int)Streets.PreFlop];
            param.DbType = DbType.String;
            cmd.Parameters.Add(param);

            // sequence1
            param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.sequence1);
            param.Value = theSequenceStrings[(int)Streets.Flop];
            param.DbType = DbType.String;
            cmd.Parameters.Add(param);

            // sequence2
            param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.sequence2);
            param.Value = theSequenceStrings[(int)Streets.Turn];
            param.DbType = DbType.String;
            cmd.Parameters.Add(param);

            // sequence3
            param = cmd.CreateParameter();
            param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, GameTable.sequence3);
            param.Value = theSequenceStrings[(int)Streets.River];
            param.DbType = DbType.String;
            cmd.Parameters.Add(param);

            return cmd;
        }

        int GetIdentityOfLastEnteredPlayer()
        {
            string query = string.Format("SELECT max({0}) FROM {1};", PlayerTable.playerid, Tables.playerhhd);

            using (IDataReader dr = _dataProvider.ExecuteQuery(query))
            {
                int playerID = dr.Read() ? dr.GetInt32(0) : 0;

                return playerID;
            }
        }

        int GetPlayerID(IConvertedPokerHand convHand, IConvertedPokerPlayer convPlayer)
        {
            string query = string.Format(
                "SELECT {2} FROM {1} " + "WHERE {3} = {0}{3} " + "AND {4} = {0}{4};", 
                _dataProvider.ParameterPlaceHolder, 
                Tables.playerhhd, 
                PlayerTable.playerid, 
                PlayerTable.nickname, 
                PlayerTable.site);

            try
            {
                int playerID = -1;

                using (IDbCommand cmd = _dataProvider.GetCommand())
                {
                    cmd.CommandText = query;

                    // nickname
                    IDataParameter param = cmd.CreateParameter();
                    param.ParameterName = string.Format(
                        "{0}{1}", _dataProvider.ParameterPlaceHolder, PlayerTable.nickname);
                    param.Value = convPlayer.Name;
                    param.DbType = DbType.StringFixedLength;
                    cmd.Parameters.Add(param);

                    // Site
                    param = cmd.CreateParameter();
                    param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, PlayerTable.site);
                    param.Value = convHand.Site;
                    param.DbType = DbType.StringFixedLength;
                    cmd.Parameters.Add(param);

                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            playerID = dr.GetInt32((int)PlayerTable.playerid);
                        }
                    }
                }

                if (playerID == -1)
                {
                    bool successfullyEnteredPlayer = UpdatePlayerTable(convHand, convPlayer);

                    if (successfullyEnteredPlayer)
                    {
                        playerID = GetIdentityOfLastEnteredPlayer();
                    }
                }

                return playerID;
            }
            catch (Exception excep)
            {
                Log.Error("Unexpected", excep);
                Log.Debug("Query: " + query);
                Console.WriteLine(excep.ToString());
                return 0;
            }
        }

        bool HandIsAlreadyInDatabase(string site, ulong gameId)
        {
            try
            {
                return _databaseUtility
                    .Use(_dataProvider)
                    .GetHandIdForHandWith(gameId, site) != null;
            }
            catch (Exception excep)
            {
                Log.Error("Unexpected", excep);
                return true;
            }
        }

        string[] PrepareActionsAndSequences(IConvertedPokerPlayer convPlayer, out string[] strAction)
        {
            // Prepare actions and sequences to be stored
            strAction = new string[(int)Streets.River + 1];
            var strSequence = new string[(int)Streets.River + 1];
            try
            {
                for (var iRound = (int)Streets.PreFlop; iRound < convPlayer.Count; iRound++)
                {
                    strAction[iRound] = _pokerHandStringConverter.BuildSqlStringFrom(convPlayer[iRound]);
                    strSequence[iRound] = convPlayer.Sequence[iRound];
                }

                // Fill remaining strings with string.empty to avoid null reference
                for (int iRound = convPlayer.Count; iRound <= (int)Streets.River; iRound++)
                {
                    strAction[iRound] = string.Empty;
                    strSequence[iRound] = string.Empty;
                }
            }
            catch (Exception excep)
            {
                Log.Error("Unexpected", excep);
            }

            return strSequence;
        }

        string[] PrepareSequences(IConvertedPokerHand convHand)
        {
            var theSequenceStrings = new string[4];

            for (int i = 0; i < convHand.Sequences.Count; i++)
            {
                theSequenceStrings[i] = _pokerHandStringConverter.BuildSqlStringFrom(convHand.Sequences[i]);
            }

            for (int i = convHand.Sequences.Count; i < 4; i++)
            {
                theSequenceStrings[i] = null;
            }

            return theSequenceStrings;
        }

        void UpdateActionTable(int handID, int playerID, IConvertedPokerPlayer convPlayer)
        {
            string[] strAction;
            string[] strSequence = PrepareActionsAndSequences(convPlayer, out strAction);

            string nonQuery =
                string.Format(
                    "INSERT INTO {1} " + "( {2}, {3}, {4}, {5}, {6}, " + "{7}, {8}, {9}, {10}, {11}," +
                    "{12}, {13}, {14}, {15}, {16}," + " {17}, {18}, {19} )" + "VALUES " +
                    "( {0}{2}, {0}{3}, {0}{4}, {0}{5}, {0}{6}, " + "{0}{7}, {0}{8}, {0}{9}, {0}{10}, {0}{11}," +
                    "{0}{12}, {0}{13}, {0}{14}, {0}{15}, {0}{16}," + " {0}{17}, {0}{18}, {0}{19} );", 
                    _dataProvider.ParameterPlaceHolder, 
                    Tables.actionhhd, 
                    ActionTable.playerid, 
                    ActionTable.handid, 
                    ActionTable.m, 
                    ActionTable.cards, 
                    ActionTable.position, 
                    ActionTable.strategicposition, 
                    ActionTable.inposflop, 
                    ActionTable.inposturn, 
                    ActionTable.inposriver, 
                    ActionTable.raiseinfrontpreflopfrompos, 
                    ActionTable.action0, 
                    ActionTable.action1, 
                    ActionTable.action2, 
                    ActionTable.action3, 
                    ActionTable.sequence0, 
                    ActionTable.sequence1, 
                    ActionTable.sequence2, 
                    ActionTable.sequence3);

            try
            {
                using (IDbCommand cmd = _dataProvider.GetCommand())
                {
                    cmd.CommandText = nonQuery;

                    // PlayerId
                    IDataParameter param = cmd.CreateParameter();
                    param.ParameterName = string.Format(
                        "{0}{1}", _dataProvider.ParameterPlaceHolder, ActionTable.playerid);
                    param.Value = playerID;
                    param.DbType = DbType.Int32;
                    cmd.Parameters.Add(param);

                    // handID
                    param = cmd.CreateParameter();
                    param.ParameterName = string.Format(
                        "{0}{1}", _dataProvider.ParameterPlaceHolder, ActionTable.handid);
                    param.Value = handID;
                    param.DbType = DbType.Int32;
                    cmd.Parameters.Add(param);

                    // m
                    param = cmd.CreateParameter();
                    param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, ActionTable.m);
                    param.Value = convPlayer.MBefore;
                    param.DbType = DbType.Int32;
                    cmd.Parameters.Add(param);

                    // Cards
                    param = cmd.CreateParameter();
                    param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, ActionTable.cards);
                    param.Value = convPlayer.Holecards;
                    param.DbType = DbType.StringFixedLength;
                    cmd.Parameters.Add(param);

                    // Position
                    param = cmd.CreateParameter();
                    param.ParameterName = string.Format(
                        "{0}{1}", _dataProvider.ParameterPlaceHolder, ActionTable.position);
                    param.Value = convPlayer.Position;
                    param.DbType = DbType.Int32;
                    cmd.Parameters.Add(param);

                    // StrategicPosition
                    param = cmd.CreateParameter();
                    param.ParameterName = string.Format(
                        "{0}{1}", _dataProvider.ParameterPlaceHolder, ActionTable.strategicposition);
                    param.Value = (int)convPlayer.StrategicPosition;
                    param.DbType = DbType.Int32;
                    cmd.Parameters.Add(param);

                    // InPosition Flop
                    param = cmd.CreateParameter();
                    param.ParameterName = string.Format(
                        "{0}{1}", _dataProvider.ParameterPlaceHolder, ActionTable.inposflop);
                    param.Value = convPlayer.InPosition[(int)Streets.Flop];
                    param.DbType = DbType.Int32;
                    cmd.Parameters.Add(param);

                    // InPosition Turn
                    param = cmd.CreateParameter();
                    param.ParameterName = string.Format(
                        "{0}{1}", _dataProvider.ParameterPlaceHolder, ActionTable.inposturn);
                    param.Value = convPlayer.InPosition[(int)Streets.Turn];
                    param.DbType = DbType.Int32;
                    cmd.Parameters.Add(param);

                    // InPosition River
                    param = cmd.CreateParameter();
                    param.ParameterName = string.Format(
                        "{0}{1}", _dataProvider.ParameterPlaceHolder, ActionTable.inposriver);
                    param.Value = convPlayer.InPosition[(int)Streets.River];
                    param.DbType = DbType.Int32;
                    cmd.Parameters.Add(param);

                    // Raiseinfrontpreflop
                    param = cmd.CreateParameter();
                    param.ParameterName = string.Format(
                        "{0}{1}", 
                        _dataProvider.ParameterPlaceHolder, 
                        ActionTable.raiseinfrontpreflopfrompos);
                    param.Value = convPlayer.PreflopRaiseInFrontPos;
                    param.DbType = DbType.Int32;
                    cmd.Parameters.Add(param);

                    // action0
                    param = cmd.CreateParameter();
                    param.ParameterName = string.Format(
                        "{0}{1}", _dataProvider.ParameterPlaceHolder, ActionTable.action0);
                    param.Value = strAction[(int)Streets.PreFlop];
                    param.DbType = DbType.String;
                    cmd.Parameters.Add(param);

                    // action1
                    param = cmd.CreateParameter();
                    param.ParameterName = string.Format(
                        "{0}{1}", _dataProvider.ParameterPlaceHolder, ActionTable.action1);
                    param.Value = strAction[(int)Streets.Flop];
                    param.DbType = DbType.String;
                    cmd.Parameters.Add(param);

                    // action2
                    param = cmd.CreateParameter();
                    param.ParameterName = string.Format(
                        "{0}{1}", _dataProvider.ParameterPlaceHolder, ActionTable.action2);
                    param.Value = strAction[(int)Streets.Turn];
                    param.DbType = DbType.String;
                    cmd.Parameters.Add(param);

                    // action3
                    param = cmd.CreateParameter();
                    param.ParameterName = string.Format(
                        "{0}{1}", _dataProvider.ParameterPlaceHolder, ActionTable.action3);
                    param.Value = strAction[(int)Streets.River];
                    param.DbType = DbType.String;
                    cmd.Parameters.Add(param);

                    // sequence0
                    param = cmd.CreateParameter();
                    param.ParameterName = string.Format(
                        "{0}{1}", _dataProvider.ParameterPlaceHolder, ActionTable.sequence0);
                    param.Value = strSequence[(int)Streets.PreFlop];
                    param.DbType = DbType.String;
                    cmd.Parameters.Add(param);

                    // sequence1
                    param = cmd.CreateParameter();
                    param.ParameterName = string.Format(
                        "{0}{1}", _dataProvider.ParameterPlaceHolder, ActionTable.sequence1);
                    param.Value = strSequence[(int)Streets.Flop];
                    param.DbType = DbType.String;
                    cmd.Parameters.Add(param);

                    // sequence2
                    param = cmd.CreateParameter();
                    param.ParameterName = string.Format(
                        "{0}{1}", _dataProvider.ParameterPlaceHolder, ActionTable.sequence2);
                    param.Value = strSequence[(int)Streets.Turn];
                    param.DbType = DbType.String;
                    cmd.Parameters.Add(param);

                    // sequence3
                    param = cmd.CreateParameter();
                    param.ParameterName = string.Format(
                        "{0}{1}", _dataProvider.ParameterPlaceHolder, ActionTable.sequence3);
                    param.Value = strSequence[(int)Streets.River];
                    param.DbType = DbType.String;
                    cmd.Parameters.Add(param);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception excep)
            {
                Log.Error("Unexpected", excep);
                Log.Debug(nonQuery);
            }
        }

        bool UpdateGameTable(IConvertedPokerHand convHand)
        {
            string nonQuery = CreateUpdateGameTableNonQueryString(_dataProvider.ParameterPlaceHolder);

            try
            {
                using (IDbCommand cmd = CreateUpdateGameTableCommand(nonQuery, convHand))
                {
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception excep)
            {
                Log.Error("Unexpected", excep);
                Log.Debug("Query: " + nonQuery);
                return false;
            }
        }

        bool UpdatePlayerTable(IConvertedPokerHand convHand, IConvertedPokerPlayer convPlayer)
        {
            string nonQuery =
                string.Format(
                    "INSERT INTO {1} " + "( {2}, {3}, {4} )" + "VALUES" + "( null, {0}{3}, {0}{4} );", 
                    _dataProvider.ParameterPlaceHolder, 
                    Tables.playerhhd, 
                    PlayerTable.playerid, 
                    PlayerTable.site, 
                    PlayerTable.nickname);

            using (IDbCommand cmd = _dataProvider.GetCommand())
            {
                cmd.CommandText = nonQuery;

                // Site
                IDataParameter param = cmd.CreateParameter();
                param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, PlayerTable.site);
                param.Value = convHand.Site;
                param.DbType = DbType.StringFixedLength;
                cmd.Parameters.Add(param);

                // nickname
                param = cmd.CreateParameter();
                param.ParameterName = string.Format("{0}{1}", _dataProvider.ParameterPlaceHolder, PlayerTable.nickname);
                param.Value = convPlayer.Name;
                param.DbType = DbType.StringFixedLength;
                cmd.Parameters.Add(param);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion
    }
}