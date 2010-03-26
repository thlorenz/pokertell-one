namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System;
    using System.ComponentModel;
    using System.Windows.Controls;

    public interface IHoleCardsViewModel : INotifyPropertyChanged, IFluentInterface
    {
        string Rank1 { get; set; }

        string Rank2 { get; set; }

        Image Suit1 { get; set; }

        Image Suit2 { get; set; }

        bool Visible { get; set; }

        void UpdateWith(string holeCardsString);
    }
}