namespace PokerTell.PokerHandParsers.PartyPoker
{
    public class AnteParser : Base.AnteParser
    {
        const string PartyPokerAntePattern =  @"-Antes\(.+ -" + SharedPatterns.RatioPattern + @"\)";
        protected override string AntePattern
        {
            get { return PartyPokerAntePattern; }
        }
    }
}