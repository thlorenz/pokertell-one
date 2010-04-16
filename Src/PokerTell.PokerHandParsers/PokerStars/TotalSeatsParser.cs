namespace PokerTell.PokerHandParsers.PokerStars
{
    using Interfaces.Parsers;

    public class TotalSeatsParser : Base.TotalSeatsParser, IPokerStarsTotalSeatsParser 
    {
        const string PokerStarsTotalSeatsPattern = 
            TableNameParser.PokerStarsTableNamePattern + @" +(?<TotalSeats>[0-9]{1,2})-max";

        protected override string TotalSeatsPattern
        {
            get { return PokerStarsTotalSeatsPattern; }
        }
    }
}