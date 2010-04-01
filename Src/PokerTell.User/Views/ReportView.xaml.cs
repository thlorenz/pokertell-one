namespace PokerTell.User.Views
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView 
    {
        public ReportView()
        {
            InitializeComponent();
        }

        void Comments_TextBox_Initialized(object sender, System.EventArgs e)
        {
            ((TextBox)sender).Focus();
        }
    }
}