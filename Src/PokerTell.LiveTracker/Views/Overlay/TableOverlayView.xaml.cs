namespace PokerTell.LiveTracker.Views.Overlay
{
    using System.Windows;
    using System.Windows.Controls;

    using Interfaces;

    /// <summary>
    /// Interaction logic for TableOverlayView.xaml
    /// </summary>
    public partial class TableOverlayView : Window
    {
        public TableOverlayView()
        {
            InitializeComponent();
        }

        public TableOverlayView(ITableOverlayViewModel tableOverlayViewModel)
            : this()
        {
            DataContext = tableOverlayViewModel;
        }
    }
}