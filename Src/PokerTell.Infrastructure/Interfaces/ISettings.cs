using System.Drawing;

namespace PokerTell.Infrastructure.Interfaces
{
    public interface ISettings : IUserConfiguration
    {
        int RetrieveInt(string key);

        int RetrieveInt(string key, int defaultValue);

        bool RetrieveBool(string key);

        bool RetrieveBool(string key, bool defaultValue);

        double RetrieveDouble(string key);

        double RetrieveDouble(string key, double defaultValue);

        string RetrieveString(string key);

        string RetrieveString(string key, string defaultValue);

        Point RetrievePoint(string key);

        Point RetrievePoint(string key, Point defaultValue);

        Size RetrieveSize(string key);

        Size RetrieveSize(string key, Size defaultValue);

        Color RetrieveColor(string key);

        Color RetrieveColor(string key, Color defaultValue);

        void Set(string strKey, object objValue);

        /// <summary>
        /// Provides string representation of all (key,value) pairs in settings
        /// </summary>
        /// <returns>String representation of all (key,value) pairs</returns>
        string ShowAll();
    }
}