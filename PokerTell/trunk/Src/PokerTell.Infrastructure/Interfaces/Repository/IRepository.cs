namespace PokerTell.Infrastructure.Interfaces.Repository
{
    using System.Collections.Generic;

    using PokerHand;

    public interface IRepository
    {
        IEnumerable<IConvertedPokerHand> RetrieveHandsFromFile(string fileName);
    }
}