﻿namespace PokerTell.DatabaseSetup.Views
{
    using ViewModels;

    /// <summary>
    /// Interaction logic for ComboBoxDialogView.xaml
    /// </summary>
    public partial class ComboBoxDialogView
    {
        #region Constructors and Destructors

        public ComboBoxDialogView()
        {
            InitializeComponent();
        }

        public ComboBoxDialogView(IComboBoxDialogViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }

        #endregion
    }
}