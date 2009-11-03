namespace PokerTell.PokerHandParsers
{
    using System.Collections;
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHandParsers;

    public class AvaliablePokerHandParsers : IPokerHandParsers
    {
        #region Constants and Fields

        readonly bool _logVerbose;

        readonly IList<IPokerHandParser> _parsers;

        #endregion

        #region Constructors and Destructors

        public AvaliablePokerHandParsers(bool logVerbose)
        {
            _logVerbose = logVerbose;
            _parsers = new List<IPokerHandParser>();
        }

        #endregion

        #region Implemented Interfaces

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable<IPokerHandParser>

        public IEnumerator<IPokerHandParser> GetEnumerator()
        {
            return _parsers.GetEnumerator();
        }

        #endregion

        #endregion

        #region Methods

        internal AvaliablePokerHandParsers Add(IPokerHandParser parser)
        {
            if (!_parsers.Contains(parser))
            {
                parser.LogVerbose = _logVerbose;
                _parsers.Add(parser);
            }
            return this;
        }

        #endregion
    }
}