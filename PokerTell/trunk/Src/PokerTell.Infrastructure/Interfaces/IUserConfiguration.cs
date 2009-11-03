using System.Configuration;

namespace PokerTell.Infrastructure.Interfaces
{
    public interface IUserConfiguration
    {
        ConnectionStringSettingsCollection ConnectionStrings { get; }

        KeyValueConfigurationCollection AppSettings { get; }

        void Save(ConfigurationSaveMode saveMode);
    }
}