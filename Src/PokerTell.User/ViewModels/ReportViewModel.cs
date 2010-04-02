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

        public string Subject { get; set; }

        public ReportViewModel(IReporter reporter)
        {
            _reporter = reporter;
            try
            {
                _reporter.PrepareReport();
                Subject = "[No Subject]";
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
                        ExecuteDelegate = arg => _reporter.SendReport(Subject, Comments, IncludeScreenshot)
                    });
            }
        }
    }
}