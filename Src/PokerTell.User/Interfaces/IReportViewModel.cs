namespace PokerTell.User.Interfaces
{
    using System.Windows.Input;

    using Infrastructure.Interfaces;

    public interface IReportViewModel : IFluentInterface
    {
        string Comments { get; set; }

        bool IncludeScreenshot { get; set; }

        string LogFileContent { get; }

        string ScreenshotPath { get; }

        ICommand SendReportCommand { get; }
    }
}