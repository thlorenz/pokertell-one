namespace PokerTell.Repository.Interfaces
{
    using Infrastructure.Interfaces.DatabaseSetup;
    using Infrastructure.Interfaces.PokerHand;

    public interface IConvertedPokerHandInserter
    {
        IConvertedPokerHandInserter Use(IDataProvider dataProvider);

        void Insert(IConvertedPokerHand convHand);
    }
}