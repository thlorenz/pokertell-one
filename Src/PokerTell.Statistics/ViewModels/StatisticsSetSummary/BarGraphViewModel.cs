namespace PokerTell.Statistics.ViewModels.StatisticsSetSummary
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Media;

    using Infrastructure.Interfaces.Statistics;

    using Tools.FunctionalCSharp;

    public class BarGraphViewModel : IBarGraphViewModel
    {
        public static readonly Color[] DefaultColors = new[] { Colors.LightBlue, Colors.Blue, Colors.DarkBlue, Colors.Gold, Colors.DarkOrange, Colors.Red, Colors.DarkRed, Colors.YellowGreen };

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