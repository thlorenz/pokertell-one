namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    using System;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Services;
    using PokerTell.PokerHand.Aquisition;
    using PokerTell.PokerHandParsers.Base;

    public class PlayerActionsParserTests : Tests.PlayerActionsParserTests
    {
        #region Methods

        protected override string AllinBetActionFor(string playerName, double ratio)
        {
            // 007hitman raises to $10, and is all in
            return string.Format("{0} raises to ${1}, and is all in", playerName, ratio);
        }

        protected override PlayerActionsParser GetPlayerActionsParser()
        {
            return new PokerTell.PokerHandParsers.FullTiltPoker.PlayerActionsParser(
                new Constructor<IAquiredPokerAction>(() => new AquiredPokerAction()));
        }

        protected override string OneBetOrCallActionFor(string playerName, IAquiredPokerAction action)
        {
            // Hero calls $16
            return string.Format("{0} {1} ${2}", playerName, _parser.ActionStrings[action.What], action.Ratio);
        }

        protected override string OneNonRatioActionFor(string playerName, IAquiredPokerAction action)
        {
            // timmviktor folds // FCHIRO checks
            return string.Format("{0} {1}", playerName, _parser.ActionStrings[action.What]);
        }

        protected override string OneRaiseActionFor(string playerName, IAquiredPokerAction action)
        {
            // Hero raises to $0.50
            return string.Format("{0} raises to ${1}", playerName, action.Ratio);
        }

        protected override string PostingActionFor(string playerName, PostingTypes postingType, double ratio)
        {
            if (postingType == PostingTypes.Ante)
            {
                // Hero antes 50
                return string.Format("{0} antes {1}", playerName, ratio);
            }

            if (postingType == PostingTypes.SmallBlind)
            {
                // enzonapoletano posts the small blind of $0.10
                return string.Format("{0} posts the small blind of ${1}", playerName, ratio);
            }

            if (postingType == PostingTypes.BigBlind)
            {
                // Hero posts the big blind of $0.25
                return string.Format("{0} posts the big blind of ${1}", playerName, ratio);
            }

            throw new ArgumentException(postingType.ToString());
        }

        protected override string UncalledBetActionFor(string playerName, double ratio)
        {
            // Uncalled bet of $5.50 returned to Hero
            return string.Format("Uncalled bet of ${0} returned to {1}", ratio, playerName);
        }

        protected override string WinningActionFor(string playerName, double ratio)
        {
            // 007hitman showed [Qh Qd] and won ($19.57) 
            return string.Format("{0} showed [Qh Qd] and won (${1})", playerName, ratio);
        }

        #endregion
    }
}