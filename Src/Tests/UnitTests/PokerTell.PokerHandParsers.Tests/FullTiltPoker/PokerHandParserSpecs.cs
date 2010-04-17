namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    using System;

    using Machine.Specifications;

    using Moq;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHandParsers.FullTiltPoker;
    using PokerTell.PokerHandParsers.Interfaces;
    using PokerTell.PokerHandParsers.Interfaces.Parsers;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class PokerHandParserSpecs
    {
        static Mock<IConstructor<IAquiredPokerAction>> _aquiredActionMake_Stub;

        static Mock<IConstructor<IAquiredPokerHand>> _aquiredHandMake_Stub;

        static Mock<IConstructor<IAquiredPokerPlayer>> _aquiredPlayerMake_Stub;

        static Mock<IConstructor<IAquiredPokerRound>> _aquiredRoundMake_Stub;

        static Mock<IFullTiltPokerAnteParser> _anteParser_Stub;

        static Mock<IFullTiltPokerBlindsParser> _blindsParser_Stub;

        static Mock<IFullTiltPokerBoardParser> _boardParser_Stub;

        static Mock<IFullTiltPokerGameTypeParser> _gameTypeParser_Stub;

        static Mock<IFullTiltPokerHandHeaderParser> _handHeaderParser_Stub;

        static Mock<IFullTiltPokerHeroNameParser> _heroNameParser_Stub;

        static Mock<IFullTiltPokerHoleCardsParser> _holeCardsParser_Stub;

        static Mock<IFullTiltPokerPlayerActionsParser> _playerActionsParser_Stub;

        static Mock<IFullTiltPokerPlayerSeatsParser> _playerSeatsParser_Stub;

        static Mock<IFullTiltPokerSmallBlindPlayerNameParser> _smallBlindPlayerNameParser_Stub;

        static Mock<IFullTiltPokerStreetsParser> _streetsParser_Stub;

        static Mock<IFullTiltPokerTableNameParser> _tableNameParser_Stub;

        static Mock<IFullTiltPokerTimeStampParser> _timeStampParser_Stub;

        static Mock<IFullTiltPokerTotalPotParser> _totalPotParser_Stub;

        static Mock<IFullTiltPokerTotalSeatsParser> _totalSeatsParser_Mock;

        static Mock<ITotalSeatsForTournamentsRecordKeeper> _totalSeatsForTournamentsRecordKeeper_Mock;

        static Mock<IAquiredPokerHand> _aquiredPokerHand_Mock;

        static PokerHandParserSut _sut;

        Establish specContext = () => {
            _aquiredHandMake_Stub = new Mock<IConstructor<IAquiredPokerHand>>();
            _aquiredPlayerMake_Stub = new Mock<IConstructor<IAquiredPokerPlayer>>();
            _aquiredRoundMake_Stub = new Mock<IConstructor<IAquiredPokerRound>>();
            _aquiredActionMake_Stub = new Mock<IConstructor<IAquiredPokerAction>>();

            _anteParser_Stub = new Mock<IFullTiltPokerAnteParser>();   
            _blindsParser_Stub = new Mock<IFullTiltPokerBlindsParser>();
            _boardParser_Stub = new Mock<IFullTiltPokerBoardParser>();
            _gameTypeParser_Stub = new Mock<IFullTiltPokerGameTypeParser>(); 
            _handHeaderParser_Stub = new Mock<IFullTiltPokerHandHeaderParser>();
            _heroNameParser_Stub = new Mock<IFullTiltPokerHeroNameParser>();
            _holeCardsParser_Stub = new Mock<IFullTiltPokerHoleCardsParser>();
            _playerActionsParser_Stub = new Mock<IFullTiltPokerPlayerActionsParser>();
            _playerSeatsParser_Stub = new Mock<IFullTiltPokerPlayerSeatsParser>();
            _smallBlindPlayerNameParser_Stub = new Mock<IFullTiltPokerSmallBlindPlayerNameParser>();
            _streetsParser_Stub = new Mock<IFullTiltPokerStreetsParser>();
            _tableNameParser_Stub = new Mock<IFullTiltPokerTableNameParser>();
            _timeStampParser_Stub = new Mock<IFullTiltPokerTimeStampParser>();
            _totalPotParser_Stub = new Mock<IFullTiltPokerTotalPotParser>();

            _totalSeatsParser_Mock = new Mock<IFullTiltPokerTotalSeatsParser>();
            _totalSeatsParser_Mock
                .Setup(sp => sp.Parse(Moq.It.IsAny<string>()))
                .Returns(_totalSeatsParser_Mock.Object);

            _totalSeatsForTournamentsRecordKeeper_Mock = new Mock<ITotalSeatsForTournamentsRecordKeeper>();

            _aquiredPokerHand_Mock = new Mock<IAquiredPokerHand>();

            _sut = new PokerHandParserSut(
                _aquiredHandMake_Stub.Object,
                _aquiredPlayerMake_Stub.Object,
                _aquiredRoundMake_Stub.Object,
                _aquiredActionMake_Stub.Object,
                _anteParser_Stub.Object,
                _blindsParser_Stub.Object,
                _boardParser_Stub.Object,
                _gameTypeParser_Stub.Object,
                _handHeaderParser_Stub.Object,
                _heroNameParser_Stub.Object,
                _holeCardsParser_Stub.Object,
                _playerActionsParser_Stub.Object,
                _playerSeatsParser_Stub.Object,
                _smallBlindPlayerNameParser_Stub.Object,
                _streetsParser_Stub.Object,
                _tableNameParser_Stub.Object,
                _timeStampParser_Stub.Object,
                _totalPotParser_Stub.Object,
                _totalSeatsParser_Mock.Object,
                _totalSeatsForTournamentsRecordKeeper_Mock.Object)
                .Set_HandHistory(string.Empty)
                .Set_AquiredPokerHand(_aquiredPokerHand_Mock.Object);
        };

        [Subject(typeof(PokerHandParser), "ParseTotalSeats")]
        public class when_parsing_valid_total_seats_of_a_cash_game : PokerHandParserSpecs
        {
            const bool isValid = true;

            const bool isTournament = false;

            const int parsedTotalSeats = 6;

            Establish context = () => {
                _handHeaderParser_Stub
                    .SetupGet(hp => hp.IsTournament)
                    .Returns(isTournament);
                _totalSeatsParser_Mock
                    .SetupGet(sp => sp.IsValid)
                    .Returns(isValid);

                _totalSeatsParser_Mock
                    .SetupGet(sp => sp.TotalSeats)
                    .Returns(parsedTotalSeats);
            };

            Because of = () => _sut.Invoke_ParseTotalSeats();

            It should_set_tell_the_TotalSeatsParser_that_it_is_not_a_tournament = () => _totalSeatsParser_Mock.VerifySet(sp => sp.IsTournament = false);

            It should_tell_the_base_to_parse_the_total_seats = () => _sut.TotalSeatsWereParsed.ShouldBeTrue();

            It should_set_the_total_seats_of_the_aquired_hand_to_the_number_returned_by_the_total_seats_parser
                = () => _aquiredPokerHand_Mock.VerifySet(ah => ah.TotalSeats = parsedTotalSeats);
        }

        [Subject(typeof(PokerHandParser), "ParseTotalSeats")]
        public class when_parsing_invalid_total_seats_of_a_cash_game : PokerHandParserSpecs
        {
            const bool isValid = false;

            const bool isTournament = false;

            const int defaultTotalSeats = 9;

            Establish context = () => {
                _handHeaderParser_Stub
                    .SetupGet(hp => hp.IsTournament)
                    .Returns(isTournament);
                _totalSeatsParser_Mock
                    .SetupGet(sp => sp.IsValid)
                    .Returns(isValid);
            };

            Because of = () => _sut.Invoke_ParseTotalSeats();

            It should_set_tell_the_TotalSeatsParser_that_it_is_not_a_tournament = () => _totalSeatsParser_Mock.VerifySet(sp => sp.IsTournament = false);

            It should_tell_the_base_to_parse_the_total_seats = () => _sut.TotalSeatsWereParsed.ShouldBeTrue();

            It should_set_the_total_seats_of_the_aquired_hand_to_the_default_total_Seats_which_is_9
                = () => _aquiredPokerHand_Mock.VerifySet(ah => ah.TotalSeats = defaultTotalSeats);
        }

        [Subject(typeof(PokerHandParser), "ParseTotalSeats")]
        public class when_parsing_valid_total_seats_of_a_tournament : PokerHandParserSpecs
        {
            const bool isValid = true;

            const bool isTournament = true;

            const int totalSeatsRecord = 2;

            const int parsedTotalSeats = 6;

            const ulong tournamentId = 1;

            Establish context = () => {
                _handHeaderParser_Stub
                    .SetupGet(hp => hp.IsTournament)
                    .Returns(isTournament);
                _handHeaderParser_Stub
                    .SetupGet(hp => hp.TournamentId)
                    .Returns(tournamentId);
                _totalSeatsForTournamentsRecordKeeper_Mock
                    .Setup(rk => rk.GetTotalSeatsRecordFor(tournamentId))
                    .Returns(totalSeatsRecord);
                _totalSeatsParser_Mock
                    .SetupGet(sp => sp.IsValid)
                    .Returns(isValid);

                _totalSeatsParser_Mock
                    .SetupGet(sp => sp.TotalSeats)
                    .Returns(parsedTotalSeats);
            };

            Because of = () => _sut.Invoke_ParseTotalSeats();

            It should_set_tell_the_TotalSeatsParser_that_it_is_a_tournament = () => _totalSeatsParser_Mock.VerifySet(sp => sp.IsTournament = true);

            It should_obtain_the_total_seats_record_for_the_tournament_with_the_Id_obtained_by_the_HandHeaderParser
                = () => _totalSeatsForTournamentsRecordKeeper_Mock.Verify(rk => rk.GetTotalSeatsRecordFor(tournamentId));

            It should_set_the_TotalSeatsParser_record_to_the_one_returned_by_the_record_keeper
                = () => _totalSeatsParser_Mock.VerifySet(sp => sp.TotalSeatsRecord = totalSeatsRecord);

            It should_tell_the_base_to_parse_the_total_seats = () => _sut.TotalSeatsWereParsed.ShouldBeTrue();

            It should_set_the_total_seats_of_the_aquired_hand_to_the_number_returned_by_the_total_seats_parser
                = () => _aquiredPokerHand_Mock.VerifySet(ah => ah.TotalSeats = parsedTotalSeats);

            It should_update_the_record_keeper_with_the_totalseats_determined_by_the_total_seats_parser
                = () => _totalSeatsForTournamentsRecordKeeper_Mock.Verify(rk => rk.SetTotalSeatsRecordIfItIsOneFor(tournamentId, parsedTotalSeats));
        }

        [Subject(typeof(PokerHandParser), "ParseTotalSeats")]
        public class when_parsing_invalid_total_seats_of_a_tournament : PokerHandParserSpecs
        {
            const bool isValid = false;

            const bool isTournament = true;

            const int totalSeatsRecord = 2;

            const int defaultTotalSeats = 9;

            const ulong tournamentId = 1;

            Establish context = () => {
                _handHeaderParser_Stub
                    .SetupGet(hp => hp.IsTournament)
                    .Returns(isTournament);
                _handHeaderParser_Stub
                    .SetupGet(hp => hp.TournamentId)
                    .Returns(tournamentId);
                _totalSeatsForTournamentsRecordKeeper_Mock
                    .Setup(rk => rk.GetTotalSeatsRecordFor(tournamentId))
                    .Returns(totalSeatsRecord);
                _totalSeatsParser_Mock
                    .SetupGet(sp => sp.IsValid)
                    .Returns(isValid);
            };

            Because of = () => _sut.Invoke_ParseTotalSeats();

            It should_set_tell_the_TotalSeatsParser_that_it_is_a_tournament = () => _totalSeatsParser_Mock.VerifySet(sp => sp.IsTournament = true);

            It should_obtain_the_total_seats_record_for_the_tournament_with_the_Id_obtained_by_the_HandHeaderParser
                = () => _totalSeatsForTournamentsRecordKeeper_Mock.Verify(rk => rk.GetTotalSeatsRecordFor(tournamentId));

            It should_set_the_TotalSeatsParser_record_to_the_one_returned_by_the_record_keeper
                = () => _totalSeatsParser_Mock.VerifySet(sp => sp.TotalSeatsRecord = totalSeatsRecord);

            It should_tell_the_base_to_parse_the_total_seats = () => _sut.TotalSeatsWereParsed.ShouldBeTrue();

            It should_set_the_total_seats_of_the_aquired_hand_to_the_default_total_Seats_which_is_9
                = () => _aquiredPokerHand_Mock.VerifySet(ah => ah.TotalSeats = defaultTotalSeats);

            It should_not_update_the_record_keeper_with_any_total_Seats
                = () => _totalSeatsForTournamentsRecordKeeper_Mock.Verify(rk => rk.SetTotalSeatsRecordIfItIsOneFor(tournamentId, Moq.It.IsAny<int>()), Times.Never());
        }
    }

    public class PokerHandParserSut : PokerHandParser
    {
        public bool TotalSeatsWereParsed;

        public PokerHandParserSut(
            IConstructor<IAquiredPokerHand> aquiredHandMake, 
            IConstructor<IAquiredPokerPlayer> aquiredPlayerMake, 
            IConstructor<IAquiredPokerRound> aquiredRoundMake, 
            IConstructor<IAquiredPokerAction> aquiredActionMake, 
            IFullTiltPokerAnteParser anteParser, 
            IFullTiltPokerBlindsParser blindsParser, 
            IFullTiltPokerBoardParser boardParser, 
            IFullTiltPokerGameTypeParser gameTypeParser, 
            IFullTiltPokerHandHeaderParser handHeaderParser, 
            IFullTiltPokerHeroNameParser heroNameParser, 
            IFullTiltPokerHoleCardsParser holeCardsParser, 
            IFullTiltPokerPlayerActionsParser playerActionsParser, 
            IFullTiltPokerPlayerSeatsParser playerSeatsParser, 
            IFullTiltPokerSmallBlindPlayerNameParser smallBlindPlayerNameParser, 
            IFullTiltPokerStreetsParser streetsParser, 
            IFullTiltPokerTableNameParser tableNameParser, 
            IFullTiltPokerTimeStampParser timeStampParser, 
            IFullTiltPokerTotalPotParser totalPotParser, 
            IFullTiltPokerTotalSeatsParser totalSeatsParser, 
            ITotalSeatsForTournamentsRecordKeeper totalSeatsForTournamentsRecordKeeper)
            : base(
                aquiredHandMake, 
                aquiredPlayerMake, 
                aquiredRoundMake, 
                aquiredActionMake, 
                anteParser, 
                blindsParser, 
                boardParser, 
                gameTypeParser, 
                handHeaderParser, 
                heroNameParser, 
                holeCardsParser, 
                playerActionsParser, 
                playerSeatsParser, 
                smallBlindPlayerNameParser, 
                streetsParser, 
                tableNameParser, 
                timeStampParser, 
                totalPotParser, 
                totalSeatsParser, 
                totalSeatsForTournamentsRecordKeeper)
        {
        }

        public void Invoke_ParseTotalSeats()
        {
            ParseTotalSeats();
        }

        public PokerHandParserSut Set_HandHistory(string handHistory)
        {
            _handHistory = handHistory;
            return this;
        }

        protected override void ParseTotalSeats()
        {
            base.ParseTotalSeats();
            TotalSeatsWereParsed = true;
        }

        public PokerHandParserSut Set_AquiredPokerHand(IAquiredPokerHand aquiredPokerHand)
        {
            AquiredPokerHand = aquiredPokerHand;
            return this;
        }
    }
}