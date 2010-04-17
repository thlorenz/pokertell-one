namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    using Interfaces.Parsers;

    using Machine.Specifications;

    using Moq;

    using PokerTell.PokerHandParsers.FullTiltPoker;

    using It = Machine.Specifications.It;

    /// <summary>
    /// Deals with the special cases for FullTilt TotalSeats detection not covered in the general tests.
    /// Namely it deals with variations in the CashGame max Seats indications and with the fact that Tournaments have no max indication
    /// at all except for a HeadsUp Tag in some Sit'n Gos
    /// </summary>
    // Resharper disable InconsistentNaming
    public abstract class TotalSeatsParserExtraSpecs
    {
        static IFullTiltPokerTotalSeatsParser _sut;

        static Mock<IFullTiltPokerPlayerSeatsParser> _playerSeatsParser_Mock;

        Establish specContext = () => {
            _playerSeatsParser_Mock = new Mock<IFullTiltPokerPlayerSeatsParser>();
            _playerSeatsParser_Mock
                .Setup(psp => psp.Parse(Moq.It.IsAny<string>()))
                .Returns(_playerSeatsParser_Mock.Object);

            _sut = new TotalSeatsParser(_playerSeatsParser_Mock.Object);
        };

        public class Ctx_ValidTournament_No_RecordSet : TotalSeatsParserExtraSpecs
        {
            protected const string ValidTournamentHandHistory = "SomeValidTournamentHandHistory ";

            Establish context = () => {
                _sut.IsTournament = true;
            };
        }

        [Subject(typeof(TotalSeatsParser), "CashTables")]
        public class when_parsing___Table_Spur_Paren_6_max_comma_deep_Paren___ : TotalSeatsParserExtraSpecs
        {
            const string headerPart = "Game #15568531231: Table Spur (6 max, deep) -";

            Because of = () => _sut.Parse(headerPart);

            It should_be_valid = () => _sut.IsValid.ShouldBeTrue();

            It should_detect_6_total_seats = () => _sut.TotalSeats.ShouldEqual(6);
        }

        [Subject(typeof(TotalSeatsParser), "CashTables")]
        public class when_parsing___Table_Ash_Paren_deep_6_Paren__ : TotalSeatsParserExtraSpecs
        {
            const string headerPart = "Game #8455423700: Table Ash (deep 6) -";

            Because of = () => _sut.Parse(headerPart);

            It should_be_valid = () => _sut.IsValid.ShouldBeTrue();

            It should_detect_6_total_seats = () => _sut.TotalSeats.ShouldEqual(6);
        }

        [Subject(typeof(TotalSeatsParser), "TournamentTables")]
        public class when_parsing_a_valid_tournament_with_no_previous_record_and_highest_seat_7 : Ctx_ValidTournament_No_RecordSet
        {
            const bool playerSeatsAreValid = true;

            const int highestSeatNumber = 7;

            Establish context = () => {
                _playerSeatsParser_Mock
                    .SetupGet(psp => psp.IsValid)
                    .Returns(playerSeatsAreValid);

                _playerSeatsParser_Mock
                    .SetupGet(psp => psp.HighestSeatNumber)
                    .Returns(highestSeatNumber);
            };

            Because of = () => _sut.Parse(ValidTournamentHandHistory);

            It should_be_valid = () => _sut.IsValid.ShouldBeTrue();

            It should_detect_9_total_seats = () => _sut.TotalSeats.ShouldEqual(9);
        }

        [Subject(typeof(TotalSeatsParser), "TournamentTables")]
        public class when_parsing_a_valid_tournament_with_no_previous_record_and_highest_seat_5 : Ctx_ValidTournament_No_RecordSet
        {
            const bool playerSeatsAreValid = true;

            const int highestSeatNumber = 5;

            Establish context = () => {
                _playerSeatsParser_Mock
                    .SetupGet(psp => psp.IsValid)
                    .Returns(playerSeatsAreValid);

                _playerSeatsParser_Mock
                    .SetupGet(psp => psp.HighestSeatNumber)
                    .Returns(highestSeatNumber);
            };

            Because of = () => _sut.Parse(ValidTournamentHandHistory);

            It should_be_valid = () => _sut.IsValid.ShouldBeTrue();

            It should_detect_6_total_seats = () => _sut.TotalSeats.ShouldEqual(6);
        }

        [Subject(typeof(TotalSeatsParser), "TournamentTables")]
        public class when_parsing_a_valid_tournament_with_no_previous_record_and_highest_seat_2 : Ctx_ValidTournament_No_RecordSet
        {
            const bool playerSeatsAreValid = true;

            const int highestSeatNumber = 2;

            Establish context = () => {
                _playerSeatsParser_Mock
                    .SetupGet(psp => psp.IsValid)
                    .Returns(playerSeatsAreValid);

                _playerSeatsParser_Mock
                    .SetupGet(psp => psp.HighestSeatNumber)
                    .Returns(highestSeatNumber);
            };

            Because of = () => _sut.Parse(ValidTournamentHandHistory);

            It should_be_valid = () => _sut.IsValid.ShouldBeTrue();

            It should_detect_2_total_seats = () => _sut.TotalSeats.ShouldEqual(2);
        }

        [Subject(typeof(TotalSeatsParser), "TournamentTables")]
        public class when_parsing_an_invalid_tournament_with_no_previous_record_and_highest_seat_7 : Ctx_ValidTournament_No_RecordSet
        {
            const bool playerSeatsAreValid = false;

            const int highestSeatNumber = 7;

            Establish context = () => {
                _playerSeatsParser_Mock
                    .SetupGet(psp => psp.IsValid)
                    .Returns(playerSeatsAreValid);

                _playerSeatsParser_Mock
                    .SetupGet(psp => psp.HighestSeatNumber)
                    .Returns(highestSeatNumber);
            };

            Because of = () => _sut.Parse(ValidTournamentHandHistory);

            It should_not_be_valid = () => _sut.IsValid.ShouldBeFalse();

            It should_leave_TotalSeats_as_0 = () => _sut.TotalSeats.ShouldEqual(0);
        }

        [Subject(typeof(TotalSeatsParser), "TournamentTables")]
        public class when_parsing_a_valid_tournament_with_previous_record_6_and_highest_seat_7 : Ctx_ValidTournament_No_RecordSet
        {
            const bool playerSeatsAreValid = true;

            const int highestSeatNumber = 7;

            const int totalSeatsRecord = 6;

            Establish context = () => {
                _playerSeatsParser_Mock
                    .SetupGet(psp => psp.IsValid)
                    .Returns(playerSeatsAreValid);

                _playerSeatsParser_Mock
                    .SetupGet(psp => psp.HighestSeatNumber)
                    .Returns(highestSeatNumber);

                _sut.TotalSeatsRecord = totalSeatsRecord;
            };

            Because of = () => _sut.Parse(ValidTournamentHandHistory);

            It should_be_valid = () => _sut.IsValid.ShouldBeTrue();

            It should_set_TotalSeats_to_9 = () => _sut.TotalSeats.ShouldEqual(9);
        }

        [Subject(typeof(TotalSeatsParser), "TournamentTables")]
        public class when_parsing_an_invalid_tournament_with_previous_record_6_and_highest_seat_7 : Ctx_ValidTournament_No_RecordSet
        {
            const bool playerSeatsAreValid = false;

            const int highestSeatNumber = 7;

            const int totalSeatsRecord = 6;

            Establish context = () => {
                _playerSeatsParser_Mock
                    .SetupGet(psp => psp.IsValid)
                    .Returns(playerSeatsAreValid);

                _playerSeatsParser_Mock
                    .SetupGet(psp => psp.HighestSeatNumber)
                    .Returns(highestSeatNumber);

                _sut.TotalSeatsRecord = totalSeatsRecord;
            };

            Because of = () => _sut.Parse(ValidTournamentHandHistory);

            It should_be_valid = () => _sut.IsValid.ShouldBeTrue();

            It should_set_TotalSeats_to_6 = () => _sut.TotalSeats.ShouldEqual(totalSeatsRecord);
        }
    }
}