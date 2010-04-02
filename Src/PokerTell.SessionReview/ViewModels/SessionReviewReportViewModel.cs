namespace PokerTell.SessionReview.ViewModels
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows.Controls;

    using log4net;

    using Microsoft.Practices.Composite.Presentation.Commands;
    using Microsoft.Win32;

    using Tools.WPF.ViewModels;

    public class SessionReviewReportViewModel : ItemsRegionViewModel
    {
        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        DelegateCommand<object> _printReportCommand;

        DelegateCommand<object> _saveReportCommand;

        public SessionReviewReportViewModel(string headerInfo, string htmlDocumentText)
        {
            HeaderInfo = headerInfo;
            HtmlDocumentText = htmlDocumentText;

            Commands.SaveSessionReviewReportCommand.RegisterCommand(SaveReportCommand);
            Commands.PrintSessionReviewReportCommand.RegisterCommand(PrintReportCommand);
        }

        public event Action PrintRequested;

        public string HtmlDocumentText { get; private set; }

        public DelegateCommand<object> PrintReportCommand
        {
            get
            {
                if (_printReportCommand == null)
                {
                    _printReportCommand = new DelegateCommand<object>(PrintReport, CanPrintReport);
                }

                return _printReportCommand;
            }
        }

        public DelegateCommand<object> SaveReportCommand
        {
            get
            {
                if (_saveReportCommand == null)
                {
                    _saveReportCommand = new DelegateCommand<object>(SaveReport, CanSaveReport);
                }

                return _saveReportCommand;
            }
        }

        public bool CanCopyWebBrowserContentToClipboard(WebBrowser arg)
        {
            return IsActive && arg != null;
        }

        public bool CanPrintReport(object arg)
        {
            return IsActive && !string.IsNullOrEmpty(HtmlDocumentText);
        }

        public bool CanSaveReport(object arg)
        {
            return IsActive && !string.IsNullOrEmpty(HtmlDocumentText);
        }

        public void PrintReport(object arg)
        {
            InvokePrintRequested();

            Log.Info("Printing report for: " + HeaderInfo);
        }

        public void SaveReport(object arg)
        {
            var saveFileDialog = new SaveFileDialog
                {
                    AddExtension = true, 
                    DefaultExt = "html", 
                    Filter = "HtmlReports (*.html)|*.html|All files (*.*)|*.*", 
                    Title = "Save PokerTell Html Report"
                };
            saveFileDialog.FileName = HeaderInfo + "." + saveFileDialog.DefaultExt;

            if ((bool)saveFileDialog.ShowDialog())
            {
                File.WriteAllText(saveFileDialog.FileName, HtmlDocumentText);
            }
        }

        protected override void OnIsActiveChanged()
        {
            SaveReportCommand.IsActive =
                PrintReportCommand.IsActive = IsActive;
        }

        void InvokePrintRequested()
        {
            Action requested = PrintRequested;
            if (requested != null)
            {
                requested();
            }
        }
    }
}