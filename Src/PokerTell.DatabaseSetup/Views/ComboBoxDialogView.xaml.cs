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
        public ComboBoxDialogView()
        {
            InitializeComponent();

            if (Utils.OperatingSystemIsWindowsXPOrOlder())
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

    }
}