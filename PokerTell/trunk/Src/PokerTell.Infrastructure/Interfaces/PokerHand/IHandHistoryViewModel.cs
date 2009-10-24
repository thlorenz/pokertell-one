namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System;
    using System.Collections.Generic;

    using Tools.WPF.Interfaces;

    public interface IHandHistoryViewModel
    {
        string Ante { get; }

        string BigBlind { get; }

        string GameId { get; }

        string SmallBlind { get; }

        string TimeStamp { get; }

        string TotalPlayers { get; }

        string TournamentId { get; }

        bool IsTournament { get; }

        bool HasAnte { get; }

        IBoardViewModel Board { get; }

        Action<IPokerHandCondition> AdjustToConditionAction { get; }

        string Note { get; set; }

        IList<IHandHistoryRow> PlayerRows { get; }

        bool Visible { get; set; }

        bool ShowSelectOption { get; set; }

        bool IsSelected { get; set; }

        bool ShowPreflopFolds { set; }

        int SelectedRow { get; set; }

        IConvertedPokerHand Hand { get; }

        IHandHistoryViewModel UpdateWith(IConvertedPokerHand hand);

        IHandHistoryViewModel Initialize(bool showPreflopFolds);

        /// <summary>
        /// Selects Player with given name
        /// If name is null selection will be cleared
        /// </summary>
        /// <param name="playerName"></param>
        void SelectRowOfPlayer(string playerName);
    }
}