namespace PokerTell.User.ViewModels
{
    using System;
    using System.Reflection;
    using System.Windows.Input;

    using log4net;

    using PokerTell.User.Interfaces;

    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public class ReportViewModel : NotifyPropertyChanged, IReportViewModel
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IReporter _reporter;

        bool _includeScreenshot;

        ICommand _sendReportCommand;

        public ReportViewModel(IReporter reporter)
        {
            _reporter = reporter;
            try
            {
                _reporter.PrepareReport();
                LogFileContent = _reporter.LogfileContent;
                ScreenshotPath = _reporter.ScreenShotFile;
            }
            catch (Exception excep)
            {
                Log.Error(excep);
                LogFileContent = "Couldn't read logfile";
                ScreenshotPath = string.Empty;
            }
        }

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
                        // TODO: SUbject should be last or second line of logfile to see right away what the problem is
                        ExecuteDelegate = arg => _reporter.SendReport("User Report", Comments, IncludeScreenshot)
                    });
            }
        }
    }
}