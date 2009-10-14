namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using Enumerations.PokerHand;

    public interface IConvertedPokerAction : IPokerAction
    {
        IConvertedPokerAction InitializeWith(ActionTypes what, double ratio);
    }
}