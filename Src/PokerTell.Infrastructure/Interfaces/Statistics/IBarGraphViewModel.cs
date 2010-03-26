namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System.Collections.ObjectModel;
    using System.Windows.Media;

    public interface IBarGraphViewModel : IFluentInterface
    {
        Color[] BarColors { get; }

        ObservableCollection<IBarViewModel> Bars { get; }

        bool Visible { get; }

        IBarGraphViewModel UpdateWith(int[] percentages);
    }
}