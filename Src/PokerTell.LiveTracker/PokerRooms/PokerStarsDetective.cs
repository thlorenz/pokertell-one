namespace PokerTell.LiveTracker.PokerRooms
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using log4net;

    using Microsoft.Win32;

    using PokerTell.LiveTracker.Interfaces;

    public class PokerStarsDetective : IPokerRoomDetective
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const bool toRead = false;

        const string HandHistoryFolderPattern = @"SaveMyHandsPath=(?<HandHistoryPath>.+)";

        const string AppFolder = @"\PokerStars";

        const string SettingsFileName = "user.ini";

        public bool IsInstalled { get; protected set; }

        public bool SavesPreferredSeats
        {
            get { return true; }
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
                IsInstalled = true;
                TryToExtractSettingsFrom(applicationDataFolder);
            }
            else
            {
                var installationFolder = FindInstallationFolderInRegistry();
                if (installationFolder != null)
                {
                    IsInstalled = true;
                    TryToExtractSettingsFrom(installationFolder);
                }
            }

            return this;
        }

        void TryToExtractSettingsFrom(string settingsFolder)
        {
            var userIniPath = string.Format("{0}\\{1}", settingsFolder, SettingsFileName);

            if (File.Exists(userIniPath))
            {
                try
                {
                    var settings = File.ReadAllText(userIniPath);

                    Match match = Regex.Match(settings, HandHistoryFolderPattern);

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

        /// <summary>
        /// The information we need sits at:
        /// HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\PokerStars or
        /// HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\PokerStars
        /// The info we need is the value of the key: InstallLocation
        /// </summary>
        /// <returns>Null if not found or the InstallationDirectory otherwise</returns>
        static string FindInstallationFolderInRegistry()
        {
            try
            {
                var wow6432NodePokerStars =
                    Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\PokerStars", toRead);
                RegistryKey pokerStars = wow6432NodePokerStars ??
                                         Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\PokerStars", toRead);

                if (pokerStars == null) return null;

                return pokerStars.GetValue("InstallLocation").ToString();
            }
            catch (Exception excep)
            {
                Log.Error(excep);
                return null;
            }
        }
    }
}