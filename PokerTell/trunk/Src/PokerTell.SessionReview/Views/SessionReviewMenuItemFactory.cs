namespace PokerTell.SessionReview.Views
{
    using System.Windows.Controls;

    using PokerTell.SessionReview.ViewModels;

    using Properties;

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
            var menuItem = new MenuItem { Header = Resources.SessionReviewMenuItem_Title };
            menuItem.Items.Add(new MenuItem { Header = Resources.SessionReviewMenuItem_Open_Review, Command = _viewModel.OpenReviewCommand });
            menuItem.Items.Add(new MenuItem { Header = Resources.SessionReviewMenuItem_Save_Review, Command = Commands.SaveSessionReviewCommand });
            menuItem.Items.Add(new MenuItem { Header = Resources.SessionReviewMenuItem_Import_Hand_Histories, Command = _viewModel.ImportHandHistoriesCommand });
            menuItem.Items.Add(new Separator());
            menuItem.Items.Add(new MenuItem { Header =Resources.SessionReviewMenuItem_Create_Report, Command = Commands.CreateSessionReviewReportCommand });
            menuItem.Items.Add(new MenuItem { Header = Resources.SessionReviewMenuItem_Save_Report, Command = Commands.SaveSessionReviewReportCommand });

            return menuItem;
        }

        #endregion
    }
}