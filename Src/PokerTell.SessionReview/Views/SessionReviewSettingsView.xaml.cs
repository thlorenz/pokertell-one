namespace PokerTell.SessionReview.Views
{
    using System.Windows;

    using Infrastructure.Interfaces.PokerHand;

    using PokerTell.SessionReview.ViewModels;

    /// <summary>
    /// Interaction logic for SessionReviewSettingsView.xaml
    /// </summary>
    public partial class SessionReviewSettingsView
    {
        #region Constructors and Destructors

        public SessionReviewSettingsView(SessionReviewSettingsViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
            DataContext = viewModel;
        }

        #endregion

        #region Properties

        public SessionReviewSettingsViewModel ViewModel { get; private set; }

        #endregion

        #region Methods

        void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                var filter = ViewModel.HandHistoriesViewModel.HandHistoriesFilter;

                ReflectChangedRadioButtonIn(filter);
            }
        }

        void ReflectChangedRadioButtonIn(IHandHistoriesFilter filter)
        {
            if (filter.ShowAll != (bool)radShowAll.IsChecked)
            {
                filter.ShowAll = (bool)radShowAll.IsChecked;
            }

            if (filter.ShowMoneyInvested != (bool)radShowMoneyInvested.IsChecked)
            {
                filter.ShowMoneyInvested = (bool)radShowMoneyInvested.IsChecked;
            }

            if (filter.ShowSawFlop != (bool)radShowSawFlop.IsChecked)
            {
                filter.ShowSawFlop = (bool)radShowSawFlop.IsChecked;
            }

            if (filter.ShowSelectedOnly != (bool)radShowSelectedOnly.IsChecked)
            {
                filter.ShowSelectedOnly = (bool)radShowSelectedOnly.IsChecked;
            }
        }

        #endregion
    }
}