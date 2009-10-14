namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    public interface IPokerHandConverter
    {
        /// <summary>
        /// Converts the given Hand with absolute ratios into a Hand with relative ratios
        /// It assumes all Players of the Hand were sorted previously
        /// </summary>
        /// <param name="sortedAquiredHand">Hand to be converted</param>
        /// <returns>Converted Hand</returns>
        IConvertedPokerHand ConvertAquiredHand(IAquiredPokerHand sortedAquiredHand);

        /// <summary>
        /// Converts the given Hands with absolute ratios into Hands with relative ratios
        /// This is done by replaying the hand and determining the relation of each action ratio
        /// to the pot or the amount that needed to be called
        /// </summary>
        /// <param name="sortedAquiredHands">Hands to be converted</param>
        /// <returns>Converted Hands</returns>
        IPokerHands ConvertAquiredHands(IPokerHands sortedAquiredHands);
    }
}