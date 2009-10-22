namespace PokerTell.PokerHand.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Text;

    using log4net;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.WPF.ViewModels;

    public class HandHistoryViewModel : ViewModel, IHandHistoryViewModel
    {
        #region Constants and Fields

        protected Action<IPokerHandCondition> _adjustToConditionAction;

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        IConvertedPokerHand _hand;

        bool _isSelected;

        string _note;

        int _selectedRow;

        bool _showPreflopFolds;

        bool _showSelectOption;

        bool _visible;

        #endregion

        #region Constructors and Destructors

        public HandHistoryViewModel()
            : this(false)
        {
        }

        public HandHistoryViewModel(bool showPreflopFolds)
        {
            Initialize(showPreflopFolds);
        }

        #endregion

        #region Properties

        public Action<IPokerHandCondition> AdjustToConditionAction
        {
            get
            {
                if (_adjustToConditionAction == null)
                {
                    _adjustToConditionAction = new Action<IPokerHandCondition>(AdjustToCondition);
                }

                return _adjustToConditionAction;
            }
        }

        public string Ante
        {
            get { return _hand.Ante.ToString(); }
        }

        public string BigBlind
        {
            get { return _hand.BB.ToString(); }
        }

        public IBoardViewModel Board { get; private set; }

        public string GameId
        {
            get { return _hand.GameId.ToString(); }
        }

        public bool HasAnte
        {
            get { return _hand.Ante > 0; }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }

        public bool IsTournament
        {
            get { return _hand.TournamentId > 0; }
        }

        public string Note
        {
            get { return _note; }
            set
            {
                _note = value;
                RaisePropertyChanged(() => Note);
            }
        }

        public IList<IHandHistoryRow> PlayerRows { get; private set; }

        public int SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                RaisePropertyChanged(() => SelectedRow);
            }
        }

        public bool ShowPreflopFolds
        {
            set
            {
                _showPreflopFolds = value;

                if (_hand != null)
                {
                    FillPlayerRows(_hand);
                }
            }
        }

        public bool ShowSelectOption
        {
            get { return _showSelectOption; }
            set
            {
                _showSelectOption = value;
                RaisePropertyChanged(() => ShowSelectOption);
            }
        }

        public string SmallBlind
        {
            get { return _hand.SB.ToString(); }
        }

        public string TimeStamp
        {
            get { return _hand.TimeStamp.ToString(); }
        }

        public string TotalPlayers
        {
            get { return _hand.TotalPlayers.ToString(); }
        }

        public string TournamentId
        {
            get { return _hand.TournamentId != 0 ? _hand.TournamentId.ToString() : string.Empty; }
        }

        public bool Visible
        {
            get { return _visible; }

            set
            {
                _visible = value;
                RaisePropertyChanged(() => Visible);
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("HandHistoryViewModel");
            foreach (HandHistoryRow playerRow in PlayerRows)
            {
                sb.AppendLine(playerRow.ToString());
            }

            return sb.ToString();
        }

        #endregion

        #region Implemented Interfaces

        #region IHandHistoryViewModel

        public IHandHistoryViewModel Initialize(bool showPreflopFolds)
        {
            ShowPreflopFolds = showPreflopFolds;

            PlayerRows = new ObservableCollection<IHandHistoryRow>();
            Board = new BoardViewModel();

            ShowSelectOption = false;
            SelectedRow = -1;
            IsSelected = false;
            Visible = true;
            return this;
        }

        /// <summary>
        /// Selects Player with given name
        /// If name is null selection will be cleared
        /// </summary>
        /// <param name="playerName"></param>
        public void SelectRowOfPlayer(string playerName)
        {
            // If player not found or null set to -1 which will tell DataGrid to select no row
            int rowToSelect = -1;
           
            if (playerName != null)
            {
                for (int i = 0; i < PlayerRows.Count; i++)
                {
                    if (PlayerRows[i].PlayerName == playerName)
                    {
                        rowToSelect = i;
                        break;
                    }
                }
            }

            SelectedRow = rowToSelect;
        }

        public IHandHistoryViewModel UpdateWith(IConvertedPokerHand hand)
        {
            _hand = hand;

            Board.UpdateWith(hand.Board);

            FillPlayerRows(hand);

            RaisePropertyChanged(() => TournamentId);
            RaisePropertyChanged(() => GameId);
            RaisePropertyChanged(() => BigBlind);
            RaisePropertyChanged(() => SmallBlind);
            RaisePropertyChanged(() => Ante);
            RaisePropertyChanged(() => TotalPlayers);
            RaisePropertyChanged(() => TimeStamp);

            return this;
        }

        #endregion

        #endregion

        #region Methods

        void AdjustToCondition(IPokerHandCondition condition)
        {
            Visible = condition.IsMetBy(_hand);
        }

        void FillPlayerRows(IConvertedPokerHand hand)
        {
            PlayerRows.Clear();

            foreach (IConvertedPokerPlayer player in hand)
            {
                if (_showPreflopFolds || player[Streets.PreFlop][0].What != ActionTypes.F)
                {
                    PlayerRows.Add(new HandHistoryRow(player));
                }
            }
        }

        #endregion
    }
}