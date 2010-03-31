namespace PokerTell.LiveTracker.Persistence
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml.Linq;

    using Infrastructure;

    using Interfaces;

    using log4net;

    public class LiveTrackerSettingsXDocumentHandler : ILiveTrackerSettingsXDocumentHandler
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const string fileName = "LiveTrackerSettings.xml";

        readonly string _fullPath = Files.LocalUserAppDataPath + string.Format("\\{0}", fileName);            

        public void Save(XDocument xmlDoc)
        {
            xmlDoc.Save(_fullPath);
        }

        /// <summary>
        /// Attempts to load the live tracker settings from the "LiveTrackerSettings.xml" in the AppData directory.
        /// </summary>
        /// <returns>null if the file was not found or an exception occurs</returns>
        public XDocument Load()
        {
            if (! new FileInfo(_fullPath).Exists)
            {
                Log.DebugFormat("Attempted to load from file {0}, but doesn't exist. Returning null.", _fullPath);
                return null;
            }
            try
            {
                return XDocument.Load(_fullPath);
            }
            catch (Exception excep)
            {
                Log.Error(excep);
                return null;
            }
        }
    }
}