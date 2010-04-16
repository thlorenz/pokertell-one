namespace PokerTell.PokerHandParsers.Base
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using Interfaces.Parsers;

    using log4net;

    public abstract class PlayerSeatsParser : IPlayerSeatsParser
    {
        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public bool IsValid { get; protected set; }

        public IDictionary<int, PlayerData> PlayerSeats { get; protected set; }

        protected abstract string SeatPattern { get; }

        public virtual IPlayerSeatsParser Parse(string handHistory)
        {
            PlayerSeats = new Dictionary<int, PlayerData>();

            MatchCollection players = MatchAllPlayerSeats(handHistory);

            IsValid = players.Count > 1;

            if (IsValid)
            {
                ExtractAllPlayers(players);
            }
            else
            {
                Log.Debug("Found only " + players.Count + " players.");
            }

            return this;
        }

        void ExtractAllPlayers(MatchCollection players)
        {
            foreach (Match player in players)
            {
                ExtractPlayer(player);
            }
        }

        void ExtractPlayer(Match player)
        {
            bool playerWasOutOfHand = player.Groups["OutOfHand"].Success;
            if (playerWasOutOfHand)
            {
                return;
            }

            int seatNumber = Convert.ToInt32(player.Groups["SeatNumber"].Value);

            if (!PlayerSeats.ContainsKey(seatNumber))
            {
                string playerName = player.Groups["PlayerName"].Value;
                double ratio = Convert.ToDouble(player.Groups["Ratio"].Value.Replace(",", string.Empty));
                PlayerSeats.Add(seatNumber, new PlayerData(playerName, ratio));
            }
        }

        MatchCollection MatchAllPlayerSeats(string handHistory)
        {
            return Regex.Matches(handHistory, SeatPattern, RegexOptions.IgnoreCase);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public class PlayerData
        {
            public readonly string Name;

            public readonly double Stack;

            public PlayerData(string name, double stack)
            {
                Name = name;
                Stack = stack;
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public override bool Equals(object obj)
            {
                return obj.GetHashCode().Equals(GetHashCode());
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public override int GetHashCode()
            {
                unchecked
                {
                    return (Stack.GetHashCode() * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public new Type GetType()
            {
                return base.GetType();
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public override string ToString()
            {
                return string.Format("stack: {0}, Name: {1}", Stack, Name);
            }
        }
    }
}