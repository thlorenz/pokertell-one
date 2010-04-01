namespace PokerTell.User.Views
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using ViewModels;

    /// <summary>
    /// Interaction logic for UserMessageView.xaml
    /// </summary>
    public partial class UserMessageView
    {

        public UserMessageView()
        {
            InitializeComponent();
        }

        public UserMessageView(UserMessageViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }

        void WindowBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }
    }
}