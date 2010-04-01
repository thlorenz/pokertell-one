namespace PokerTell.User.ViewModels
{
    using System.Windows.Input;

    using PokerTell.User.Interfaces;

    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public class ReportViewModel : NotifyPropertyChanged, IReportViewModel
    {
        public ReportViewModel(IReporter reporter)
        {
            _reporter = reporter;

            _reporter.PrepareReport();
            LogFileContent = _reporter.LogfileContent;
            ScreenshotPath = _reporter.ScreenShotFile;
        }

        bool _includeScreenshot;

        ICommand _sendReportCommand;

        readonly IReporter _reporter;

        public string Comments { get; set; }

        public bool IncludeScreenshot
        {
            get { return _includeScreenshot; }
            set
            {
                _includeScreenshot = value;
                RaisePropertyChanged(() => IncludeScreenshot);
            }
        }

        public string LogFileContent { get; protected set; }

        public string ScreenshotPath { get; protected set; }

        public ICommand SendReportCommand
        {
            get
            {
                return _sendReportCommand ?? (_sendReportCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => _reporter.SendReport("User Report", Comments, IncludeScreenshot)
                    });
            }
        }
    }
}