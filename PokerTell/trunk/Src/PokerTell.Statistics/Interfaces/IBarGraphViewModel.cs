namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.ObjectModel;
    using System.Windows.Media;

    using Infrastructure.Interfaces;

    using ViewModels.StatisticsSetSummary;

    public interface IBarGraphViewModel : IFluentInterface
    {
        Color[] BarColors { get; }

        ObservableCollection<BarViewModel> Bars { get; }

        bool Visible { get; }

        IBarGraphViewModel UpdateWith(int[] percentages);
    }
}