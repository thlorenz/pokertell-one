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
            menuItem.Items.Add(new MenuItem { Header = "_Import Hand History", Command = _viewModel.ImportHandHistoriesCommand });
            menuItem.Items.Add(new MenuItem { Header = "_Save", Command = Commands.SaveSessionReviewCommand });
            menuItem.Items.Add(new Separator());
            menuItem.Items.Add(new MenuItem { Header = "_Create Report", Command = Commands.CreateSessionReviewReportCommand });
            menuItem.Items.Add(new MenuItem { Header = "Save _Report", Command = Commands.SaveSessionReviewReportCommand });
            menuItem.Items.Add(new MenuItem { Header = "Print Report", Command = Commands.PrintSessionReviewReportCommand });

            return menuItem;
        }

        #endregion
    }
}