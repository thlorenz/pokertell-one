namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using System;

    using Base;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Services;

    using PokerHand.Aquisition;

    public class PlayerActionsParserTests : Tests.PlayerActionsParserTests
    {
        protected override PlayerActionsParser GetPlayerActionsParser()
        {
            return new PokerTell.PokerHandParsers.PokerStars.PlayerActionsParser(
                new Constructor<IAquiredPokerAction>(() => new AquiredPokerAction()));
        }

        protected override string OneRaiseActionFor(string playerName, IAquiredPokerAction action)
        {
           // barbadardo: raises $0.50 to $0.75
           return string.Format("{0}: {1} $0.50 to ${2}", playerName, _parser.ActionStrings[action.What], action.Ratio);
        }

        protected override string OneBetOrCallActionFor(string playerName, IAquiredPokerAction action)
        {
            // Luetze: bets $300
            return string.Format("{0}: {1} ${2}", playerName, _parser.ActionStrings[action.What], action.Ratio);
        }

        protected override string PostingActionFor(string playerName, PostingTypes postingType, double ratio)
        {
            if (postingType == PostingTypes.Ante)
            {
                // monzaaa: posts the ante 25
                return string.Format("{0}: posts the ante {1}",playerName ,ratio);
            }

            if (postingType == PostingTypes.SmallBlind)
            {
               // Gryko13: posts small blind $0.10
                return string.Format("{0}: posts small blind ${1}", playerName, ratio);
            }

            if (postingType == PostingTypes.BigBlind)
            {
                // evgueni88: posts big blind $0.25
                return string.Format("{0}: posts big blind ${1}", playerName, ratio);
            }
            
            throw new ArgumentException(postingType.ToString());
        }

        protected override string OneNonRatioActionFor(string playerName, IAquiredPokerAction action)
        {
            // chuengi: folds 
            return string.Format("{0}: {1}", playerName, _parser.ActionStrings[action.What]);
        }

        protected override string UncalledBetActionFor(string playerName, double ratio)
        {
            // Uncalled bet (500) returned to marciocid
            return string.Format("Uncalled bet ({0}) returned to {1}", ratio, playerName);
        }

        protected override string AllinBetActionFor(string playerName, double ratio)
        {
            // renniweg: bets 5950 and is all-in
            return string.Format("{0}: bets {1} and is all-in", playerName, ratio);
        }

        protected override string WinningActionFor(string playerName, double ratio)
        {
            // Gryko13 collected $1.70 from pot
            return string.Format("{0} collected ${1} from pot", playerName, ratio);
        }
    }
}