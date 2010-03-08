namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using Enumerations.PokerHand;

    public interface IPokerAction
    {
        /// <summary>
        /// The amount connected to the action in relation to the pot
        /// for calling and betting or in relation to the amount to call for raising
        /// </summary>
        double Ratio { get;  }

        /// <summary>The kind of action (call, fold etc.)</summary>
        ActionTypes What { get; }

        /// <summary>
        /// Gives a string representation of the action
        /// </summary>
        /// <returns>String representation of the action type and ratio</returns>
        string ToString();
    }
}