namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using Machine.Specifications;

    using PokerTell.PokerHandParsers.PokerStars;

    using Properties;

    // Resharper disable InconsistentNaming
    public abstract class TotalSeatsParserSpecs
    {
        static TotalSeatsParser _sut;

        Establish specContext = () => _sut = new TotalSeatsParser();

        [Subject(typeof(TotalSeatsParser), "Parse PokerOffice Database Blob")]
        public class when_parsing_the_totals_seats_and_the_tablename_info_in_front_of_it_has_no_quotation_marks : TotalSeatsParserSpecs
        {
           static readonly string sampleHandHistory = Resources.PokerStars_PokerOfficeDatabaseBlob;

           Because of = () => _sut.Parse(sampleHandHistory);

           It should_be_valid = () => _sut.IsValid.ShouldBeTrue();

           It should_detect_the_correct_TotalSeats = () => _sut.TotalSeats.ShouldEqual(9);
        }
    }
}