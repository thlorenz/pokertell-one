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
        public const string configexePokerTell = @"\PokerTell.exe.config";

        public const string dirAbsolutForUnitTestingOnly = @"C:\SD\PokerTell\data\Mocking";

        public const string dirData = @"\data\";

        public const string dirLayouts = "\\layouts\\";

        public const string dirReviews = @"\Reviews\";

        public const string dirTables = @"\tables\";

        public const string dirTemp = @"\temp\";

        public const string dirTutorials = @"\tutorials\";

        public const string m3uLiveStatsTutorialPlaylist = dirTutorials + @"livestats\PokerTell.LiveStats.Tutorial.m3u";

        public const string xmlExePokerTell = @"\PokerTell.exe.xml";

        public const string xmlPokerRooms = @"\PokerRooms.xml";

        public const string xmlUserConfig = @"\User.config";

        public static readonly string AppDataDirectory = Application.StartupPath.Contains(@"TestDriven.NET")
                                                             ? dirAbsolutForUnitTestingOnly
                                                             : Static.GetLocalUserDataPath(ApplicationProperties.ApplicationName);

        public static readonly string dirDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                                     @"\PokerTell";

        public static readonly string dirStartUp = Application.StartupPath.Contains(@"TestDriven.NET")
                                                       ? dirAbsolutForUnitTestingOnly
                                                       : Application.StartupPath;
    }
}