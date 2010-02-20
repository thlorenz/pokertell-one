namespace PokerTell.PokerHand.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Text;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.WPF.ViewModels;

    /// <summary>
    /// ViewModel to show data of a handhistorie
    /// By default show preflop folds is false, to change use InitializeWith(true)
    /// </summary>
    [Serializable]
    public class HandHistoryViewModel : NotifyPropertyChanged, IHandHistoryViewModel
    {
        protected Action<IPokerHandCondition> _adjustToConditionAction;

        [NonSerialized]
        IBoardViewModel _board;

        IConvertedPokerHand _hand;

        bool _isSelected;

        string _note;

        [NonSerialized]
        IList<IHandHistoryRow> _playerRows;

        int _selectedRow;

        bool _showPreflopFolds;

        bool _showSelectOption;

        bool _visible;

        public HandHistoryViewModel()
        {
            InitializeWith(false);
        }
        #region Properties

        public Action<IPokerHandCondition> AdjustToConditionAction
        {
            get
            {
                return _adjustToConditionAction ??
                       (_adjustToConditionAction = new Action<IPokerHandCondition>(AdjustToCondition));
            }
        }

        bool _showHandNote;

        public bool ShowHandNote
        {
            get { return _showHandNote; }
            set { _showHandNote = value; }
        }

        public string Ante
        {
            get { return Hand.Ante.ToString(); }
        }

        public string BigBlind
        {
            get { return Hand.BB.ToString(); }
        }

        public IBoardViewModel Board
        {
            get { return _board; }
        }

        public string GameId
        {
            get { return Hand.GameId.ToString(); }
        }

        public IConvertedPokerHand Hand
        {
            get { return _hand; }
        }

        public bool HasAnte
        {
            get { return Hand.Ante > 0; }
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
            get { return Hand.TournamentId > 0; }
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

        public IList<IHandHistoryRow> PlayerRows
        {
            get { return _playerRows; }
            private set { _playerRows = value; }
        }

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
            get { return _showPreflopFolds; }

            set
            {
                _showPreflopFolds = value;

                if (Hand != null)
                {
                    FillPlayerRows(Hand);
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
            get { return Hand.SB.ToString(); }
        }

        public string TimeStamp
        {
            get { return Hand.TimeStamp.ToString(); }
        }

        public string TotalPlayers
        {
            get { return Hand.TotalPlayers.ToString(); }
        }

        public string TournamentId
        {
            get { return Hand.TournamentId != 0 ? Hand.TournamentId.ToString() : string.Empty; }
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

        public IHandHistoryViewModel InitializeWith(bool showPreflopFolds)
        {
            CreateBoardAndPlayerRows();

            ShowPreflopFolds = showPreflopFolds;
            ShowSelectOption = false;
            SelectedRow = -1;
            IsSelected = false;
            Visible = true;

            return this;
        }

        void CreateBoardAndPlayerRows()
        {
            PlayerRows = new ObservableCollection<IHandHistoryRow>();
            _board = new BoardViewModel();
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
            Visible = condition.IsMetBy(Hand);
        }

        void FillPlayerRows(IConvertedPokerHand hand)
        {
            PlayerRows.Clear();

            foreach (var player in hand)
            {
                if (_showPreflopFolds || player[Streets.PreFlop][0].What != ActionTypes.F)
                {
                    PlayerRows.Add(new HandHistoryRow(player));
                }
            }
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(HandHistoryViewModel))
            {
                return false;
            }

            return Equals((HandHistoryViewModel)obj);
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            CreateBoardAndPlayerRows();
            if (_hand != null)
            {
                UpdateWith(_hand);
            }
        }

        public bool Equals(HandHistoryViewModel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.GetHashCode().Equals(GetHashCode());
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = _hand != null ? _hand.GetHashCode() : 0;
                result = (result * 397) ^ _isSelected.GetHashCode();
                result = (result * 397) ^ (_note != null ? _note.GetHashCode() : 0);
                result = (result * 397) ^ _visible.GetHashCode();
                result = (result * 397) ^ _showPreflopFolds.GetHashCode();
                result = (result * 397) ^ _showSelectOption.GetHashCode();
                result = (result * 397) ^ _selectedRow;
                return result;
            }
        }
    }
}