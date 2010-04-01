namespace PokerTell.User.Tests.ViewModels
{
    using Interfaces;

    using Machine.Specifications;

    using Moq;

    using User.ViewModels;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class ReportViewModelSpecs
    {
        protected static Mock<IReporter> _reporter_Mock;

        protected static IReportViewModel _sut;
       
        protected const string UserComments = "some Comments";

        protected const string LogFileContent = "some Content";

        protected const string ScreenshotPath = "some Path";

        Establish specContext = () => {
            _reporter_Mock = new Mock<IReporter>();
            _reporter_Mock
                .SetupGet(r => r.LogfileContent)
                .Returns(LogFileContent);
            _reporter_Mock
                .SetupGet(r => r.ScreenShotFile)
                .Returns(ScreenshotPath);
        };

        public class Ctx_InstantiatedSut : ReportViewModelSpecs
        {
            Establish instantiatedContext = () => _sut = new ReportViewModel(_reporter_Mock.Object);
        }

        [Subject(typeof(ReportViewModel), "Instantiation")]
        public class when_it_is_instantiated : ReportViewModelSpecs
        {
            Because of = () => _sut = new ReportViewModel(_reporter_Mock.Object);

            It should_tell_the_reporter_to_prepare_the_report = () => _reporter_Mock.Verify(r => r.PrepareReport());

            It should_set_the_logfile_content_to_the_one_returned_by_the_reporter = () => _sut.LogFileContent.ShouldEqual(LogFileContent);

            It should_set_the_screenshot_path_to_the_one_returned_by_the_reporter = () => _sut.ScreenshotPath.ShouldEqual(ScreenshotPath);
        }

        [Subject(typeof(ReportViewModel), "Send Report")]
        public class when_the_user_included_the_screenshot_and_wants_to_send_the_report : Ctx_InstantiatedSut
        {
            const bool includeScreenShot = true;            
            
            Establish context = () => {
                _sut.Comments = UserComments;
                _sut.IncludeScreenshot = includeScreenShot;
            };

            Because of = () => _sut.SendReportCommand.Execute(null);

            It should_tell_the_reporter_to_send_the_report_including_the_screenshot
                = () => _reporter_Mock.Verify(r => r.SendReport(Moq.It.IsAny<string>(), UserComments, includeScreenShot));
        }

        [Subject(typeof(ReportViewModel), "Send Report")]
        public class when_the_user_did_not_include_the_screenshot_and_wants_to_send_the_report : Ctx_InstantiatedSut
        {
            const bool includeScreenShot = false;            
            
            Establish context = () => {
                _sut.Comments = UserComments;
                _sut.IncludeScreenshot = includeScreenShot;
            };

            Because of = () => _sut.SendReportCommand.Execute(null);

            It should_tell_the_reporter_to_send_the_report_without_the_screenshot
                = () => _reporter_Mock.Verify(r => r.SendReport(Moq.It.IsAny<string>(), UserComments, includeScreenShot));
        }
    }
}