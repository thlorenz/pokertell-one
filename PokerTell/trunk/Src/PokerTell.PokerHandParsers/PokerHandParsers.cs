namespace PokerTell.PokerHandParsers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHandParsers;

    using Microsoft.Practices.Unity;

    public class PokerHandParsers : IPokerHandParsers
    {
        #region Constants and Fields

        readonly bool _logVerbose;

        readonly IList<Func<IPokerHandParser>> _parserMakers;

        #endregion

        #region Constructors and Destructors

        public PokerHandParsers(bool logVerbose)
        {
            _logVerbose = logVerbose;
            _parserMakers = new List<Func<IPokerHandParser>>();
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
            foreach (var parserMaker in _parserMakers)
            {
                var parser = parserMaker();
                parser.LogVerbose = _logVerbose;
                yield return parser;
            }
        }

        #endregion

        #endregion

        #region Methods

        internal IPokerHandParsers Add(Func<IPokerHandParser> parserMaker)
        {
            if (!_parserMakers.Contains(parserMaker))
            {
                _parserMakers.Add(parserMaker);
            }

            return this;
        }

        #endregion
    }

    public class AvailablePokerHandParsers : PokerHandParsers
    {
        readonly IUnityContainer _container;

        public AvailablePokerHandParsers(IUnityContainer container)
            : base(false)
        {
            _container = container;
            Add(() => _container.Resolve<FullTiltPoker.PokerHandParser>());
            Add(() => _container.Resolve<PokerStars.PokerHandParser>());
        }
    }
   
}