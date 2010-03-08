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

    public class UnableToParseHandHistoryException : Exception
    {
        public UnableToParseHandHistoryException(string handHistory)
            :base(handHistory)
        {
        }
    }

}