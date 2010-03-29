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

        const string HandHistoryFolderPattern = @"SaveMyHandsPath=(?<HandHistoryPath>.+)";

        const string AppFolder = @"\PokerStars";

        const string SettingsFileName = "user.ini";

        public bool PokerRoomIsInstalled { get; protected set; }

        public bool PokerRoomSavesPreferredSeats
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
                PokerRoomIsInstalled = true;
                TryToExtractSettingsFrom(applicationDataFolder);
            }
            else
            {
                var installationFolder = FindInstallationFolderInRegistry();
                if (installationFolder != null)
                {
                    PokerRoomIsInstalled = true;
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

                    DetectPreferredSeats(settings);
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
        /// PokerStars: saves the preferred seats as follows
        /// SeatPref=-1 -1 -1 4 -1 -1 -1 
        /// These values correspond in order to the following total seats at the table:
        /// 2 6 8 9 10 4 7
        /// -1 means no preference
        ///  0 means Seat 1 preference
        ///  1 means Seat 2 preference etc.
        /// So after detecting the values we add 1 since we use 0 for no preferred seat and 1 for Seat1 etc.
        /// </summary>
        public void DetectPreferredSeats(string settings)
        {
            const string preferredSeatsPattern =
                @"SeatPref=(?<TP2>-*\d) (?<TP6>-*\d) (?<TP8>-*\d) (?<TP9>-*\d) (?<TP10>-*\d) (?<TP4>-*\d) (?<TP7>-*\d)";

            Match match = Regex.Match(settings, preferredSeatsPattern);

            DetectedPreferredSeats = match.Success;
            if (DetectedPreferredSeats)
            {
                try
                {
                    PreferredSeats = new Dictionary<int, int>
                        {
                            { 2, int.Parse(match.Groups["TP2"].Value) + 1 }, 
                            { 6, int.Parse(match.Groups["TP6"].Value) + 1 }, 
                            { 8, int.Parse(match.Groups["TP8"].Value) + 1 }, 
                            { 9, int.Parse(match.Groups["TP9"].Value) + 1 }, 
                            { 10, int.Parse(match.Groups["TP10"].Value) + 1 }, 
                            { 4, int.Parse(match.Groups["TP4"].Value) + 1 }, 
                            { 7, int.Parse(match.Groups["TP7"].Value) + 1 }
                        };
                }
                catch (Exception excep)
                {
                    Log.Error(excep);
                    DetectedPreferredSeats = false;
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
            const bool toRead = false;

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