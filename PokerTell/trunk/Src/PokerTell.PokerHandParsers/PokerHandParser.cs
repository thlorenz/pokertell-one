namespace PokerTell.PokerHandParsers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.PokerHandParsers;

    using log4net;

    public abstract class PokerHandParser : IPokerHandParser
    {
        #region Constants and Fields

        protected readonly IConstructor<IAquiredPokerAction> _aquiredActionMake;

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IConstructor<IAquiredPokerHand> _aquiredHandMake;

        readonly IConstructor<IAquiredPokerPlayer> _aquiredPlayerMake;

        readonly IConstructor<IAquiredPokerRound> _aquiredRoundMake;

        string _handHistory;

        #endregion

        #region Constructors and Destructors

        public PokerHandParser(
            IConstructor<IAquiredPokerHand> aquiredHandMake,
            IConstructor<IAquiredPokerPlayer> aquiredPlayerMake,
            IConstructor<IAquiredPokerRound> aquiredRoundMake,
            IConstructor<IAquiredPokerAction> aquiredActionMake)
        {
            _aquiredActionMake = aquiredActionMake;
            _aquiredRoundMake = aquiredRoundMake;
            _aquiredPlayerMake = aquiredPlayerMake;
            _aquiredHandMake = aquiredHandMake;
        }

        #endregion

        #region Properties

        public IAquiredPokerHand AquiredPokerHand { get; private set; }

        public bool IsValid { get; private set; }

        public bool LogVerbose { get; set; }

        protected AnteParser AnteParser;

        protected BlindsParser BlindsParser;

        protected BoardParser BoardParser;

        protected HandHeaderParser HandHeaderParser;

        protected HoleCardsParser HoleCardsParser;

        protected PlayerActionsParser PlayerActionsParser;

        protected PlayerSeatsParser PlayerSeatsParser;

        protected string Site;

        protected SmallBlindSeatNumberParser SmallBlindSeatNumberParser;

        protected StreetsParser StreetsParser;

        protected TableNameParser TableNameParser;

        protected TimeStampParser TimeStampParser;

        protected TotalPotParser TotalPotParser;

        protected TotalSeatsParser TotalSeatsParser;

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Site;
        }

        #endregion

        #region Implemented Interfaces

        #region IPokerHandParser

        public IDictionary<ulong, string> ExtractSeparateHandHistories(string handHistories)
        {
            var gamesFoundInHandHistories = new Dictionary<ulong, string>();

            IList<HandHeaderParser.HeaderMatchInformation> headerMatches =
                HandHeaderParser.FindAllHeaders(handHistories);

            ExtractAllHandHistoriesExceptLast(handHistories, headerMatches, gamesFoundInHandHistories);

            ExtractLastHandHistory(handHistories, headerMatches, gamesFoundInHandHistories);

            return gamesFoundInHandHistories;
        }

        public IPokerHandParser ParseHand(string handHistory)
        {
            _handHistory = handHistory;

            IsValid =
                HandHeaderParser.Parse(handHistory).IsValid &&
                PlayerSeatsParser.Parse(handHistory).IsValid &&
                StreetsParser.Parse(handHistory).IsValid;

            if (IsValid)
            {
                InitializeAquiredHand();

                ParseHandInfo();

                int smallBlindSeatNumber = ParseSmallBlindSeatNumber();

                ParsePlayers(smallBlindSeatNumber);
            }

            return this;
        }

        public bool RecognizesHandHistoriesIn(string histories)
        {
            return HandHeaderParser.Parse(histories).IsValid;
        }

        #endregion

        #endregion

        #region Methods

        static void ExtractAllHandHistoriesExceptLast(
            string handHistories,
            IList<HandHeaderParser.HeaderMatchInformation> headerMatches,
            IDictionary<ulong, string> gamesFoundInHandHistories)
        {
            for (int i = 0; i < headerMatches.Count - 1; i++)
            {
                int currentHeaderIndex = headerMatches[i].HeaderMatch.Index;
                int nextHeaderIndex = headerMatches[i + 1].HeaderMatch.Index;
                int handHistoryLength = nextHeaderIndex - currentHeaderIndex;
                string handHistoryString = handHistories.Substring(currentHeaderIndex, handHistoryLength);
                ulong gameId = headerMatches[i].GameId;

                if (!gamesFoundInHandHistories.ContainsKey(gameId))
                {
                    gamesFoundInHandHistories.Add(gameId, handHistoryString);
                }
            }
        }

        static void ExtractLastHandHistory(
            string handHistories,
            IList<HandHeaderParser.HeaderMatchInformation> headerMatches,
            IDictionary<ulong, string> gamesFoundInHandHistories)
        {
            int lastHeaderIndex = headerMatches[headerMatches.Count - 1].HeaderMatch.Index;
            string lastHandHistoryString = handHistories.Substring(lastHeaderIndex);

            ulong lastGameId = headerMatches[headerMatches.Count - 1].GameId;
            if (!gamesFoundInHandHistories.ContainsKey(lastGameId))
            {
                gamesFoundInHandHistories.Add(lastGameId, lastHandHistoryString);
            }
        }

        int AddPlayersToHandAndGetSmallBlindPosition(int smallBlindSeatNumber)
        {
            int smallBlindPosition = 0;
            int relativeSeatNumber = 0;

            foreach (KeyValuePair<int, PlayerSeatsParser.PlayerData> playerSeat in PlayerSeatsParser.PlayerSeats)
            {
                IAquiredPokerPlayer aquiredPlayer = _aquiredPlayerMake.New
                    .InitializeWith(playerSeat.Value.Name, playerSeat.Value.Stack);

                aquiredPlayer.RelativeSeatNumber = relativeSeatNumber;
                aquiredPlayer.SeatNumber = playerSeat.Key;

                if (aquiredPlayer.SeatNumber == smallBlindSeatNumber)
                {
                    smallBlindPosition = aquiredPlayer.RelativeSeatNumber;
                }

                AquiredPokerHand.AddPlayer(aquiredPlayer);

                relativeSeatNumber++;
            }
            return smallBlindPosition;
        }

        IAquiredPokerRound GetPlayerActionsFor(string streetHistory, string playerName)
        {
            IAquiredPokerRound aquiredRound = _aquiredRoundMake.New;
            foreach (
                IAquiredPokerAction action in PlayerActionsParser.Parse(streetHistory, playerName).PlayerActions)
            {
                aquiredRound.Add(action);
            }

            return aquiredRound;
        }

        void InitializeAquiredHand()
        {
            DateTime timeStamp;
            if (TimeStampParser.Parse(_handHistory).IsValid)
            {
                timeStamp = TimeStampParser.TimeStamp;
            }
            else
            {
                Log.Debug("TimeStamp Parser failed to find timestamp -> setting to DateTime.Now");
                timeStamp = DateTime.Now;
            }

            double smallBlind;
            double bigBlind;

            if (BlindsParser.Parse(_handHistory).IsValid)
            {
                smallBlind = BlindsParser.SmallBlind;
                bigBlind = BlindsParser.BigBlind;
            }
            else
            {
                LogParsingError("Blinds Parser failed to find blinds - -> setting to 1.0, 1.0");
                smallBlind = 1.0;
                bigBlind = 1.0;
            }

            int totalPlayers = PlayerSeatsParser.PlayerSeats.Count;

            AquiredPokerHand = _aquiredHandMake.New
                .InitializeWith(Site, HandHeaderParser.GameId, timeStamp, bigBlind, smallBlind, totalPlayers);

            AquiredPokerHand.TournamentId = HandHeaderParser.IsTournament
                                                ? HandHeaderParser.TournamentId
                                                : 0;
        }

        void LogParsingError(string message)
        {
            var sb = new StringBuilder();

            sb.AppendLine(message);

            if (LogVerbose)
            {
                sb.AppendLine(_handHistory);
            }
            else
            {
                sb.AppendFormat(
                    "Site: {0} GameId: {1} TimeStamp: {2} ", Site, HandHeaderParser.GameId, TimeStampParser.TimeStamp);
            }

            Log.Debug(sb);
        }

        void ParseActionsFor(IAquiredPokerPlayer aquiredPlayer)
        {
            IAquiredPokerRound aquiredRound = GetPlayerActionsFor(StreetsParser.Preflop, aquiredPlayer.Name);

            if (aquiredRound.Actions.Count > 0)
            {
                aquiredPlayer.AddRound(aquiredRound);

                if (StreetsParser.HasFlop)
                {
                    ParseFlopTurnAndRiver(aquiredPlayer);
                }
            }
        }

        void ParseAnte()
        {
            AquiredPokerHand.Ante = AnteParser.Parse(_handHistory).IsValid
                                        ? AnteParser.Ante
                                        : 0;
        }

        void ParseBoard()
        {
            AquiredPokerHand.Board = BoardParser.Parse(_handHistory).IsValid
                                         ? BoardParser.Board
                                         : string.Empty;
        }

        void ParseFlopTurnAndRiver(IAquiredPokerPlayer aquiredPlayer)
        {
            IAquiredPokerRound aquiredRound = GetPlayerActionsFor(StreetsParser.Flop, aquiredPlayer.Name);
            if (aquiredRound.Actions.Count > 0)
            {
                aquiredPlayer.AddRound(aquiredRound);

                if (StreetsParser.HasTurn)
                {
                    ParseTurnAndRiver(aquiredPlayer);
                }
            }
        }

        void ParseHandInfo()
        {
            ParseAnte();
            ParseBoard();
            ParseTableName();
            ParseTotalPot();
            ParseTotalSeats();
        }

        void ParsePlayers(int smallBlindSeatNumber)
        {
            int smallBlindPosition = AddPlayersToHandAndGetSmallBlindPosition(smallBlindSeatNumber);

            SetPlayersPositionsRelativeTo(smallBlindPosition);

            ParsePlayersHoleCards();

            ParsePlayersActions();

            AquiredPokerHand.SortPlayersByPosition();
        }

        void ParsePlayersActions()
        {
            foreach (IAquiredPokerPlayer aquiredPlayer in AquiredPokerHand.Players)
            {
                ParseActionsFor(aquiredPlayer);
            }
        }

        void ParsePlayersHoleCards()
        {
            foreach (IAquiredPokerPlayer aquiredPlayer in AquiredPokerHand.Players)
            {
                aquiredPlayer.Holecards =
                    HoleCardsParser.Parse(_handHistory, aquiredPlayer.Name).Holecards;
            }
        }

        void ParseRiver(IAquiredPokerPlayer aquiredPlayer)
        {
            IAquiredPokerRound aquiredRound = GetPlayerActionsFor(StreetsParser.River, aquiredPlayer.Name);
            if (aquiredRound.Actions.Count > 0)
            {
                aquiredPlayer.AddRound(aquiredRound);
            }
        }

        int ParseSmallBlindSeatNumber()
        {
            int smallBlindSeatNumber;
            if (SmallBlindSeatNumberParser.Parse(_handHistory).IsValid)
            {
                smallBlindSeatNumber = SmallBlindSeatNumberParser.SmallBlindSeatNumber;
            }
            else
            {
                LogParsingError("SmallBlindSeatNumber Parser failed to find small blind seat -> setting to 1");
                smallBlindSeatNumber = 1;
            }
            return smallBlindSeatNumber;
        }

        void ParseTableName()
        {
            if (TableNameParser.Parse(_handHistory).IsValid)
            {
                AquiredPokerHand.TableName = TableNameParser.TableName;
            }
            else
            {
                LogParsingError("TableName Parser failed to find TableName -> setting to string.Empty");
                AquiredPokerHand.TableName = string.Empty;
            }
        }

        void ParseTotalPot()
        {
            if (TotalPotParser.Parse(_handHistory).IsValid)
            {
                AquiredPokerHand.TotalPot = TotalPotParser.TotalPot;
            }
            else
            {
                LogParsingError("TotalPot Parser failed to find total pot -> setting to BB + SB");
                AquiredPokerHand.TotalPot = AquiredPokerHand.BB + AquiredPokerHand.SB;
            }
        }

        void ParseTotalSeats()
        {
            if (TotalSeatsParser.Parse(_handHistory).IsValid)
            {
                AquiredPokerHand.TotalSeats = TotalSeatsParser.TotalSeats;
            }
            else
            {
                LogParsingError("TotalSeatsParser Parser failed to find total seats -> setting to 9");
                AquiredPokerHand.TotalSeats = 9;
            }
        }

        void ParseTurnAndRiver(IAquiredPokerPlayer aquiredPlayer)
        {
            IAquiredPokerRound aquiredRound = GetPlayerActionsFor(StreetsParser.Turn, aquiredPlayer.Name);
            if (aquiredRound.Actions.Count > 0)
            {
                aquiredPlayer.AddRound(aquiredRound);

                if (StreetsParser.HasRiver)
                {
                    ParseRiver(aquiredPlayer);
                }
            }
        }

        void SetPlayersPositionsRelativeTo(int smallBlindPosition)
        {
            foreach (IAquiredPokerPlayer aquiredPlayer in AquiredPokerHand.Players)
            {
                aquiredPlayer.SetPosition(smallBlindPosition, AquiredPokerHand.TotalPlayers);
            }
        }

        #endregion
    }
}