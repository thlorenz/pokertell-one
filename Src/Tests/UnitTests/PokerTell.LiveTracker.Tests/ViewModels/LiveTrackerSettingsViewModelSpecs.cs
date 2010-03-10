namespace PokerTell.LiveTracker.Tests.ViewModels
{
    using System.Xml.Linq;

    using Machine.Specifications;

    using Moq;

    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.Tests.Fakes;
    using PokerTell.LiveTracker.ViewModels;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class LiveTrackerSettingsViewModelSpecs
    {
        protected static LiveTrackerSettingsXDocumentHandlerMock _xDocumentHandler_Mock;

        protected static ILiveTrackerSettingsViewModel _sut;

        Establish specContext = () => {
            _xDocumentHandler_Mock = new LiveTrackerSettingsXDocumentHandlerMock();
            _sut = new LiveTrackerSettingsViewModel(_xDocumentHandler_Mock);
        };

        [Subject(typeof(LiveTrackerSettingsViewModel), "Load")]
        public class when_x_document_handler_returns_existing_and_not_existing_path_as_hand_history_file_paths : LiveTrackerSettingsViewModelSpecs
        {
            const string existingPath = @"C:\";

            const string notExisitingPath = "doesn't exist";

            Establish context = () => {
                var settings = new LiveTrackerSettingsViewModel(new Mock<ILiveTrackerSettingsXDocumentHandler>().Object)
                    { HandHistoryFilesPaths = new[] { existingPath, notExisitingPath } };

                _xDocumentHandler_Mock.DocumentToLoad = LiveTrackerSettingsViewModel.CreateXDocumentFor(settings);
            };

            Because of = () => _sut.LoadSettings();

            It should_add_the_existing_path_to_the_HandHistoryFilesPaths = () => _sut.HandHistoryFilesPaths.ShouldContain(existingPath);

            It should_not_add_the_not_existing_path_to_the_HandHistoryFilesPaths = () => _sut.HandHistoryFilesPaths.ShouldNotContain(notExisitingPath);
        }

        [Subject(typeof(LiveTrackerSettingsViewModel), "LoadSettings")]
        public class when_the_returned_XDocument_is_null : LiveTrackerSettingsViewModelSpecs
        {
            Establish context = () => _xDocumentHandler_Mock.DocumentToLoad = null;

            Because of = () => _sut.LoadSettings();

            It should_load_the_settings_from_the_XDoxument_Handler = () => _xDocumentHandler_Mock.DocumentWasLoaded.ShouldBeTrue();

            It should_set_AutoTrack_to_true = () => _sut.AutoTrack.ShouldBeTrue();

            It should_set_ShowLiveStatsWindowOnStartup_to_true = () => _sut.ShowLiveStatsWindowOnStartup.ShouldBeTrue();

            It should_set_ShowTableOverlay_to_true = () => _sut.ShowTableOverlay.ShouldBeTrue();

            It should_set_HoleCardsDuration_to_5 = () => _sut.ShowHoleCardsDuration.ShouldEqual(5);

            It should_set_HistoryPaths_to_empty = () => _sut.HandHistoryFilesPaths.ShouldBeEmpty();
        }

        [Subject(typeof(LiveTrackerSettingsViewModel), "LoadSettings")]
        public class when_all_boolean_properties_were_saved_as_true_and_ShowHoleCardsDuration_Is_1 : LiveTrackerSettingsViewModelSpecs
        {
            const bool setTrue = true;

            const int duration = 1;

            Establish context = () => {
                var settings = new LiveTrackerSettingsViewModel(new Mock<ILiveTrackerSettingsXDocumentHandler>().Object)
                    {
                        AutoTrack = setTrue, 
                        ShowLiveStatsWindowOnStartup = setTrue, 
                        ShowTableOverlay = setTrue, 
                        ShowHoleCardsDuration = duration
                    };

                _xDocumentHandler_Mock.DocumentToLoad = LiveTrackerSettingsViewModel.CreateXDocumentFor(settings);
            };

            Because of = () => _sut.LoadSettings();

            It should_load_the_settings_from_the_XDoxument_Handler = () => _xDocumentHandler_Mock.DocumentWasLoaded.ShouldBeTrue();

            It should_set_AutoTrack_to_true = () => _sut.AutoTrack.ShouldBeTrue();

            It should_set_ShowLiveStatsWindowOnStartup_to_true = () => _sut.ShowLiveStatsWindowOnStartup.ShouldBeTrue();

            It should_set_ShowTableOverlay_to_true = () => _sut.ShowTableOverlay.ShouldBeTrue();

            It should_set_HoleCardsDuration_to_1 = () => _sut.ShowHoleCardsDuration.ShouldEqual(duration);
        }

        [Subject(typeof(LiveTrackerSettingsViewModel), "LoadSettings")]
        public class when_all_boolean_properties_were_saved_as_false_and_ShowHoleCardsDuration_Is_2 : LiveTrackerSettingsViewModelSpecs
        {
            const bool setFalse = false;

            const int duration = 2;

            Establish context = () => {
                var settings = new LiveTrackerSettingsViewModel(new Mock<ILiveTrackerSettingsXDocumentHandler>().Object)
                    {
                        AutoTrack = setFalse, 
                        ShowLiveStatsWindowOnStartup = setFalse, 
                        ShowTableOverlay = setFalse, 
                        ShowHoleCardsDuration = duration
                    };

                _xDocumentHandler_Mock.DocumentToLoad = LiveTrackerSettingsViewModel.CreateXDocumentFor(settings);
            };

            Because of = () => _sut.LoadSettings();

            It should_load_the_settings_from_the_XDoxument_Handler = () => _xDocumentHandler_Mock.DocumentWasLoaded.ShouldBeTrue();

            It should_set_AutoTrack_to_false = () => _sut.AutoTrack.ShouldBeFalse();

            It should_set_ShowLiveStatsWindowOnStartup_to_false = () => _sut.ShowLiveStatsWindowOnStartup.ShouldBeFalse();

            It should_set_ShowTableOverlay_to_false = () => _sut.ShowTableOverlay.ShouldBeFalse();

            It should_set_HoleCardsDuration_to_2 = () => _sut.ShowHoleCardsDuration.ShouldEqual(duration);
        }

        [Subject(typeof(LiveTrackerSettingsViewModel), "Saving Settings")]
        public class when_the_user_executes_SaveComand : LiveTrackerSettingsViewModelSpecs
        {
            static XDocument expectedDocument;

            Establish context = () => {
                _sut.HandHistoryFilesPaths = new[] { "somePath" };
                expectedDocument = LiveTrackerSettingsViewModel.CreateXDocumentFor(_sut);
            };

            Because of = () => _sut.SaveSettingsCommand.Execute(null);

            It should_save_the_settings = () => _xDocumentHandler_Mock.DocumentWasSaved.ShouldBeTrue();

            It the_saved_xDocument_should_contain_the_correct_settings
                = () => _xDocumentHandler_Mock.SavedDocument.ToString().ShouldEqual(expectedDocument.ToString());

        }
    }
}