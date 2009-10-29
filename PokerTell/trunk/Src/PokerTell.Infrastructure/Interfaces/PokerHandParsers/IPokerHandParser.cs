namespace PokerTell.Infrastructure.Interfaces.PokerHandParsers
{
    using System.Collections.Generic;

    using PokerHand;

    public interface IPokerHandParser
    {
        #region Public Methods

        IDictionary<ulong, string> ExtractSeparateHandHistories(string handHistories);

        IAquiredPokerHand ParseHand(string handHistory);

        bool RecognizesHandHistoriesIn(string histories);

        #endregion
    }
}