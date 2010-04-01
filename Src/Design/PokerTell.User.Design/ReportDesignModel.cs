namespace PokerTell.User.Design
{
    using Moq;

    using PokerTell.User.Interfaces;
    using PokerTell.User.ViewModels;

    public class ReportDesignModel : ReportViewModel
    {
        public ReportDesignModel()
            : base(new Mock<IReporter>().Object)
        {
            Comments = "An error occurred while: ";
            IncludeScreenshot = false;
            LogFileContent = "Lots of stuff logged here.";
            ScreenshotPath = @"C:\Documents and Settings\Owner.LapThor\Application Data\PokerTell\temp\screenshot0.jpeg";
        }
    }

    public static class ReportDesign
    {
        public static IReportViewModel Model
        {
            get { return new ReportDesignModel(); }
        }
    }
}