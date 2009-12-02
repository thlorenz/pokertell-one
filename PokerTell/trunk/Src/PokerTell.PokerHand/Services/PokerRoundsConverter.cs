namespace PokerTell.PokerHand.Services
{
    using System;
    using System.Reflection;

    using log4net;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    public class PokerRoundsConverter : IPokerRoundsConverter
    {
        #region Constants and Fields

        protected int ActionCount;

        protected bool FoundAction;

        protected double Pot;

        protected IConvertedPokerRound SequenceForCurrentRound;

        protected string SequenceSoFar;

        protected double ToCall;

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IPokerActionConverter _actionConverter;

        readonly IConstructor<IConvertedPokerActionWithId> _convertedActionWithId;

        readonly IConstructor<IConvertedPokerRound> _convertedRound;

        IAquiredPokerHand _aquiredHand;

        IConvertedPokerHand _convertedHand;

        #endregion

        #region Constructors and Destructors

        public PokerRoundsConverter(
            IConstructor<IConvertedPokerActionWithId> convertedActionWithId, 
            IConstructor<IConvertedPokerRound> convertedRound, 
            IPokerActionConverter actionConverter)
        {
            _convertedActionWithId = convertedActionWithId;
            _convertedRound = convertedRound;
            _actionConverter = actionConverter;
        }

        #endregion

        #region Properties

        bool HeadsUp
        {
            get { return _aquiredHand.Players.Count == 2; }
        }

        bool ThreeOrMore
        {
            get { return _aquiredHand.Players.Count >= 3; }
        }

        #endregion

        #region Implemented Interfaces

        #region IPokerRoundsConverter

        public virtual IConvertedPokerHand ConvertFlopTurnAndRiver()
        {
            for (Streets street = Streets.Flop; street <= Streets.River; street++)
            {
                ActionCount = 0;
                SequenceSoFar = string.Empty;

                SequenceForCurrentRound = _convertedRound.New;

                // Do as long as we still find an action of ANY Player
                do
                {
                    FoundAction = false;
                    int fromPlayer;
                    int toPlayer;
                    
                    if (HeadsUp)
                    {
                        // BigBlind acts before button (small Blind)
                        fromPlayer = 1;
                        toPlayer = 0;
                        ProcessPlayersForRound(street, fromPlayer, toPlayer);
                    }
                    else if (ThreeOrMore)
                    {
                        fromPlayer = 0;
                        toPlayer = _aquiredHand.Players.Count - 1;
                        ProcessPlayersForRound(street, fromPlayer, toPlayer);
                    }

                    ActionCount++;
                }
                while (FoundAction);

                _convertedHand.Sequences[(int)street] = SequenceForCurrentRound;
            }

            return _convertedHand;
        }

        public virtual IPokerRoundsConverter ConvertPreflop()
        {
            // At this point all posting has been removed
            SequenceForCurrentRound = _convertedRound.New;
            ActionCount = 0;

            // Do as long as we still find an action of ANY Player
            do
            {
                FoundAction = false;
                SequenceSoFar = string.Empty;

                int fromPlayer;
                int toPlayer;
               
                if (HeadsUp)
                {
                    fromPlayer = 0;
                    toPlayer = 1;

                    ProcessPlayersForRound(Streets.PreFlop, fromPlayer, toPlayer);
                }
                else if (ThreeOrMore)
                {
                    // Preflop Blinds are last

                    // Everyone but blinds
                    fromPlayer = 2;
                    toPlayer = _aquiredHand.Players.Count - 1;

                    ProcessPlayersForRound(Streets.PreFlop, fromPlayer, toPlayer);

                    // Blinds
                    fromPlayer = 0;
                    toPlayer = 1;

                    ProcessPlayersForRound(Streets.PreFlop, fromPlayer, toPlayer);
                }

                ActionCount++;
            }
            while (FoundAction);

            _convertedHand.Sequences[(int)Streets.PreFlop] = SequenceForCurrentRound;

            return this;
        }

        public IPokerRoundsConverter InitializeWith(
            IAquiredPokerHand aquiredHand, IConvertedPokerHand convertedHand, double pot, double toCall)
        {
            ToCall = toCall;
            Pot = pot;
            _convertedHand = convertedHand;
            _aquiredHand = aquiredHand;

            return this;
        }

        #endregion

        #endregion

        #region Methods

        protected virtual void ProcessPlayersForRound(Streets street, int fromPlayer, int toPlayer)
        {
            if (fromPlayer <= toPlayer)
            {
                for (int playerIndex = fromPlayer; playerIndex <= toPlayer; playerIndex++)
                {
                    // Check if non empty bettinground for that player exists in the current round
                    IAquiredPokerPlayer aquiredPlayer = _aquiredHand[playerIndex];

                    if (aquiredPlayer.Count > (int)street
                        && aquiredPlayer[street].Actions.Count > ActionCount)
                    {
                        ProcessRound(street, aquiredPlayer[street], _convertedHand[playerIndex]);
                    }
                }
            }
            else
            {
                // Headsup
                for (int playerIndex = fromPlayer; playerIndex >= toPlayer; playerIndex--)
                {
                    // Check if non empty bettinground for that player exists in the current round
                    IAquiredPokerPlayer aquiredPlayer = _aquiredHand[playerIndex];

                    if (aquiredPlayer.Count > (int)street
                        && aquiredPlayer[street].Actions.Count > ActionCount)
                    {
                        ProcessRound(street, aquiredPlayer[street], _convertedHand[playerIndex]);
                    }
                }
            }
        }

        protected virtual void ProcessRound(
            Streets street, IAquiredPokerRound aquiredPokerRound, IConvertedPokerPlayer convertedPlayer)
        {
            try
            {
                FoundAction = true;

                // Ignore returned chips (they didn't add or substract from the pot
                // and this action was not "done" by the player
                if (aquiredPokerRound[ActionCount].What != ActionTypes.U)
                {
                    IConvertedPokerAction convertedAction =
                        _actionConverter.Convert(
                            aquiredPokerRound[ActionCount], ref Pot, ref ToCall, _aquiredHand.TotalPot);

                    SequenceForCurrentRound.Add(
                        _convertedActionWithId.New.InitializeWith(convertedAction, convertedPlayer.Position));

                    SetActionSequenceAndAddActionTo(convertedPlayer, convertedAction, street);
                }
            }
            catch (Exception excep)
            {
                Log.Error("Unhandled", excep);
            }
        }

        void SetActionSequenceAndAddActionTo(
            IConvertedPokerPlayer convertedPlayer, IConvertedPokerAction convertedAction, Streets street)
        {
            convertedPlayer.SetActionSequenceString(ref SequenceSoFar, convertedAction, street);

            if (convertedPlayer.Rounds != null && convertedPlayer.Rounds.Count < (int)street + 1)
            {
                convertedPlayer.Add();
            }

            convertedPlayer[street].Add(convertedAction);
        }

        #endregion
    }
}