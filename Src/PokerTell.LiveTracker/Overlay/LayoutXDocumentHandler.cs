namespace PokerTell.LiveTracker.Overlay
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml.Linq;

    using log4net;

    using PokerTell.Infrastructure;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.PokerRooms;
    using PokerTell.LiveTracker.Properties;

    using Tools.FunctionalCSharp;

    public class LayoutXDocumentHandler : ILayoutXDocumentHandler
    {
        static readonly string LayoutPath = Files.LocalUserAppDataPath + @"\layouts\";

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string PokerSite { get; set; }

        /// <summary>
        /// Tries to read a custom layout from the application data folder
        /// If unsuccesfull it falls back on the default layout file in the resources
        /// If the defautl resource cannot be found it returns null
        /// </summary>
        public XDocument Load()
        {
            string fileName = DetermineFileName();

            var fullPathToCustomLayout = new FileInfo(LayoutPath + fileName);

            try
            {
                if (fullPathToCustomLayout.Exists)
                    return XDocument.Load(fullPathToCustomLayout.FullName);
            }
            catch (Exception excep)
            {
                Log.Error(excep);
            }

            // If the custom layout was not found or we encountered an error when loading it we will use the default layout from the resources
            var defaultLayoutResource = Resources.ResourceManager.GetString(fileName.Replace(".xml", "Layout"));
            
            return defaultLayoutResource != null ? XDocument.Parse(defaultLayoutResource) : null;
        }

        public void Save(XDocument xmlDoc)
        {
            string fileName = DetermineFileName();

            // Needed for first run of app and in case the user decides to delete the layout path
            if (!Directory.Exists(LayoutPath))
                Directory.CreateDirectory(LayoutPath);

            var fullPathToCustomLayout = LayoutPath + fileName;

            xmlDoc.Save(fullPathToCustomLayout);
        }

        string DetermineFileName()
        {
            if (string.IsNullOrEmpty(PokerSite))
                throw new ArgumentException("Initialize PokerSite first!");

            return PokerSite.ToLower().Match()
                .With(site => site == new PokerStarsInfo().Site.ToLower(), _ => "PokerStars.xml")
                .With(site => site == new FullTiltPokerInfo().Site.ToLower(), _ => "FullTiltPoker.xml")
                .Do();
        }
    }
}