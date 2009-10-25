namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System.Collections;
    using System.Collections.Generic;

    public interface IPokerRound
    {
        IList<IPokerAction> Actions { get; }
    }
}