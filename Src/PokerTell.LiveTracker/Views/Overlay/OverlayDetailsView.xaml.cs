namespace PokerTell.LiveTracker.Views.Overlay
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;

    using Interfaces;

    using Tools.WPF.Controls;

    /// <summary>
    /// Interaction logic for OverlayDetailsView.xaml
    /// </summary>
    public partial class OverlayDetailsView 
    {
        AdornerLayer _adornerLayer;

        UIElement _selectedElement;

        public OverlayDetailsView()
        {
            InitializeComponent();

            DataContextChanged += OverlayDetailsView_DataContextChanged;
        }

        public void ResizableElement_MouseEnter(object sender, MouseEventArgs e)
        {
            _selectedElement = e.Source as UIElement;
            AddResizeAdorner();

            e.Handled = true;
        }

        void AddResizeAdorner()
        {
            if (_selectedElement != null)
            {
                _adornerLayer = AdornerLayer.GetAdornerLayer(_selectedElement);
                _adornerLayer.Add(new ResizeAdorner(_selectedElement));
            }
        }

        void GameHistory_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
           const int rollSize = 120;

           int change = 0 - (e.Delta / rollSize);

           var tableOverlayViewModel = (ITableOverlayViewModel) DataContext;

           tableOverlayViewModel.GameHistory.CurrentHandIndex += change;

            // Prevent the hand itself from being scrolled
            e.Handled = true;
        }

        void DetailedStatistics_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
           const int rollSize = 120;

           int change = 0 - (e.Delta / rollSize);

           var tableOverlayViewModel = (ITableOverlayViewModel) DataContext;

           tableOverlayViewModel.PokerTableStatisticsViewModel.DetailedStatisticsAnalyzer.CurrentViewModel.Scroll(change);

           e.Handled = true;
        }

        void OverlayDetailsView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           var tableOverlayViewModel = (ITableOverlayViewModel) DataContext;

           tableOverlayViewModel.PokerTableStatisticsViewModel.UserSelectedStatisticsSet += _ => DetailedStatistics_TabItem.IsSelected = true;
        }
    }
}