namespace PokerTell.PokerHandParsers.Interfaces
{
    using Infrastructure.Interfaces;

    public interface ITotalSeatsForTournamentsRecordKeeper : IFluentInterface
    {
        int GetTotalSeatsRecordFor(ulong tournamentId);

        ITotalSeatsForTournamentsRecordKeeper SetTotalSeatsRecordIfItIsOneFor(ulong tournamentId, int totalSeats);
    }
}