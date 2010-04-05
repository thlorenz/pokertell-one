namespace PokerTell.Statistics.ViewModels
{
    using System;
    using System.Windows.Input;

    using PokerTell.Infrastructure.Interfaces.Statistics;

    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public class AnalyzablePokerPlayersFilterAdjustmentViewModel : NotifyPropertyChanged, IAnalyzablePokerPlayersFilterAdjustmentViewModel
    {
        ICommand _applyFilterToAllCommand;

        ICommand _applyFilterToPlayerCommand;

        Action<string, IAnalyzablePokerPlayersFilter> _applyTo;

        Action<IAnalyzablePokerPlayersFilter> _applyToAll;

        IAnalyzablePokerPlayersFilterViewModel _filter;

        public ICommand ApplyFilterToAllCommand
        {
            get
            {
                return _applyFilterToAllCommand ?? (_applyFilterToAllCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => _applyToAll(Filter.CurrentFilter)
                    });
            }
        }

        public ICommand ApplyFilterToPlayerCommand
        {
            get
            {
                return _applyFilterToPlayerCommand ?? (_applyFilterToPlayerCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => _applyTo(PlayerName, Filter.CurrentFilter)
                    });
            }
        }

        public IAnalyzablePokerPlayersFilterViewModel Filter
        {
            get { return _filter; }
            protected set
            {
                _filter = value;
                RaisePropertyChanged(() => Filter);
            }
        }

        string _playerName;

        public string PlayerName
        {
            get { return _playerName; }
            protected set
            {
                _playerName = value;
                RaisePropertyChanged(() => PlayerName);
            }
        }

        public IAnalyzablePokerPlayersFilterAdjustmentViewModel InitializeWith(
            string playerName, 
            IAnalyzablePokerPlayersFilter currentFilter, 
            Action<string, IAnalyzablePokerPlayersFilter> applyTo, 
            Action<IAnalyzablePokerPlayersFilter> applyToAll)
        {
            PlayerName = playerName;
            Filter = new AnalyzablePokerPlayersFilterViewModel(currentFilter);
            _applyTo = applyTo;
            _applyToAll = applyToAll;

            return this;
        }
    }
}