namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using System.Text.RegularExpressions;

    public class HandHeaderParser : Base.HandHeaderParser
    {
        #region Constants and Fields

        const string FullTiltHeaderPattern =
            @"((Full Tilt Poker Game)|(FullTiltPoker Game)) " +
            @"[#](?<GameId>[0-9]+)[:].+" +
            @"(No |Pot ){0,1}Limit (Hold'em|Holdem)";

        const string TournamentPattern = @"(\((?<TournamentId>[0-9]+)\),) +Table";

        #endregion

        #region Properties

        protected override string HeaderPattern
        {
            get { return FullTiltHeaderPattern; }
        }

        #endregion

        #region Public Methods

        public override Base.HandHeaderParser Parse(string handHistory)
        {
            Match header = MatchHeader(handHistory);
            IsValid = header.Success;

            if (IsValid)
            {
                GameId = ExtractGameId(header);
                ExtractTournamentId(handHistory);
            }

            return this;
        }

        #endregion

        #region Methods

        void ExtractTournamentId(string handHistory)
        {
            Match tournament = Regex.Match(handHistory, TournamentPattern, RegexOptions.IgnoreCase);
            IsTournament = tournament.Success;
            TournamentId = IsTournament
                               ? ulong.Parse(tournament.Groups["TournamentId"].Value)
                               : 0;
        }

        #endregion
    }
}