namespace PokerTell.PokerHandParsers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHandParsers;

    using Microsoft.Practices.Unity;

    public class PokerHandParsers : IPokerHandParsers
    {
        readonly bool _logVerbose;

        readonly IList<Func<IPokerHandParser>> _parserMakers;

        public PokerHandParsers(bool logVerbose)
        {
            _logVerbose = logVerbose;
            _parserMakers = new List<Func<IPokerHandParser>>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IPokerHandParser> GetEnumerator()
        {
            foreach (var parserMaker in _parserMakers)
            {
                var parser = parserMaker();
                parser.LogVerbose = _logVerbose;
                yield return parser;
            }
        }

        internal IPokerHandParsers Add(Func<IPokerHandParser> parserMaker)
        {
            if (!_parserMakers.Contains(parserMaker))
            {
                _parserMakers.Add(parserMaker);
            }

            return this;
        }
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