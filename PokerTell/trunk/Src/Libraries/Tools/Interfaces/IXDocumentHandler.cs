namespace Tools.Interfaces
{
    using System.Xml.Linq;

    public interface IXDocumentHandler
    {
        void Save(XDocument xmlDoc);

        XDocument Load();
    }
}