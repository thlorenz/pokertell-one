namespace PokerTell.PokerHand.Services
{
    using Infrastructure.Interfaces.PokerHand;

    public interface IPokerActionConverter
    {
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
        IConvertedPokerAction Convert(
            IPokerAction aquiredAction, ref double pot, ref double toCall, double totalPot);
    }
}