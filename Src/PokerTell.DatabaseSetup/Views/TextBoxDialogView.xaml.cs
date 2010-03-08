namespace PokerTell.DatabaseSetup.Views
{
    using Interfaces;

    /// <summary>
    /// Interaction logic for TextBoxDialogView.xaml
    /// </summary>
    public partial class TextBoxDialogView
    {
        #region Constructors and Destructors

        public TextBoxDialogView()
        {
            InitializeComponent();
        }

        public TextBoxDialogView(ITextBoxDialogViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }

        #endregion
    }
}