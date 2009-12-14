namespace PokerTell.Statistics.ViewModels.StatisticsSetSummary
{
    using System.Windows.Media;

    public class BarViewModel
    {
        public int Percentage { get; private set; }

        public SolidColorBrush Stroke { get; private set; }

        public BarViewModel(Color color, int percentage)
        {
            Stroke = new SolidColorBrush(color);
            Percentage = percentage;
        }
    }
}