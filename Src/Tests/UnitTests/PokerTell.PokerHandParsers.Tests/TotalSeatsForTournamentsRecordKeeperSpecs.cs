namespace PokerTell.PokerHandParsers.Tests
{
    using System.Collections.Generic;

    using Machine.Specifications;

    // Resharper disable InconsistentNaming
    public abstract class TotalSeatsForTournamentsRecordKeeperSpecs
    {
        const ulong NewTournamentId = 1;

        static TotalSeatsForTournamentsRecordKeeperSut _sut;

        Establish specContext = () => _sut = new TotalSeatsForTournamentsRecordKeeperSut();

        [Subject(typeof(TotalSeatsForTournamentsRecordKeeper), "SetTotalSeatsRecordIfItIsOneFor")]
        public class when_dictionary_is_empty_and_totalseat_record_is_set : TotalSeatsForTournamentsRecordKeeperSpecs
        {
            const int newTotalSeats = 2;

            Because of = () => _sut.SetTotalSeatsRecordIfItIsOneFor(NewTournamentId, newTotalSeats);

            It should_add_the_new_total_seats_as_a_record_for_the_tournament_Id 
                = () => _sut.TotalSeatsForTournaments[NewTournamentId].ShouldEqual(newTotalSeats);
        }

        [Subject(typeof(TotalSeatsForTournamentsRecordKeeper), "SetTotalSeatsRecordIfItIsOneFor")]
        public class when_dictionary_contains_only_totalseats_for_other_tournament_and_totalseat_record_is_set :
            TotalSeatsForTournamentsRecordKeeperSpecs
        {
            const ulong otherTournamentId = 0;

            const int newTotalSeats = 2;

            Establish context = () => _sut.TotalSeatsForTournaments.Add(otherTournamentId, 9);

            Because of = () => _sut.SetTotalSeatsRecordIfItIsOneFor(NewTournamentId, newTotalSeats);

            It should_add_the_new_total_seats_as_a_record_for_the_tournament_Id 
                = () => _sut.TotalSeatsForTournaments[NewTournamentId].ShouldEqual(newTotalSeats);
        }

        [Subject(typeof(TotalSeatsForTournamentsRecordKeeper), "SetTotalSeatsRecordIfItIsOneFor")]
        public class when_dictionary_contains_smaller_totalseats_for_the_same_tournament_and_totalseat_record_is_set :
            TotalSeatsForTournamentsRecordKeeperSpecs
        {
            const int previousTotalSeats = 2;

            const int newTotalSeats = 6;

            Establish context = () => _sut.TotalSeatsForTournaments.Add(NewTournamentId, previousTotalSeats);

            Because of = () => _sut.SetTotalSeatsRecordIfItIsOneFor(NewTournamentId, newTotalSeats);

            It should_add_the_new_total_seats_as_a_record_for_the_tournament_Id 
                = () => _sut.TotalSeatsForTournaments[NewTournamentId].ShouldEqual(newTotalSeats);
        }

        [Subject(typeof(TotalSeatsForTournamentsRecordKeeper), "SetTotalSeatsRecordIfItIsOneFor")]
        public class when_dictionary_contains_larger_totalseats_for_the_same_tournament_and_totalseat_record_is_set :
            TotalSeatsForTournamentsRecordKeeperSpecs
        {
            const int previousTotalSeats = 9;

            const int newTotalSeats = 6;

            Establish context = () => _sut.TotalSeatsForTournaments.Add(NewTournamentId, previousTotalSeats);

            Because of = () => _sut.SetTotalSeatsRecordIfItIsOneFor(NewTournamentId, newTotalSeats);

            It should_keep_the_previous_total_seats_as_a_record_for_the_tournament_Id 
                = () => _sut.TotalSeatsForTournaments[NewTournamentId].ShouldEqual(previousTotalSeats);
        }


        [Subject(typeof(TotalSeatsForTournamentsRecordKeeper), "GetTotalSeatsRecordFor")]
        public class when_dictionary_is_empty_get_record_for_a_tournament : TotalSeatsForTournamentsRecordKeeperSpecs
        {
            const int newTotalSeats = 2;

            static int returnedTotalSeats;

            Because of = () => returnedTotalSeats = _sut.GetTotalSeatsRecordFor(NewTournamentId);

            It should_return_zero = () => returnedTotalSeats.ShouldEqual(0);
        }

        [Subject(typeof(TotalSeatsForTournamentsRecordKeeper), "GetTotalSeatsRecordFor")]
        public class when_dictionary_contains_only_another_tournament_get_record_for_a_tournament : TotalSeatsForTournamentsRecordKeeperSpecs
        {
            const ulong otherTournamentId = 0;

            static int returnedTotalSeats;

            Establish context = () => _sut.TotalSeatsForTournaments.Add(otherTournamentId, 0);

            Because of = () => returnedTotalSeats = _sut.GetTotalSeatsRecordFor(NewTournamentId);

            It should_return_zero = () => returnedTotalSeats.ShouldEqual(0);
        }

        [Subject(typeof(TotalSeatsForTournamentsRecordKeeper), "GetTotalSeatsRecordFor")]
        public class when_dictionary_contains_a_record_for_the_tournament_get_record_for_a_tournament : TotalSeatsForTournamentsRecordKeeperSpecs
        {
            static int returnedTotalSeats;

            const int newTotalSeats = 6;

            Establish context = () => _sut.TotalSeatsForTournaments.Add(NewTournamentId, newTotalSeats);

            Because of = () => returnedTotalSeats = _sut.GetTotalSeatsRecordFor(NewTournamentId);

            It should_return_the_total_seats_record_for_the_tournament = () => returnedTotalSeats.ShouldEqual(newTotalSeats);
        }
    }
    
    public class TotalSeatsForTournamentsRecordKeeperSut : TotalSeatsForTournamentsRecordKeeper
    {
        public IDictionary<ulong, int> TotalSeatsForTournaments
        {
            get { return _totalSeatsForTournaments; }
        }
    }
}