namespace PokerTell.PokerHand.Services
{
    using System;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    public class PokerActionConverter : IPokerActionConverter
    {
        readonly IConstructor<IConvertedPokerAction> _convertedAction;

        public PokerActionConverter(IConstructor<IConvertedPokerAction> convertedAction)
        {
            _convertedAction = convertedAction;
        }

        /// <summary>
        /// Converts an action with an absolute ratio into an action with a relative ratio
        /// It also updates the pot and the amount to call
        /// </summary>
        /// <param name="aquiredAction">The absolute action</param>
        /// <param name="aquiredAction"></param>
        /// <param name="pot">The size of the pot when player acted</param>
        /// <param name="toCall">The amount that player needed to call</param>
        /// <param name="totalPot">The pot at the end of the hand needed to determine winning ratio, in case pot is shared at show down</param>
        /// <returns></returns>
        public IConvertedPokerAction Convert(
            IPokerAction aquiredAction, ref double pot, ref double toCall, double totalPot)
        {
            double currentPot = pot;
            double currentToCall = toCall;

            switch (aquiredAction.What)
            {
                case ActionTypes.F:
                case ActionTypes.X:
                case ActionTypes.A:
                    {
                        // Ratio got added to pot on previous action like R (raise all in) - all in is just the tail of the action
                        return _convertedAction.New.InitializeWith(
                            aquiredAction.What, aquiredAction.Ratio); // Didn't change
                    }

                case ActionTypes.C:
                    {
                        pot += aquiredAction.Ratio;
                        return _convertedAction.New.InitializeWith(
                            aquiredAction.What, aquiredAction.Ratio / currentPot);
                    }

                case ActionTypes.B:
                    {
                        toCall = aquiredAction.Ratio; // next player needs to call this or raise relative to it
                        pot += aquiredAction.Ratio;

                        // Bet relative current to pot
                        return _convertedAction.New.InitializeWith(
                            aquiredAction.What, aquiredAction.Ratio / currentPot);
                    }

                case ActionTypes.R:
                    {
                        toCall = toCall > aquiredAction.Ratio ? toCall : aquiredAction.Ratio;
                        pot += aquiredAction.Ratio;
                        return _convertedAction.New.InitializeWith(
                            aquiredAction.What, aquiredAction.Ratio / currentToCall);
                    }

                case ActionTypes.W:
                    {
                        return _convertedAction.New.InitializeWith(
                            aquiredAction.What, aquiredAction.Ratio / totalPot);
                    }

                default:
                    {
                        throw new ArgumentException("aquiredAction", "Was: " + aquiredAction.What);
                    }
            }
        }

    }
}