namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using Enumerations.PokerHand;

    public interface IAquiredPokerAction : IPokerAction
    {
        double ChipsGained { get; }

        IAquiredPokerAction InitializeWith(ActionTypes what, double ratio);
    }
}