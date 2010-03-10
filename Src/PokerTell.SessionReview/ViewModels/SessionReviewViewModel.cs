namespace PokerTell.SessionReview.ViewModels
{
    using System.Reflection;
    using System.Text;

    using Interfaces;

    using log4net;

    using Microsoft.Practices.Composite.Presentation.Commands;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Win32;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.SessionReview.Views;

    using Tools.Serialization;
    using Tools.WPF.ViewModels;

    internal class SessionReviewViewModel : ItemsRegionViewModel, ISessionReviewViewModel
    {
        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IHandHistoriesViewModel _handHistoriesViewModel;

        readonly IRegionManager _regionManager;

        DelegateCommand<object> _createReportCommand;

        DelegateCommand<object> _saveCommand;

        public SessionReviewViewModel(IRegionManager regionManager, IHandHistoriesViewModel handHistoriesViewModel)
        {
            _regionManager = regionManager;
            _handHistoriesViewModel = handHistoriesViewModel;

            _handHistoriesViewModel.ShowSelectOption = true;
            _handHistoriesViewModel.ShowHandNotes = true;

            Commands.SaveSessionReviewCommand.RegisterCommand(SaveCommand);

            Commands.CreateSessionReviewReportCommand.RegisterCommand(CreateReportCommand);

            HeaderInfo = "SessionReview " + GetHashCode() + " for: " + _handHistoriesViewModel.GetHashCode();
        }

        public DelegateCommand<object> CreateReportCommand
        {
            get
            {
                return _createReportCommand ??
                       (_createReportCommand = new DelegateCommand<object>(CreateReport, arg => IsActive));
            }
        }

        public IHandHistoriesViewModel HandHistoriesViewModel
        {
            get { return _handHistoriesViewModel; }
        }

        public DelegateCommand<object> SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new DelegateCommand<object>(Save, arg => IsActive)); }
        }

        public void CreateReport(object arg)
        {
            Log.InfoFormat("SessionReview->CreatingReport: {0}", GetHashCode());

            string htmlText = BuildHtmlText();

            AddReportToShell(htmlText);
        }

        public void Save(object arg)
        {
            Log.InfoFormat("SessionReview->Saving: {0}", GetHashCode());
            var saveFileDialog = new SaveFileDialog
                {
                    AddExtension = true, 
                    DefaultExt = "pthh", 
                    Filter = "PokerTell HandHistories (*.pthh)|*.pthh|All files (*.*)|*.*", 
                    Title = "Save PokerTell Session Review"
                };
            saveFileDialog.FileName = "SessionReview" + "." + saveFileDialog.DefaultExt;

            if ((bool)saveFileDialog.ShowDialog())
            {
                BinarySerializer.Serialize(_handHistoriesViewModel, saveFileDialog.FileName);
            }
        }

        protected override void OnIsActiveChanged()
        {
            SaveCommand.IsActive =
                CreateReportCommand.IsActive = IsActive;
        }

        void AddReportToShell(string htmlText)
        {
            var reportViewModel = new SessionReviewReportViewModel(GetHashCode().ToString(), htmlText);
            var reportView = new SessionReviewReportView(reportViewModel);
            _regionManager.Regions[ApplicationProperties.ShellMainRegion].Add(reportView);
            _regionManager.Regions[ApplicationProperties.ShellMainRegion].Activate(reportView);
        }

        string BuildHtmlText()
        {
            IHandHistoriesFilter filter = _handHistoriesViewModel.HandHistoriesFilter;

            string heroName = filter.SelectHero ? filter.HeroName : null;

            string htmlHandHistories = HtmlStringBuilder.BuildFrom(
                _handHistoriesViewModel.SelectedHandHistories, 
                filter.ShowPreflopFolds, 
                heroName);

            Encoding enc = Encoding.UTF8;

            string htmlBegin = "<html>\r\n<head>\r\n"
                               + "<meta http-equiv=\"Content-Type\""
                               + " content=\"text/html; charset=" + enc.WebName + "\">\r\n"
                               + "<title>Poker Game Report</title>\r\n</head>\r\n<body>\r\n";

            const string htmlEnd = "\r\n</body>\r\n</html>\r\n";

            return htmlBegin + htmlHandHistories + htmlEnd;
        }
    }
}