namespace PokerTell.Repository.Database
{
    using System;
    using System.Data;
    using System.Reflection;

    using Infrastructure.Enumerations.DatabaseSetup;
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.DatabaseSetup;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using log4net;

    /// <summary>
    /// Contains all static methods to retrieve previouslyconverted PokerHands from a database
    /// </summary>
    public class ConvertedPokerHandRetriever : IConvertedPokerHandRetriever
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IConstructor<IConvertedPokerHand> _convertedHandMake;

        readonly IConstructor<IConvertedPokerPlayer> _convertedPlayerMake;

        readonly IPokerHandStringConverter _pokerHandStringConverter;

        IDataProvider _dataProvider;

        #endregion

        #region Constructors and Destructors

        public ConvertedPokerHandRetriever(
            IPokerHandStringConverter pokerHandStringConverter, 
            IConstructor<IConvertedPokerHand> convertedHandMake, 
            IConstructor<IConvertedPokerPlayer> convertedPlayerMake)
        {
            _convertedHandMake = convertedHandMake;
            _convertedPlayerMake = convertedPlayerMake;
            _pokerHandStringConverter = pokerHandStringConverter;
        }

        #endregion

        #region Public Methods

        public IConvertedPokerHand RetrieveHand(int handId)
        {
            IConvertedPokerHand convHand = CreateHandFromDatabase(handId);

            if (convHand != null)
            {
                ExtractPlayersFromDatabase(handId, convHand);
            }

            return convHand;
        }

        public IConvertedPokerHandRetriever Use(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            return this;
        }

        #endregion

        #region Methods

        static double ExtractBigBlind(IDataRecord dr)
        {
            if (dr == null)
            {
                throw new ArgumentNullException("dr");
            }

            double bigBlind;
            bool couldNotParse = !double.TryParse(dr[GameTable.bb.ToString()].ToString(), out bigBlind);

            if (couldNotParse)
            {
                Log.Debug("Illegal bb value in Database " + dr[GameTable.bb.ToString()]);
                bigBlind = 0;
            }

            return bigBlind;
        }

        static ulong ExtractGameId(IDataRecord dr)
        {
            ulong gameId;
            bool couldNotParse = !ulong.TryParse(dr[GameTable.gameid.ToString()].ToString(), out gameId);

            if (couldNotParse)
            {
                Log.Debug("Illegal gameid value in Database " + dr[GameTable.gameid.ToString()]);
                gameId = 0;
            }

            return gameId;
        }

        static void ExtractInPositionFields(IConvertedPokerPlayer convPlayer, IDataRecord dr)
        {
            convPlayer.InPosition[(int)Streets.Flop] = dr.GetInt32((int)ActionTable.inposflop);
            convPlayer.InPosition[(int)Streets.Turn] = dr.GetInt32((int)ActionTable.inposturn);
            convPlayer.InPosition[(int)Streets.River] = dr.GetInt32((int)ActionTable.inposriver);
        }

        static void ExtractNumberOfPlayersInEachRound(IConvertedPokerHand convHand, IDataRecord dr)
        {
            convHand.PlayersInRound[(int)Streets.Flop] = dr.GetInt32((int)GameTable.inflop);
            convHand.PlayersInRound[(int)Streets.Turn] = dr.GetInt32((int)GameTable.inturn);
            convHand.PlayersInRound[(int)Streets.River] = dr.GetInt32((int)GameTable.inriver);
        }

        static double ExtractSmallBlind(IDataRecord dr)
        {
            double smallBlind;
            bool couldNotParse = !double.TryParse(dr[GameTable.sb.ToString()].ToString(), out smallBlind);

            if (couldNotParse)
            {
                Log.Debug("Illegal sb value in Database " + dr[GameTable.sb.ToString()]);
                smallBlind = 0;
            }

            return smallBlind;
        }

