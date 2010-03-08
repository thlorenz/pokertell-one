namespace PokerTell.PokerHandParsers.PartyPoker
{
    using System.Text.RegularExpressions;

    public class HandHeaderParser : Base.HandHeaderParser
    {
        const string PartyPokerHeaderPattern = @"\*\*\*\*\* Hand History for Game (?<GameId>[0-9]+) \*\*\*\*\*.*";

        const string GameTypePattern = @"((PL )|(NL )){0,1}Texas Hold.+em";
         
        const string TournamentPattern = @"Trny: {0,1}(?<TournamentId>[0-9]+)";
        
        protected override string HeaderPattern
        {
            get { return PartyPokerHeaderPattern; }
        }

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