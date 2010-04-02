namespace PokerTell.User.ViewModels
{
    using System.Diagnostics;
    using System.Windows.Input;

    using Infrastructure;
    using Infrastructure.Properties;

    using Tools.WPF;

    public class AboutViewModel
    {
        public AboutViewModel()
        {
            ApplicationName = ApplicationProperties.ApplicationName;
            ApplicationAuthor = ApplicationProperties.Author;
            ApplicationVersion = ApplicationProperties.Version;

            ThanksTo = "Riva Capistrano, Arne Rudnick \nand the stackoverflow community";
        }

        public string ApplicationName { get; protected set; }

        public string ApplicationVersion { get; protected set; }

        public string ApplicationAuthor { get; protected set; }

        public string ThanksTo { get; protected set; }

        ICommand _browseToHomepageCommand;

        public ICommand BrowseToHomepageCommand
        {
            get
            {
                return _browseToHomepageCommand ?? (_browseToHomepageCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => Process.Start(Resources.Links_Homepage),
                    });
            }
        }
    }
}