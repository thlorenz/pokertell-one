using System.Drawing;

namespace PokerTell.Infrastructure.Interfaces
{
    public interface ISettings : IUserConfiguration
    {
        void Persist(string strKey, out int objValue);

        void Persist(string strKey, out int objValue, int objDefault);

        void Persist(string strKey, out bool objValue);

        void Persist(string strKey, out bool objValue, bool objDefault);

        void Persist(string strKey, out double objValue);

        void Persist(string strKey, out double objValue, double objDefault);

        void Persist(string strKey, out string objValue);

        void Persist(string strKey, out string objValue, string objDefault);

        void Persist(string strKey, out Point objValue);

        void Persist(string strKey, out Point objValue, Point objDefault);

        void Persist(string strKey, out Size objValue);

        void Persist(string strKey, out Size objValue, Size objDefault);

        void Persist(string strKey, out Color objValue);

        void Persist(string strKey, out Color objValue, Color objDefault);

        void Save(string strKey, object objValue);

        /// <summary>
        /// Provides string representation of all (key,value) pairs in settings
        /// </summary>
        /// <returns>String representation of all (key,value) pairs</returns>
        string ShowAll();
    }
}