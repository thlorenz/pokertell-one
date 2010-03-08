namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    public class AnteParser : Base.AnteParser
    {
        const string FullTiltAntePattern = @" Ante " + SharedPatterns.RatioPattern + " - ";

        protected override string AntePattern
        {
            get { return FullTiltAntePattern; }
        }
    }
}