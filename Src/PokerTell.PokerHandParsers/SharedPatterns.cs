namespace PokerTell.PokerHandParsers
{
    internal static class SharedPatterns
    {
        internal const string RatioPattern = @"[$€]{0,1}(\b(?<Ratio>[\d,]+\.\d+)|(?<Ratio>[\d,]+)\b)";
        internal const string Ratio2Pattern = @"[$€]{0,1}(\b(?<Ratio2>[\d,]+\.\d+)|(?<Ratio2>[\d,]+)\b)";
        internal const string CardPattern = @"[2-9TJQKA][cdhs]";
    }
}