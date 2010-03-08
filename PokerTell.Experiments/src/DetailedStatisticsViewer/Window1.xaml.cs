
namespace DetailedStatisticsViewer
{
    using System.Windows;

    using ViewModels;

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            DataContext = new DetailedPreFlopStatisticsViewModel();
        }
    }
}
