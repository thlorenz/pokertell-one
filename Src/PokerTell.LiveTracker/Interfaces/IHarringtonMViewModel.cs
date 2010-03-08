namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;
    using System.Windows;

    using Tools.Interfaces;
    using Tools.WPF.Interfaces;

    public interface IHarringtonMViewModel : IFluentInterface
    {
        int Value { get; set; }

        IPositionViewModel Position { get; }

        IHarringtonMViewModel InitializeWith(IPositionViewModel position);
    }
}