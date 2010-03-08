namespace PokerTell.User.Views
{
    using ViewModels;

    /// <summary>
    /// Interaction logic for ComboBoxDialogView.xaml
    /// </summary>
    public partial class ConfirmActionView
    {
        #region Constructors and Destructors

        public ConfirmActionView()
        {
            InitializeComponent();
        }

        public ConfirmActionView(ConfirmActionViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }

        #endregion
    }
}