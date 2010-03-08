namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for TableOverlayControllerWindow.xaml
    /// </summary>
    public partial class TableOverlayControllerWindow : Window
    {
        public TableOverlayControllerWindow()
        {
            InitializeComponent();
        }

        public TableOverlayControllerWindow(TableOverlayControllerViewModel controllerViewModel)
            : this()
        {
            DataContext = controllerViewModel;
        }
    }
}