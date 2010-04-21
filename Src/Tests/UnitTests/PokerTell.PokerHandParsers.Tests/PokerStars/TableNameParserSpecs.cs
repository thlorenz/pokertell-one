namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using Machine.Specifications;

    using PokerTell.PokerHandParsers.PokerStars;

    using Properties;

    // Resharper disable InconsistentNaming
    public abstract class TableNameParserSpecs
    {
        static TableNameParser _sut;

        Establish specContext = () => _sut = new TableNameParser();

        [Subject(typeof(TableNameParser), "Parse PokerOffice Database blob")]
        public class when_table_name_info_starts_on_new_line_but_is_missing_quotation_marks : TableNameParserSpecs
        {
           static readonly string sampleHandHistory = Resources.PokerStars_PokerOfficeDatabaseBlob;

           Because of = () => _sut.Parse(sampleHandHistory);

           It should_be_valid = () => _sut.IsValid.ShouldBeTrue();

           It should_detect_the_table_name = () => _sut.TableName.ShouldEqual("Chloris III ");
        }
    }
}