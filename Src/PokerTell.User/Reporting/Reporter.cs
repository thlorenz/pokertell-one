namespace PokerTell.User.Reporting
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading;

    using log4net;

    using PokerTell.Infrastructure;
    using PokerTell.User.Interfaces;

    using Tools;

    /// <summary>
    /// Description of Reporter.
    /// Only one should be created per application to avoid mulitple logfiles and screenshots with the same name being
    /// created and confilictiong with each other in case multiple errors occur close to each other.
    /// </summary>
    public class Reporter : IReporter
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IEmailer _emailer;

        readonly string _fullPathToLogFile = Files.LocalUserAppDataPath + @"\" + Files.LogFile;

        readonly string _tempFolder = string.Format("{0}\\{1}", Files.LocalUserAppDataPath, Files.TempFolder);

        int _count;

        string _logFileCopy;

        public Reporter(IEmailer emailer)
        {
            _emailer = emailer;
        }

        public string LogfileContent { get; protected set; }

        public string ScreenShotFile { get; protected set; }

        public IReporter DeleteReportingTempDirectory()
        {
            if (Directory.Exists(_tempFolder))
            {
                Directory.Delete(_tempFolder, true);
            }

            return this;
        }

        public IReporter PrepareReport()
        {
            AssignFileNamesForCurrentCountAndIncrementIt();

            Utils.TakeScreenShotAndSaveJpegAs(ScreenShotFile);
            LogfileContent = CopyLogFileContent(_fullPathToLogFile, _logFileCopy);

            return this;
        }

        public IReporter SendReport(string caption, string comments, bool includeScreenshot)
        {
            if (! Directory.Exists(_tempFolder))
                Directory.CreateDirectory(_tempFolder);

            new Thread(() => SendMail(caption, comments, _logFileCopy, ScreenShotFile, includeScreenshot))
                .Start();

            return this;
        }

        /// <summary>
        /// This function is neccessary b/c the logfile is locked and exceptions will be thrown
        /// when trying to send it directly
        /// </summary>
        /// <param name="sourcePath">original logfile</param>
        /// <param name="destPath">copy of logfile</param>
        static string CopyLogFileContent(string sourcePath, string destPath)
        {
            string logFileText;

            using (var fileStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    logFileText = streamReader.ReadToEnd();
                }
            }

            File.WriteAllText(destPath, logFileText);

            return logFileText;
        }

        void AssignFileNamesForCurrentCountAndIncrementIt()
        {
            _logFileCopy = string.Format("{0}\\logfile{1}.txt", _tempFolder, _count);
            ScreenShotFile = string.Format("{0}\\screenshot{1}.jpeg", _tempFolder, _count);
            _count++;
        }

        void SendMail(
            string subject, 
            string body, 
            string logFileCopy, 
            string screenShot, 
            bool includeScreenShot)
        {
            FileInfo[] fi;
            var fiLogfileCopy = new FileInfo(logFileCopy);

            if (includeScreenShot)
            {
                var fiScreenShot = new FileInfo(screenShot);
                fi = new[] { fiLogfileCopy, fiScreenShot };
            }
            else
            {
                fi = new[] { fiLogfileCopy };
            }

            try
            {
                _emailer.Send(subject, body, fi);
            }
            catch (Exception excep)
            {
                Log.Error("Unhandled", excep);
            }
        }
    }
}