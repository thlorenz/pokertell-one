namespace PokerTell.Infrastructure
{
    using System;

    using Resources;

    public class UnrecognizedHandHistoryFormatException : Exception
    {
        public UnrecognizedHandHistoryFormatException(string fileName)
            : base(string.Format(ErrorMessages.UnrecognizedHandHistoryFormat, fileName))
        {
        }
    }
}