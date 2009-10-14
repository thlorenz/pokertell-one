namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
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

        void UpdateWith(IConvertedPokerHand hand);
    }
}