namespace PokerTell.SessionReview.Views
{
    using System.Windows.Controls;

    using PokerTell.SessionReview.ViewModels;

    public class SessionReviewMenuItemFactory
    {
        readonly SessionReviewMenuItemViewModel _viewModel;

        public SessionReviewMenuItemFactory(SessionReviewMenuItemViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        #region Public Methods

        public MenuItem Create()
        {
            var menuItem = new MenuItem { Header = "SessionReview" };
            menuItem.Items.Add(new MenuItem { Header = "_Open", Command = _viewModel.OpenReviewCommand });
            menuItem.Items.Add(new MenuItem { Header = "_Save", Command = _viewModel.SaveReviewCommand });

            return menuItem;
        }

        #endregion
    }
}