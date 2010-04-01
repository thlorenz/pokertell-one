namespace PokerTell.Infrastructure
{
    using System;
    using System.Windows.Forms;

    using Tools;

    /// <summary>
    /// Contains filenames and directories to be used to store settings 
    /// and graphics
    /// </summary>
    public struct Files
    {
        public const string LogFile = "logfile.txt";

        public const string ForUnitTestingOnlyDataFolder = @"C:\SD\PokerTell\data\Mocking";

        public const string DataFolder = @"data";

        public const string UserConfigFile = @"User.config";

        public const string TempFolder = "temp";

        public static readonly string LocalUserAppDataPath = Application.StartupPath.Contains(@"TestDriven.NET")
                                                             ? ForUnitTestingOnlyDataFolder
                                                             : Utils.GetLocalUserDataPath(ApplicationProperties.ApplicationName);

        public static readonly string DocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                                     @"\PokerTell";

        public static readonly string StartupFolder = Application.StartupPath.Contains(@"TestDriven.NET")
                                                       ? ForUnitTestingOnlyDataFolder
                                                       : Application.StartupPath;
    }
}