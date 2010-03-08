namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System;
    using System.Collections.Generic;

    public interface IPokerHand : IComparable
    {
        /// <summary>
        /// Array containing the names of all players present at the table
        /// </summary>
        IList<string> AllNames { get; }

        double Ante { get; set; }

        /// <summary>
        /// The Big Blind
        /// </summary>
        double BB { get; }

        /// <summary>
        /// Contains all cards on the board (As Kh Qd
        /// </summary>
        string Board { get; set; }

        /// <summary>
        /// String representation of date obtained from TimeStamp
        /// </summary>
        string DateAsString { get; }

        /// <summary>
        /// The ID of the hand set by the Site and given in the Hand History
        /// </summary>
        ulong GameId { get; }

        /// <summary>
        /// The small Blind
        /// </summary>
        double SB { get; set; }

        /// <summary>
        /// Name of the PokerSite the hand occurred on
        /// </summary>
        string Site { get; }

        /// <summary>
        /// The name of the table the hand occurred at - given in Hand History
        /// </summary>
        string TableName { get; set; }

        /// <summary>
        /// String representation of time obtained from TimeStamp
        /// </summary>
        string TimeAsString { get; }

        /// <summary>
        /// Date and Time when the hand occured
        /// </summary>
        DateTime TimeStamp { get; }

        /// <summary>
        /// Total Players that acted in the hand
        /// </summary>
        int TotalPlayers { get; set; }

        /// <summary>
        /// Total number seats at the table
        /// </summary>
        int TotalSeats { get; set; }

        /// <summary>
        /// The ID of the torunament set by the Site and given in the Hand History
        /// </summary>
        ulong TournamentId { get; set; }

        string HeroName { get; set; }
    }
}