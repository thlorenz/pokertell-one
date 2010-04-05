namespace PokerTell.Statistics.ViewModels
{
    using System;
    using System.Windows.Input;

    using Interfaces;

    using PokerTell.Infrastructure.Interfaces.Statistics;

    using Tools.WPF;

    public class AnalyzablePokerPlayersFilterAdjustmentViewModel : IAnalyzablePokerPlayersFilterAdjustmentViewModel
    {
        ICommand _applyFilterToAllCommand;

        ICommand _applyFilterToPlayerCommand;

        Action<string, IAnalyzablePokerPlayersFilter> _applyTo;

        Action<IAnalyzablePokerPlayersFilter> _applyToAll;

        public AnalyzablePokerPlayersFilterAdjustmentViewModel()
        {
        }

        public AnalyzablePokerPlayersFilterAdjustmentViewModel(
            string playerName, 
            IAnalyzablePokerPlayersFilter currentFilter, 
            Action<string, IAnalyzablePokerPlayersFilter> applyTo, 
            Action<IAnalyzablePokerPlayersFilter> applyToAll)
        {
            InitializeWith(playerName, currentFilter, applyTo, applyToAll);
        }

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

        public IAnalyzablePokerPlayersFilterViewModel Filter { get; protected set; }

        public string PlayerName { get; protected set; }

        public IAnalyzablePokerPlayersFilterAdjustmentViewModel InitializeWith(string playerName, IAnalyzablePokerPlayersFilter currentFilter, Action<string, IAnalyzablePokerPlayersFilter> applyTo, Action<IAnalyzablePokerPlayersFilter> applyToAll)
        {
            PlayerName = playerName;
            Filter = new AnalyzablePokerPlayersFilterViewModel(currentFilter);
            _applyTo = applyTo;
            _applyToAll = applyToAll;

            return this;
        }
    }
}