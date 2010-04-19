namespace PokerTell.Repository.Views
{
    using Interfaces;

    /// <summary>
    /// Interaction logic for DatabaseImportView.xaml
    /// </summary>
    public partial class DatabaseImportView 
    {
        public DatabaseImportView(IDatabaseImportViewModel databaseImportViewModel)
        {
            InitializeComponent();

            DataContext = databaseImportViewModel;
        }
    }
}