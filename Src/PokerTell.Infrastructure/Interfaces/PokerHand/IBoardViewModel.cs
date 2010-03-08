namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System.ComponentModel;
    using System.Windows.Controls;

    public interface IBoardViewModel : INotifyPropertyChanged
    {
        string Rank1 { get; set; }

        string Rank2 { get; set; }

        string Rank3 { get; set; }

        string Rank4 { get; set; }

        string Rank5 { get; set; }

        Image Suit1 { get; set; }

        Image Suit2 { get; set; }

        Image Suit3 { get; set; }

        Image Suit4 { get; set; }

        Image Suit5 { get; set; }

        bool Visible { get; set; }

        void UpdateWith(string boardString);
    }
}