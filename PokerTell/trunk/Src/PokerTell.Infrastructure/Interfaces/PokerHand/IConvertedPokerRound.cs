namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System;

    public interface IConvertedPokerRound : IPokerRound
    {
        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        IConvertedPokerAction this[int index]
        {
            get;
        }

        /// <summary>
        /// The add action.
        /// </summary>
        /// <param name="theAction">
        /// The the action.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        IConvertedPokerRound Add(IConvertedPokerAction theAction);
    }
}