namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using Enumerations.PokerHand;

    public interface IConvertedPokerActionWithId : IConvertedPokerAction
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        int Id { get; }

        IConvertedPokerActionWithId InitializeWith(IConvertedPokerAction convertedAction, int id);

        IConvertedPokerActionWithId InitializeWith(ActionTypes what, double ratio, int id);
    }
}