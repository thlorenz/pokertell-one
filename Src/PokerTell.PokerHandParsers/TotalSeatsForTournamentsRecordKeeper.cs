namespace PokerTell.PokerHandParsers
{
    using System.Collections.Generic;

    using Interfaces;

    /// <summary>
    /// Keeps track of the highest detected number of total seats for each tournament.
    /// There should only be one instance per application.
    /// Currently needed for FullTilt since it doesn't explicitly specify the number of total seats for tournaments.
    /// </summary>
    public class TotalSeatsForTournamentsRecordKeeper : ITotalSeatsForTournamentsRecordKeeper
    {
        protected readonly IDictionary<ulong, int> _totalSeatsForTournaments;

        public TotalSeatsForTournamentsRecordKeeper()
        {
            _totalSeatsForTournaments = new Dictionary<ulong, int>();
        }

        public int GetTotalSeatsRecordFor(ulong tournamentId)
        {
            return _totalSeatsForTournaments.ContainsKey(tournamentId) ? _totalSeatsForTournaments[tournamentId] : 0;
        }

        public ITotalSeatsForTournamentsRecordKeeper SetTotalSeatsRecordIfItIsOneFor(ulong tournamentId, int totalSeats)
        {
            if (!_totalSeatsForTournaments.ContainsKey(tournamentId) || _totalSeatsForTournaments[tournamentId] < totalSeats)
                _totalSeatsForTournaments[tournamentId] = totalSeats;

            return this;
        }
    }
}