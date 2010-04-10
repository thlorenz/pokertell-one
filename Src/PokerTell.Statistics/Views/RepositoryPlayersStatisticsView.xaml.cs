namespace PokerTell.Statistics.Views
{
    using Interfaces;

    /// <summary>
    /// Interaction logic for RepositoryPlayersStatisticsView.xaml
    /// </summary>
    public partial class RepositoryPlayersStatisticsView 
    {
        public RepositoryPlayersStatisticsView(IRepositoryPlayersStatisticsViewModel playersStatisticsViewModel)
        {
            InitializeComponent();

            DataContext = playersStatisticsViewModel;
        }
    }
}