        static int ExtractTotalPlayers(IDataRecord dr)
        {
            int totalPlayers;
            bool couldNotParse = !int.TryParse(dr[GameTable.totalplayers.ToString()].ToString(), out totalPlayers);

            if (couldNotParse)
            {
                Log.Debug("Illegal totalPlayers value in Database " + dr[GameTable.totalplayers.ToString()]);
                totalPlayers = 2;
            }

            return totalPlayers;
        }

        static ulong ExtractTournamentId(IDataRecord dr)
        {
            ulong tournamentId;
            bool couldNotParse = !ulong.TryParse(dr[GameTable.tournamentid.ToString()].ToString(), out tournamentId);

            if (couldNotParse)
            {
                Log.Debug("Illegal tournamentid value in Database " + dr[GameTable.tournamentid.ToString()]);
                tournamentId = 0;
            }

            return tournamentId;
        }

        IConvertedPokerHand CreateHandFromDatabase(int handId)
        {
            // Hand Header
            string query = string.Format(
                "SELECT * FROM {0} WHERE {1} = {2};", 
                Tables.gamehhd, 
                GameTable.identity, 
                handId);

            using (IDataReader dr = _dataProvider.ExecuteQuery(query))
            {
                if (dr.Read())
                {
                    try
                    {
                        IConvertedPokerHand convertedPokerHand = ExtractHandHeader(handId, dr);

                        ExtractNumberOfPlayersInEachRound(convertedPokerHand, dr);

                        ExtractSequencesForEachRound(convertedPokerHand, dr);

                        return convertedPokerHand;
                    }
                    catch (Exception excep)
                    {
                        Log.Error("Unexpected", excep);
                    }
                }
            }

            return null;
        }

        IConvertedPokerPlayer CreatePlayerFromDatabase(IDataRecord dr)
        {
            IConvertedPokerPlayer convPlayer = _convertedPlayerMake.New;

            convPlayer.MBefore = Convert.ToInt32(dr[ActionTable.m.ToString()].ToString());
            convPlayer.Position = Convert.ToInt32(dr[ActionTable.position.ToString()].ToString());
            convPlayer.Holecards = dr[ActionTable.cards.ToString()].ToString();
            convPlayer.PlayerId = long.Parse(dr[ActionTable.playerid.ToString()].ToString());

            return convPlayer;
        }

        IConvertedPokerHand ExtractHandHeader(int handId, IDataRecord dr)
        {
            DateTime timeStamp = DateTime.Parse(dr[(int)GameTable.timein].ToString());

            ulong gameId = ExtractGameId(dr);

            double bb = ExtractBigBlind(dr);

            double sb = ExtractSmallBlind(dr);

            int totalPlayers = ExtractTotalPlayers(dr);

            ulong tournamentId = ExtractTournamentId(dr);

            IConvertedPokerHand convertedHand = _convertedHandMake.New
                .InitializeWith(
                dr[GameTable.site.ToString()].ToString(), 
                gameId, 
                timeStamp, 
                bb, 
                sb, 
                totalPlayers);

            convertedHand.HandId = handId;

            convertedHand.Board = dr[GameTable.board.ToString()].ToString();

            convertedHand.TournamentId = tournamentId;

            convertedHand.TableName = dr[GameTable.tablename.ToString()].ToString();

            return convertedHand;
        }

        void ExtractNamesForAllPlayers(IConvertedPokerHand convHand)
        {
            foreach (IConvertedPokerPlayer convPlayer in convHand)
            {
                convPlayer.Name = GetPlayerNameFromId(convPlayer.PlayerId);
            }
        }

        IConvertedPokerPlayer ExtractPlayer(IDataRecord dr)
        {
            IConvertedPokerPlayer convPlayer = CreatePlayerFromDatabase(dr);

            ExtractRoundsAndSequences(convPlayer, dr);

            ExtractInPositionFields(convPlayer, dr);

            return convPlayer;
        }

