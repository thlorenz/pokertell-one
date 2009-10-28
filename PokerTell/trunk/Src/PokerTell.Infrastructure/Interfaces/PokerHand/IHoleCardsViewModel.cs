namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    public interface IHoleCardsViewModel : INotifyPropertyChanged
    {
        string Rank1 { get; set; }

        string Rank2 { get; set; }

        Image Suit1 { get; set; }

        Image Suit2 { get; set; }

        double Left { get; set; }

        double Top { get; set; }

        bool Visible { get; set; }

        void UpdateWith(string holeCardsString);

        void SetLocationTo(Point location);
    }
}