namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    public interface IConvertedPokerActionWithId : IConvertedPokerAction
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        int Id { get; }

        IConvertedPokerActionWithId InitializeWith(IConvertedPokerAction convertedAction, int id);
       
    }
}