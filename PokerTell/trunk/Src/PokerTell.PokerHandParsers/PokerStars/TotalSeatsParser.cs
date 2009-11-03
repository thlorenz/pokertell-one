namespace PokerTell.PokerHandParsers.PokerStars
{
    public class TotalSeatsParser : Base.TotalSeatsParser
    {
        const string PokerStarsTotalSeatsPattern = 
            TableNameParser.PokerStarsTableNamePattern + @" +(?<TotalSeats>[0-9]{1,2})-max";

        protected override string TotalSeatsPattern
        {
            get { return PokerStarsTotalSeatsPattern; }
        }
    }
}