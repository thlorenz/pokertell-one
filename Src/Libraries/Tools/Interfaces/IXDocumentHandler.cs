namespace Tools.Interfaces
{
    using System.Xml.Linq;

    public interface IXDocumentHandler
    {
        void Save(XDocument xmlDoc);

        /// <summary>
        /// Tries to read a custom layout from the application data folder
        /// If unsuccesfull it falls back on the default layout file in the resources
        /// If the defautl resource cannot be found it returns null
        /// </summary>
        XDocument Load();
    }
}