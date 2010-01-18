namespace PokerTell.Statistics.ViewModels
{
    using System;
    using System.Windows.Input;

    using PokerTell.Infrastructure.Interfaces.Statistics;

    using Tools.WPF;

    public class AnalyzablePokerPlayersFilterAdjustmentViewModel 
    {
        #region Constants and Fields

        Action<string, IAnalyzablePokerPlayersFilter> _applyTo;

        Action<IAnalyzablePokerPlayersFilter> _applyToAll;

        ICommand _applyFilterToAllCommand;

        ICommand _applyFilterToPlayerCommand;

        #endregion

        #region Constructors and Destructors

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

        void InitializeWith(string playerName, IAnalyzablePokerPlayersFilter currentFilter, Action<string, IAnalyzablePokerPlayersFilter> applyTo, Action<IAnalyzablePokerPlayersFilter> applyToAll)
        {
            PlayerName = playerName;
            Filter = new AnalyzablePokerPlayersFilterViewModel(currentFilter);
            _applyTo = applyTo;
            _applyToAll = applyToAll;
        }

        #endregion

        #region Properties

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

        #endregion
    }
}