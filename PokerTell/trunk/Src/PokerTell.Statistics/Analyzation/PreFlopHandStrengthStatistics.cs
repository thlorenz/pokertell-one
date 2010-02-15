namespace PokerTell.Statistics.Analyzation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Statistics.Interfaces;

    using Tools;
    using Tools.FunctionalCSharp;

    public class PreFlopHandStrengthStatistics : IPreFlopHandStrengthStatistics
    {
        public PreFlopHandStrengthStatistics(
            IConstructor<IValuedHoleCards> valuedHoleCardsMake, IValuedHoleCardsAverage valuedHoleCardsAverage)
        {
            _valuedHoleCardsAverage = valuedHoleCardsAverage;
            _valuedHoleCardsMake = valuedHoleCardsMake;
        }

        double[] _unraisedPotCallingRatios;

        double[] _raisedPotCallingRatios;

        double[] _raiseSizeKeys;

        public IEnumerable<IAnalyzablePokerPlayer> AnalyzablePlayersWithKnownCards { get; protected set; }

        List<IAnalyzablePokerPlayer>[] _sortedAnalyzablePokerPlayers;

        readonly IConstructor<IValuedHoleCards> _valuedHoleCardsMake;

        public IEnumerable<IAnalyzablePokerPlayer>[] SortedAnalyzablePokerPlayers
        {
            get { return _sortedAnalyzablePokerPlayers; }
        }

        public double[] RatiosUsed { get; protected set; }

        List<IValuedHoleCards>[] _knownCards;

        readonly IValuedHoleCardsAverage _valuedHoleCardsAverage;

        public IEnumerable<IValuedHoleCards>[] KnownCards
        {
            get { return _knownCards; }
        }

        public string[] AverageChenValues { get; protected set; }

        public string[] AverageSklanskyMalmuthGroupings { get; protected set; }

        public IPreFlopHandStrengthStatistics InitializeWith(
            double[] unraisedPotCallingRatios, double[] raisedPotCallingRatios, double[] raiseSizeKeys)
        {
            _raiseSizeKeys = raiseSizeKeys;
            _raisedPotCallingRatios = raisedPotCallingRatios;
            _unraisedPotCallingRatios = unraisedPotCallingRatios;

            return this;
        }

        public IPreFlopHandStrengthStatistics BuildStatisticsFor(
            IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers, ActionSequences actionSequence)
        {
            RatiosUsed = SelectRatioAccordingTo(actionSequence);

            AnalyzablePlayersWithKnownCards =
                analyzablePokerPlayers.Where(p => !string.IsNullOrEmpty(p.Holecards) && !p.Holecards.Contains('?'));

            SortAnalyzablePlayersWithKnownCardsByColumn();

            ExtractAndValueTheirHoleCards();

            DetermineAverageChenValuesAndSklanskyMalmuthGroupings();

            return this;
        }

        void DetermineAverageChenValuesAndSklanskyMalmuthGroupings()
        {
            AverageChenValues = new string[RatiosUsed.Length];
            AverageSklanskyMalmuthGroupings = new string[RatiosUsed.Length];

            foreach (int i in 0.To(RatiosUsed.Length - 1))
            {
                _valuedHoleCardsAverage.InitializeWith(KnownCards[i]);

                if (_valuedHoleCardsAverage.IsValid)
                {
                    AverageChenValues[i] = _valuedHoleCardsAverage.ChenValue.ToString();
                    AverageSklanskyMalmuthGroupings[i] = _valuedHoleCardsAverage.SklanskyMalmuthGrouping.ToString();
                }
                else
                {
                    AverageChenValues[i] = "n/a";
                    AverageSklanskyMalmuthGroupings[i] = "n/a";
                }
            }
        }

        void ExtractAndValueTheirHoleCards()
        {
            InitializeKnownCards();

            foreach (int i in 0.To(_sortedAnalyzablePokerPlayers.Length - 1))
            {
                var iCopy = i;

                _sortedAnalyzablePokerPlayers[iCopy].ForEach(p =>
                {
                    var valuedHoleCards = _valuedHoleCardsMake.New.InitializeWith(p.Holecards);
                    if (valuedHoleCards.AreValid)
                    {
                        _knownCards[iCopy].Add(valuedHoleCards);
                    }
                });
            }
        }

        void InitializeKnownCards()
        {
            _knownCards = new List<IValuedHoleCards>[RatiosUsed.Length];
            foreach (var i in 0.To(RatiosUsed.Length - 1))
            {
                _knownCards[i] = new List<IValuedHoleCards>();
            }
        }

        void SortAnalyzablePlayersWithKnownCardsByColumn()
        {
            InitializeSortedAnalyzablePokerPlayers();

            AnalyzablePlayersWithKnownCards.ForEach(p => {
                var firstActionRatio = p.Sequences[(int)Streets.PreFlop].Actions.First().Ratio;
                var normalizedRatio = Normalizer.NormalizeToKeyValues(RatiosUsed, firstActionRatio);
                var index = Array.IndexOf(RatiosUsed, normalizedRatio);
                _sortedAnalyzablePokerPlayers[index].Add(p);
            });
        }

        double[] SelectRatioAccordingTo(ActionSequences actionSequence)
        {
            switch (actionSequence)
            {
                case ActionSequences.HeroC:
                    return _unraisedPotCallingRatios;
                case ActionSequences.OppRHeroC:
                    return _raisedPotCallingRatios;
                default:
                    return _raiseSizeKeys;
            }
        }

        void InitializeSortedAnalyzablePokerPlayers()
        {
            _sortedAnalyzablePokerPlayers = new List<IAnalyzablePokerPlayer>[RatiosUsed.Length];
            for (int i = 0; i < _sortedAnalyzablePokerPlayers.Length; i++)
            {
                _sortedAnalyzablePokerPlayers[i] = new List<IAnalyzablePokerPlayer>();
            }
        }
    }
}