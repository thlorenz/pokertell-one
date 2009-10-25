namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System.Collections;
    using System.Collections.Generic;

    public interface IPokerRound<TAction> : IEnumerable
    {
        IList<TAction> Actions { get; }

        TAction this[int index]
        {
            get;
        }
    }
}