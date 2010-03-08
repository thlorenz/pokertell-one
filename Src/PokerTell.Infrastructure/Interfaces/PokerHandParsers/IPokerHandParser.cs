namespace PokerTell.Infrastructure.Interfaces.PokerHandParsers
{
    using System.Collections.Generic;

    using PokerHand;

    public interface IPokerHandParser
    {
        #region Public Methods

        IDictionary<ulong, string> ExtractSeparateHandHistories(string handHistories);

        IPokerHandParser ParseHand(string handHistory);

        bool RecognizesHandHistoriesIn(string histories);

        #endregion

        bool IsValid { get; }

        IAquiredPokerHand AquiredPokerHand { get; }

        bool LogVerbose { get; set; }
    }
}