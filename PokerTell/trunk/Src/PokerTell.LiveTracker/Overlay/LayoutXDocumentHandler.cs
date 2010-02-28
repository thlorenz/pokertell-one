namespace PokerTell.LiveTracker.Overlay
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml.Linq;

    using Infrastructure;

    using Interfaces;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;

    public class LayoutXDocumentHandler : ILayoutXDocumentHandler
    {
        static readonly string LayoutPath = Files.AppDataDirectory + @"\layouts\";
        static readonly string DefaultLayoutsPath = LayoutPath + @"defaults\";

        public void Save(XDocument xmlDoc)
        {
            string fileName = DetermineFileName();
            
            var fullPathToCustomLayout = LayoutPath + fileName;

            xmlDoc.Save(fullPathToCustomLayout);
        }

        public XDocument Load()
        {
            string fileName = DetermineFileName();
            
            var fullPathToCustomLayout = new FileInfo(LayoutPath + fileName);
            var fullPathToDefaultLayout = new FileInfo(DefaultLayoutsPath + fileName);

            var usedFullPath = fullPathToCustomLayout.Exists ? fullPathToCustomLayout.FullName : fullPathToDefaultLayout.FullName;

            return XDocument.Load(usedFullPath);
        }

        string DetermineFileName()
        {
            if (string.IsNullOrEmpty(PokerSite))
                throw new ArgumentException("Initialize PokerSite first!");

            return PokerSite.ToLower().Match()
                .With(site => site == "pokerstars", _ => "PokerStars.xml")
                .With(site => site == "fulltilt", _ => "FullTilt.xml")
                .Do();
        }

        public string PokerSite { get; set; }
    }
}