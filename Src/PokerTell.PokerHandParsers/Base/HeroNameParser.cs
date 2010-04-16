namespace PokerTell.PokerHandParsers.Base
{
    using System.Text.RegularExpressions;

    using Interfaces.Parsers;

    public abstract class HeroNameParser : IHeroNameParser
    {
        protected abstract string HeroNamePattern { get; }

        public string HeroName { get; protected set; }

        public bool IsValid { get; protected set; }

        public IHeroNameParser Parse(string handHistory)
        {
            Match hero = MatchTableName(handHistory);
            IsValid = hero.Success;

            if (IsValid)
            {
                ExtractHeroName(hero);
            }

            return this;
        }

        Match MatchTableName(string handHistory)
        {
            return Regex.Match(handHistory, HeroNamePattern, RegexOptions.IgnoreCase);
        }

        void ExtractHeroName(Match table)
        {
            HeroName = table.Groups["HeroName"].Value;
        }
    }
}