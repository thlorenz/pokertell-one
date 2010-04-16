namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using Interfaces.Parsers;

    public class AnteParser : Base.AnteParser, IFullTiltPokerAnteParser 
    {
        const string FullTiltAntePattern = @" Ante " + SharedPatterns.RatioPattern + " - ";

        protected override string AntePattern
        {
            get { return FullTiltAntePattern; }
        }
    }
}