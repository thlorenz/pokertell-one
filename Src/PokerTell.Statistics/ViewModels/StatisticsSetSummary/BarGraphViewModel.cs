namespace PokerTell.Statistics.ViewModels.StatisticsSetSummary
{
    using System.Collections.ObjectModel;
    using System.Windows.Media;

    using PokerTell.Infrastructure.Interfaces.Statistics;

    public class BarGraphViewModel : IBarGraphViewModel
    {
        public static readonly Color[] DefaultColors = new[]
            { Colors.LightBlue, Colors.Blue, Colors.DarkRed,  Colors.Gold, Colors.DarkBlue, Colors.DarkOrange, Colors.Red, Colors.YellowGreen };

        // Resharper disable InconsistentNaming
        public static Color BarColor_0 = DefaultColors[0];

        public static Color BarColor_1 = DefaultColors[1];

        public static Color BarColor_2 = DefaultColors[2];

        public static Color BarColor_3 = DefaultColors[3];

        public static Color BarColor_4 = DefaultColors[4];

        public static Color BarColor_5 = DefaultColors[5];

        public static Color BarColor_6 = DefaultColors[6];

        public static Color BarColor_7 = DefaultColors[7];

        // Resharper enable InconsistentNaming
        readonly Color[] _barColors;

        public BarGraphViewModel()
            : this(DefaultColors)
        {
        }

        public BarGraphViewModel(Color[] barColors)
        {
            _barColors = barColors;
            Bars = new ObservableCollection<IBarViewModel>();
        }

        public Color[] BarColors
        {
            get { return _barColors; }
        }

        public ObservableCollection<IBarViewModel> Bars { get; private set; }

        public bool Visible { get; private set; }

        public IBarGraphViewModel UpdateWith(int[] percentages)
        {
            Bars.Clear();

            if (percentages.Length < 2)
            {
                Visible = false;
                return this;
            }

            AddBarsAndMakeVisible(percentages);

            return this;
        }

        void AddBarsAndMakeVisible(int[] percentages)
        {
            for (int i = 0; i < percentages.Length; i++)
            {
                Bars.Add(new BarViewModel(_barColors[i], percentages[i]));
            }

            Visible = true;
        }
    }
}