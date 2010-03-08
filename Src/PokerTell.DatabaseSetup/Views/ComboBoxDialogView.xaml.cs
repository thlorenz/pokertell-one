namespace PokerTell.DatabaseSetup.Views
{
    using System.Windows.Media;

    using Infrastructure;

    using Interfaces;

    using Tools;

    /// <summary>
    /// Interaction logic for ComboBoxDialogView.xaml
    /// </summary>
    public partial class ComboBoxDialogView
    {
        #region Constructors and Destructors

        public ComboBoxDialogView()
        {
            InitializeComponent();

            if (Static.OperatingSystemIsWindowsXPOrOlder())
            {
                Background = ApplicationProperties.BorderedWindowBackgoundBrush;
                AllowsTransparency = false;
            }
        }

        public ComboBoxDialogView(IComboBoxDialogViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }

        #endregion
    }
}