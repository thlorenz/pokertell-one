namespace PokerTell.Repository.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;

    public interface IRepositoryParser
    {
        IEnumerable<IConvertedPokerHand> RetrieveAndConvert(string handHistories, string fileName);
    }
}