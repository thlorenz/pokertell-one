namespace PokerTell.LiveTracker.PokerRooms
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using log4net;

    using PokerTell.LiveTracker.Interfaces;

    public class FullTiltPokerDetective : IPokerRoomDetective
    {
        const string AppFolder = @"\FullTiltPoker";

        const string HandHistoryFolderPattern = "<WSTRING Value=\"(?<HandHistoryPath>.+)\"";

        const string HandHistoryPathSettingsFileName = "machine.prefs";

        const string PreferredSeatsSettingsFileName = "user.prefs";

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public bool DetectedHandHistoryDirectory { get; protected set; }

        public bool DetectedPreferredSeats { get; protected set; }

        public string HandHistoryDirectory { get; protected set; }

        public bool PokerRoomIsInstalled { get; protected set; }

        public bool PokerRoomSavesPreferredSeats
        {
            get { return true; }
        }

        public IDictionary<int, int> PreferredSeats { get; protected set; }

        /// <summary>
        /// When AutoRotate is selected, Full Tilt will seat the player in the bottom Center which corresponds to the following seats for the different max players:
        /// Max     CenterSeat
        ///  2      1
        ///  5      3
        ///  6      3
        ///  7      4
        ///  8      4
        ///  9      5
        /// </summary>
        /// <remarks>
        /// Settings Element found inside "*-user.prefs"
        /// <KEY Name="AutoRotate">
        /// <BOOL Value="true"/> 
        /// </KEY>
        /// </remarks>
        /// <param name="settings"></param>
        public void DetectPreferredSeats(string settings)
        {
            const string autoRotatePattern = "\\<.*KEY.+Name=\\\"AutoRotate\\\".*>.*\\n\r\n\\<BOOL.+Value=.*\\\"(?<AutoRotate>(true|false))\".* />";

            Match match = Regex.Match(settings, autoRotatePattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

            DetectedPreferredSeats = match.Success;

            if (DetectedPreferredSeats)
            {
                var autoRotateIsTrue = bool.Parse(match.Groups["AutoRotate"].Value);
                if (autoRotateIsTrue)
                    PreferredSeats = new Dictionary<int, int>
                        {
                            { 2, 1 }, 
                            { 5, 3 }, 
                            { 6, 3 }, 
                            { 7, 4 }, 
                            { 8, 4 }, 
                            { 9, 5 }, 
                        };
                else
                    PreferredSeats = new Dictionary<int, int>
                        {
                            { 2, 0 }, 
                            { 5, 0 }, 
                            { 6, 0 }, 
                            { 7, 0 }, 
                            { 8, 0 }, 
                            { 9, 0 }, 
                        };
            }
        }

        public IPokerRoomDetective Investigate()
        {
            var applicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + AppFolder;

            if (Directory.Exists(applicationDataFolder))
            {
                PokerRoomIsInstalled = true;
                TryToExtractSettingsFrom(applicationDataFolder);
            }

            return this;
        }

        static string GetUserPrefSettings(string settingsFolder)
        {
            var userPrefsPath = string.Format("{0}\\{1}", settingsFolder, PreferredSeatsSettingsFileName);

            try
            {
                if (File.Exists(userPrefsPath))
                    return File.ReadAllText(userPrefsPath);

                return string.Empty;
            }
            catch (Exception excep)
            {
                Log.Error(excep);
                return string.Empty;
            }
        }

        void DetectHandHistoryPath(string settingsFolder)
        {
            var machinePrefsPath = string.Format("{0}\\{1}", settingsFolder, HandHistoryPathSettingsFileName);

            if (File.Exists(machinePrefsPath))
            {
                try
                {
                    var settings = File.ReadAllText(machinePrefsPath);

                    Match match = Regex.Match(settings, HandHistoryFolderPattern, RegexOptions.IgnoreCase);

                    DetectedHandHistoryDirectory = match.Success;

                    if (DetectedHandHistoryDirectory)
                        HandHistoryDirectory = match.Groups["HandHistoryPath"].Value;
                }
                catch (Exception excep)
                {
                    Log.Error(excep);
                    DetectedHandHistoryDirectory = false;
                    HandHistoryDirectory = null;
                }
            }
        }

        void TryToExtractSettingsFrom(string settingsFolder)
        {
            DetectHandHistoryPath(settingsFolder);

            var userPrefSettings = GetUserPrefSettings(settingsFolder);
            DetectPreferredSeats(userPrefSettings);
        }
    }
}