namespace PokerTell.Statistics.ViewModels.StatisticsSetSummary
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Media;

    using Interfaces;

    public class BarGraphViewModel : IBarGraphViewModel
    {
        readonly Color[] _barColors;

        public BarGraphViewModel()
            : this(new[]
                {
                    Colors.LightBlue, Colors.Blue, Colors.DarkBlue, Colors.Gold, Colors.DarkOrange, Colors.Red,
                    Colors.DarkRed, Colors.YellowGreen
                })
        {
        }

        public BarGraphViewModel(Color[] barColors)
        {
            _barColors = barColors;
            Bars = new ObservableCollection<BarViewModel>();
        }

        public Color[] BarColors
        {
            get { return _barColors; }
        }

        public ObservableCollection<BarViewModel> Bars { get; private set; }

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