namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public interface IPokerHands : IEnumerable
    {
        int Count { get; }

        /// <summary>
        /// The collection of Poker Hands
        /// </summary>
        ReadOnlyCollection<IPokerHand> Hands { get; }

        /// <summary>
        /// Last Hand in this collection
        /// </summary>
        IPokerHand LastHand { get; }

        IPokerHand this[int index]
        {
            get;
        }

        /// <summary>
        /// Adds a hand, but only if it is not in the Hand collection yet
        /// </summary>
        /// <param name="hand"></param>
        /// <returns>true if it was added, false if it was already in the Hand collection</returns>
        bool AddHand(IPokerHand hand);

        /// <summary>
        /// Adds a collection of hands to this collection
        /// </summary>
        /// <param name="hands">Hands to be added</param>
        void AddHands(IPokerHands hands);

        IPokerHand GetHand(ulong gameId);

        /// <summary>
        /// Checks if a Hand is already contained in this list
        /// </summary>
        /// <param name="givenHand">Hand to check for</param>
        /// <returns>true if the hand does exist, false if it doesn't</returns>
        bool HandExists(IPokerHand givenHand);

        /// <summary>
        /// Checks if a Hand with a certain GamID is already contained in this list
        /// </summary>
        /// <param name="givenGameId">GameID of Hand to check for</param>
        /// <returns>true if the hand does exist, false if it doesn't</returns>
        bool HandExists(ulong givenGameId);

        /// <summary>
        /// Removes a hand at the index
        /// </summary>
        /// <param name="index">Index at which hand will be removed</param>
        void RemoveHand(int index);

        /// <summary>
        /// Removes a Poker Hand from the list
        /// </summary>
        /// <param name="handToRemove">Hand to be removed</param>
        /// <returns>true if removed successfully, otherwise false</returns>
        bool RemoveHand(IPokerHand handToRemove);

        void Sort();

        /// <summary>
        /// Gives a string presentation of all Hands in this collection
        /// </summary>
        /// <returns>String presentation of all Hands in this collection</returns>
        string ToString();

        IPokerHands InitializeWith(List<IPokerHand> hands);
    }
}