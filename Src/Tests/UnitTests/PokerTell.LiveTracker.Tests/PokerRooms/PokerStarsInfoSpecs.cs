namespace PokerTell.LiveTracker.Tests.PokerRooms
{
    using Interfaces;

    using LiveTracker.PokerRooms;

    using Machine.Specifications;

    // Resharper disable InconsistentNaming
    public abstract class PokerStarsInfoSpecs
    {
        protected static IPokerRoomInfo _sut;

        Establish specContext = () => _sut = new PokerStarsInfo();
    }

    [Subject(typeof(PokerStarsInfo), "TableName formatting to be matchable in title")]
    public class when_formatting___125696289_4__ : PokerStarsInfoSpecs
    {
        static string formattedTableName;

        Because of = () => formattedTableName = _sut.TableNameFoundInPokerTableTitleFrom("123456789 1");

        It should_return___123456789_Table_1___ = () => formattedTableName.ShouldEqual("123456789 Table 1 ");
    }
}