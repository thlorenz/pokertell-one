﻿namespace PokerTell.User.Views
{
    using System.Windows.Controls.Primitives;

    using ViewModels;

    /// <summary>
    /// Interaction logic for StatusBarView.xaml
    /// </summary>
    public partial class StatusBarView
    {
        public StatusBarView()
        {
            InitializeComponent();
        }

        public StatusBarView(StatusBarViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }
    }
}
