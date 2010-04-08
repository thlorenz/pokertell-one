namespace PokerTell.Statistics.ViewModels
{
    using System;
    using System.Windows.Input;

    using Infrastructure.Interfaces;

    using PokerTell.Infrastructure.Interfaces.Statistics;

    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public class AnalyzablePokerPlayersFilterAdjustmentViewModel : NotifyPropertyChanged, IAnalyzablePokerPlayersFilterAdjustmentViewModel
    {
        public AnalyzablePokerPlayersFilterAdjustmentViewModel(IConstructor<IAnalyzablePokerPlayersFilterViewModel> filterViewModelMake)
        {
            _filterViewModelMake = filterViewModelMake;
        }

        ICommand _applyFilterToAllCommand;

        ICommand _applyFilterToPlayerCommand;

        Action<string, IAnalyzablePokerPlayersFilter> _applyTo;

        Action<IAnalyzablePokerPlayersFilter> _applyToAll;

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

        IAnalyzablePokerPlayersFilterViewModel _filter;

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

        readonly IConstructor<IAnalyzablePokerPlayersFilterViewModel> _filterViewModelMake;

        public string PlayerName
        {
            get { return _playerName; }
            protected set
            {
                _playerName = value;
                RaisePropertyChanged(() => PlayerName);
            }
        }

        public bool ShowApplyToAllCommand
        {
            get { return _applyToAll != null; }
        }

        public IAnalyzablePokerPlayersFilterAdjustmentViewModel InitializeWith(
            string playerName, 
            IAnalyzablePokerPlayersFilter currentFilter, 
            Action<string, IAnalyzablePokerPlayersFilter> applyTo, 
            Action<IAnalyzablePokerPlayersFilter> applyToAll)
        {
            PlayerName = playerName;

            // Need to create a new filter each time, to renew the binding and ensure that each player gets a separate one since the properties of the 
            // filter itself don't raise property changed
            Filter = _filterViewModelMake.New.InitializeWith(currentFilter);

            _applyTo = applyTo;
            _applyToAll = applyToAll;
            
            return this;
        }
    }
}