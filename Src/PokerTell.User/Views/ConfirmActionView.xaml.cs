namespace PokerTell.User.Views
{
    using ViewModels;

    /// <summary>
    /// Interaction logic for ComboBoxDialogView.xaml
    /// </summary>
    public partial class ConfirmActionView
    {
        public ConfirmActionView()
        {
            InitializeComponent();
        }

        public ConfirmActionView(ConfirmActionViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }

    }
}