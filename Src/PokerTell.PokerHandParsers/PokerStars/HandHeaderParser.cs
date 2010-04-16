namespace PokerTell.PokerHandParsers.PokerStars
{
    using Interfaces.Parsers;

    public class HandHeaderParser : Base.HandHeaderParser, IPokerStarsHandHeaderParser 
    {
        const string PokerStarsHeaderPattern =
            @"PokerStars Game [#](?<GameId>[0-9]+)[:] (Tournament [#](?<TournamentId>[0-9]+)){0,1}.*(Hold'em|Holdem) (No |Pot )*Limit";

        protected override string HeaderPattern
        {
            get { return PokerStarsHeaderPattern; }
        }
    }
}