namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    public class ThatAnteParser : Tests.ThatAnteParser
    {
        protected override AnteParser GetAnteParser()
        {
            return new PokerHandParsers.PokerStars.AnteParser();
        }

        protected override string ValidCashGameAnte(double ante)
        {
            // Lezli90: posts the ante $50
            return string.Format("posts the ante ${0}", ante);
        }

        protected override string ValidTournamentAnte(double ante)
        {
            // Lezli90: posts the ante 50
            return string.Format("posts the ante {0}", ante);
        }
    }
}