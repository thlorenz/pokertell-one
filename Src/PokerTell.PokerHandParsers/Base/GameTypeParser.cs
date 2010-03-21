namespace PokerTell.PokerHandParsers.Base
{
    using System.Text.RegularExpressions;

    using PokerTell.Infrastructure.Enumerations.PokerHand;

    using Tools.FunctionalCSharp;

    public abstract class GameTypeParser
    {
        public bool IsValid { get; protected set; }

        public GameTypes GameType { get; protected set; }

        protected abstract string GameTypePattern { get; }

        public virtual GameTypeParser Parse(string handHistory)
        {
            Match gameTypeMatch = MatchGameType(handHistory);
            IsValid = gameTypeMatch.Success;

            if (IsValid)
                ExtractGameType(gameTypeMatch);

            return this;
        }

        protected virtual void ExtractGameType(Match gameTypeMatch)
        {
            GameType =
                gameTypeMatch.Groups["GameType"].Value.ToLower().Match()
                    .With(gt => gt == "limit", gt => GameTypes.Limit)
                    .With(gt => gt == "pot limit", gt => GameTypes.PotLimit)
                    .With(gt => gt == "no limit", gt => GameTypes.NoLimit)
                    .Do();
        }

        protected virtual Match MatchGameType(string handHistory)
        {
            return Regex.Match(handHistory, GameTypePattern, RegexOptions.IgnoreCase);
        }
    }
}