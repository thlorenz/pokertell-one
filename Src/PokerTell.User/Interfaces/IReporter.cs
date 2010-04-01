namespace PokerTell.User.Interfaces
{
    using Infrastructure.Interfaces;

    public interface IReporter : IFluentInterface
    {
        string LogfileContent { get; }

        string ScreenShotFile { get; }

        IReporter DeleteReportingTempDirectory();

        IReporter PrepareReport();

        IReporter SendReport(string caption, string comments, bool includeScreenshot);
    }
}