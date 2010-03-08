namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    public interface IPokerHandStringConverter
    {
        string BuildSqlStringFrom(IConvertedPokerRound convertedRound);

        /// <summary>
        /// Converts an action string in Database format into a PokerRound
        /// </summary>
        /// <param name="csvRound">Comma seperated String representation of Bettinground
        /// for instance as read from the database</param>
        IConvertedPokerRound ConvertedRoundFrom(string csvRound);
    }
}