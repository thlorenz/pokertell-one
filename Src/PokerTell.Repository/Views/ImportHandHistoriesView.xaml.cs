namespace PokerTell.Repository.Views
{
    using System;
    using System.Windows.Controls;

    using PokerTell.Repository.ViewModels;

    /// <summary>
    /// Interaction logic for ImportHandHistoriesView.xaml
    /// </summary>
    public partial class ImportHandHistoriesView
    {
        public ImportHandHistoriesView()
        {
            InitializeComponent();
        }

        public ImportHandHistoriesView(ImportHandHistoriesViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }

        void Directory_TextBox_Initialized(object sender, EventArgs e)
        {
            ((TextBox)sender).Focus();
        }
    }
}