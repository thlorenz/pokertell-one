namespace PokerTell.PokerHandParsers.PokerStars
{
    using Interfaces.Parsers;

    public class TotalSeatsParser : Base.TotalSeatsParser, IPokerStarsTotalSeatsParser 
    {
        const string PokerStarsTotalSeatsPattern = "((" + PokerStarsLegitTotalSeatsPattern + ")|(" + PokerStarsPokerOfficeDatabaseBlobTotalSeatsPattern + "))"; 
        const string PokerStarsLegitTotalSeatsPattern = TableNameParser.PokerStarsLegitTableNamePattern + @" +(?<TotalSeats>[0-9]{1,2})-max ";

        // e.g. Table Messina III 9-max Seat #1
        const string PokerStarsPokerOfficeDatabaseBlobTotalSeatsPattern = @"\nTable .+ (?<TotalSeats>\d{1,2})-max ";

        protected override string TotalSeatsPattern
        {
            get { return PokerStarsTotalSeatsPattern; }
        }
    }
}