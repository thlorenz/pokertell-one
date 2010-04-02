namespace PokerTell.User.Views
{
    using System.Windows.Controls;
    using System.Windows.Input;

    using Infrastructure.Interfaces;

    using Interfaces;

    using Properties;

    using Tools.WPF;

    public class UserMenuItemFactory
    {
        public UserMenuItemFactory(IConstructor<IReportWindowManager> reportWindowMake)
        {
            _reportWindowMake = reportWindowMake;
        }

        public MenuItem Create()
        {
            var menuItem = new MenuItem { Header = Resources.User_MenuItem_Title };
            menuItem.Items.Add(new MenuItem { Header = Resources.User_MenuItem_ReportAProblem, Command = ReportProblemCommand });
            menuItem.Items.Add(new Separator());
            menuItem.Items.Add(new MenuItem { Header = Resources.User_MenuItem_About, Command = ShowAboutCommand });
            return menuItem;
        }

        ICommand _reportProblemCommand;

        public ICommand ReportProblemCommand
        {
            get
            {
                return _reportProblemCommand ?? (_reportProblemCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            var reportWindow = _reportWindowMake.New;
                            reportWindow.Subject = "User Report";
                            reportWindow.ShowDialog();
                        }
                    });
            }
        }

        ICommand _showAboutCommand;

        readonly IConstructor<IReportWindowManager> _reportWindowMake;

        public ICommand ShowAboutCommand
        {
            get
            {
                return _showAboutCommand ?? (_showAboutCommand = new SimpleCommand {
                        ExecuteDelegate = arg => new AboutView().ShowDialog()
                    });
            }
        }
    }
}