        void ExtractPlayersFromDatabase(int handId, IConvertedPokerHand convertedHand)
        {
            string query = string.Format(
                "SELECT * FROM {0} WHERE {1} = {2};", 
                Tables.actionhhd, 
                ActionTable.handid, 
                handId);

            using (IDataReader dr = _dataProvider.ExecuteQuery(query))
            {
                // Iterate through all found players
                while (dr.Read())
                {
                    IConvertedPokerPlayer convPlayer = ExtractPlayer(dr);

                    convertedHand.AddPlayer(convPlayer);
                }
            }

            ExtractNamesForAllPlayers(convertedHand);

            convertedHand.SetStrategicPositionsForAllPlayers();
        }

        void ExtractRoundsAndSequences(IConvertedPokerPlayer convPlayer, IDataRecord dr)
        {
            IConvertedPokerRound convRound =
                _pokerHandStringConverter.ConvertedRoundFrom(dr[ActionTable.action0.ToString()].ToString());

            if (convRound != null && convRound.Count > 0)
            {
                convPlayer.Add(convRound);
                convPlayer.Sequence[0] = dr[ActionTable.sequence0.ToString()].ToString();

                // Flop
                convRound = _pokerHandStringConverter.ConvertedRoundFrom(dr[ActionTable.action1.ToString()].ToString());
                if (convRound != null && convRound.Count > 0)
                {
                    convPlayer.Add(convRound);
                    convPlayer.Sequence[1] = dr[ActionTable.sequence1.ToString()].ToString();

                    // Turn
                    convRound =
                        _pokerHandStringConverter.ConvertedRoundFrom(dr[ActionTable.action2.ToString()].ToString());
                    if (convRound != null && convRound.Count > 0)
                    {
                        convPlayer.Add(convRound);
                        convPlayer.Sequence[2] = dr[ActionTable.sequence2.ToString()].ToString();

                        // River
                        convRound =
                            _pokerHandStringConverter.ConvertedRoundFrom(dr[ActionTable.action3.ToString()].ToString());
                        if (convRound != null && convRound.Count > 0)
                        {
                            convPlayer.Add(convRound);
                            convPlayer.Sequence[3] = dr[ActionTable.sequence3.ToString()].ToString();
                        }
                    }
                }
            }
        }

        void ExtractSequencesForEachRound(IConvertedPokerHand convHand, IDataRecord dr)
        {
            if (dr.FieldCount > (int)GameTable.sequence0)
            {
                convHand.AddSequence(
                    _pokerHandStringConverter.ConvertedRoundFrom(dr[(int)GameTable.sequence0].ToString()));
            }

            if (dr.FieldCount > (int)GameTable.sequence1)
            {
                convHand.AddSequence(
                    _pokerHandStringConverter.ConvertedRoundFrom(dr[(int)GameTable.sequence1].ToString()));
            }

            if (dr.FieldCount > (int)GameTable.sequence2)
            {
                convHand.AddSequence(
                    _pokerHandStringConverter.ConvertedRoundFrom(dr[(int)GameTable.sequence2].ToString()));
            }

            if (dr.FieldCount > (int)GameTable.sequence3)
            {
                convHand.AddSequence(
                    _pokerHandStringConverter.ConvertedRoundFrom(dr[(int)GameTable.sequence3].ToString()));
            }
        }

        /// <summary>
        /// Gets the name of a player with a certain ID
        /// </summary>
        /// <param name="playerid">Player ID</param>
        /// <returns>Name of the player</returns>
        string GetPlayerNameFromId(long playerid)
        {
            try
            {
                string query = string.Format("SELECT * FROM {0} WHERE playerid = {1};", Tables.playerhhd, playerid);

                using (IDataReader dr = _dataProvider.ExecuteQuery(query))
                {
                    if (!dr.Read())
                    {
                        throw new Exception(
                            string.Format(
                                "Didn't find Player with ID {0} in GetPlayerNameFromId.", 
                                playerid));
                    }
                    
                    string nickname = dr[PlayerTable.nickname.ToString()].ToString();
                    return nickname;
                }
            }
            catch (Exception excep)
            {
                Log.Error("Unexpected -> returning ? as Playername", excep);
                return "?";
            }
        }

        #endregion
    }
}