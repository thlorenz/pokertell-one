namespace PokerTell.PokerHand.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using System.Windows.Input;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Microsoft.Practices.Composite.Presentation.Commands;

    using Tools.WPF.ViewModels;

    public class HandHistoryViewModel : ViewModel, IHandHistoryViewModel
    {
        #region Constants and Fields

        private readonly bool _showPreflopFolds;

        private IConvertedPokerHand _hand;

        bool _visible;

        ICommand _checkForConditionCommand;

        #endregion

        #region Constructors and Destructors

        public HandHistoryViewModel()
            : this(false)
        {
        }

        public HandHistoryViewModel(bool showPreflopFolds)
        {
            _showPreflopFolds = showPreflopFolds;

            PlayerRows = new ObservableCollection<HandHistoryRow>();
            Board = new BoardViewModel();
        }

        #endregion

        #region Properties

        public string Ante
        {
            get { return _hand.Ante.ToString(); }
        }

        public string BigBlind
        {
            get { return _hand.BB.ToString(); }
        }

        public string GameId
        {
            get { return _hand.GameId.ToString(); }
        }

        public IList<HandHistoryRow> PlayerRows { get; private set; }

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

        public bool IsTournament
        {
            get { return _hand.TournamentId > 0; }
        }

        public bool HasAnte
        {
            get { return _hand.Ante > 0;  }
        }

        public IBoardViewModel Board { get; private set; }

        public bool Visible
        {
            get 
            {
                return _visible;
            }

            set
            {
                _visible = value;
                RaisePropertyChanged(() => Visible);
            }
        }

        DelegateCommand<IPokerHandCondition> _adjustToConditionCommand;

        public DelegateCommand<IPokerHandCondition> AdjustToConditionCommand
        {
            get
            {
                if (_adjustToConditionCommand == null)
                {
                    _adjustToConditionCommand = new DelegateCommand<IPokerHandCondition>(AdjustToCondition);
                }

                return _adjustToConditionCommand;
            }
        }

        void AdjustToCondition(IPokerHandCondition condition)
        {
            Visible = condition.IsFullFilledBy(_hand);
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

        public void UpdateWith(IConvertedPokerHand hand)
        {
            _hand = hand;
           
            Board.UpdateWith(hand.Board);
            
            PlayerRows.Clear();

            foreach (IConvertedPokerPlayer player in hand)
            {
                if (_showPreflopFolds || player[Streets.PreFlop][0].What != ActionTypes.F)
                {
                    PlayerRows.Add(new HandHistoryRow(player));
                }
            }

            RaisePropertyChanged(() => TournamentId);
            RaisePropertyChanged(() => GameId);
            RaisePropertyChanged(() => BigBlind);
            RaisePropertyChanged(() => SmallBlind);
            RaisePropertyChanged(() => Ante);
            RaisePropertyChanged(() => TotalPlayers);
            RaisePropertyChanged(() => TimeStamp);
        }

        #endregion
    }
}