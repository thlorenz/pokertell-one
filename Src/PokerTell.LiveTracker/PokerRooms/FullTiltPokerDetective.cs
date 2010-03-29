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
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const string HandHistoryFolderPattern = "<WSTRING Value=\"(?<HandHistoryPath>.+)\"";

        const string AppFolder = @"\FullTiltPoker";

        const string SettingsFileName = "machine.prefs";

        public bool PokerRoomIsInstalled { get; protected set; }

        public bool PokerRoomSavesPreferredSeats
        {
            get { return false; }
        }

        public bool DetectedPreferredSeats { get; protected set; }

        public bool DetectedHandHistoryDirectory { get; protected set; }

        public string HandHistoryDirectory { get; protected set; }

        public IDictionary<int, int> PreferredSeats { get; protected set; }

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

        void TryToExtractSettingsFrom(string settingsFolder)
        {
            var machinePrefsPath = string.Format("{0}\\{1}", settingsFolder, SettingsFileName);

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
    }
}