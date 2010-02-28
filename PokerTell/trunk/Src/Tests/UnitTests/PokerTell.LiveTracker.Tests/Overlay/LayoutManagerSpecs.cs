namespace PokerTell.LiveTracker.Tests.Overlay
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    using Machine.Specifications;

    using Moq;

    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.Overlay;

    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    using Utilities;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class LayoutManagerSpecs
    {
        protected static ILayoutManager _sut;

        protected static LayoutXDocumentHandlerMock _xDocumentHandler_Mock;

        Establish specContext = () => {
            _xDocumentHandler_Mock = new LayoutXDocumentHandlerMock { DocumentToLoad = XDocumentStubWith_5max_6max() };
            _sut = new LayoutManager(_xDocumentHandler_Mock);
        };


        [Subject(typeof(LayoutManager), "Load")]
        public class when_asked_to_get_the_settings_for_the_site_PokerStars_and_TotalSeats_6 : LayoutManagerSpecs
        {
            const int seats = 6;

            const string site = "PokerStars";

            static ITableOverlaySettingsViewModel returnedSettings; 

            Because of = () => returnedSettings = _sut.Load(site, seats);

            It should_set_the_PokerSite_of_the_documentHandler_to___PokerStars__ = () => _xDocumentHandler_Mock.PokerSite.ShouldEqual(site);

            It should_load_the_XDocument_from_the_documentHandler = () => _xDocumentHandler_Mock.DocumentWasLoaded.ShouldBeTrue();

            It should_return_the_Settings_for_6_TotalSeats_contained_in_the_XDocument
                = () => returnedSettings.ToString().ShouldEqual(Utils.GetOverlaySettings_6Max().ToString());
        }

        [Subject(typeof(LayoutManager), "Save")]
        public class when_asked_to_save_the_settings_for_PokerStars_and_5_total_seats : LayoutManagerSpecs
        {
            const string site = "PokerStars";

            static ITableOverlaySettingsViewModel settings;

            Establish context = () => {
                settings = Utils.GetOverlaySettings_5Max();
                settings.HarringtonMPositions[0] = new PositionViewModel(1.0, 1.0);
            };

            Because of = () => _sut.Save(settings, site);

            It should_set_the_PokerSite_of_the_documentHandler_to___PokerStars__ = () => _xDocumentHandler_Mock.PokerSite.ShouldEqual(site);

            It should_load_the_XDocument_from_the_documentHandler = () => _xDocumentHandler_Mock.DocumentWasLoaded.ShouldBeTrue();

            It should_save_the_XDocument_since_it_was_changed = () => _xDocumentHandler_Mock.DocumentWasSaved.ShouldBeTrue();

            It should_have_replaced_the_settings_for_5_total_seats = () => {
                _xDocumentHandler_Mock.DocumentToLoad = _xDocumentHandler_Mock.SavedDocument;
                _sut.Load(site, 5).ToString().ShouldNotEqual(Utils.GetOverlaySettings_5Max().ToString());
                _sut.Load(site, 5).ToString().ShouldEqual(settings.ToString());
            };

            It should_not_have_changed_the_settings_for_6_total_seats = () => {
                _xDocumentHandler_Mock.DocumentToLoad = _xDocumentHandler_Mock.SavedDocument;
                _sut.Load(site, 6).ToString().ShouldEqual(Utils.GetOverlaySettings_6Max().ToString());
            };
        }

        [Subject(typeof(LayoutManager), "Save, settings not saved before")]
        public class when_asked_to_save_the_settings_for_PokerStars_and_2_total_seats : LayoutManagerSpecs
        {
            const string site = "PokerStars";

            static ITableOverlaySettingsViewModel settings;

            Establish context = () => settings = Utils.GetOverlaySettings_2Max();

            Because of = () => _sut.Save(settings, site);

            It should_have_added_the_settings_for_2_total_seats
                = () => _sut.Load(site, 2).ToString().ShouldEqual(settings.ToString());

            It should_not_have_changed_the_settings_for_6_total_seats = () => {
                _xDocumentHandler_Mock.DocumentToLoad = _xDocumentHandler_Mock.SavedDocument;
                _sut.Load(site, 6).ToString().ShouldEqual(Utils.GetOverlaySettings_6Max().ToString());
            };
        }

        static XDocument XDocumentStubWith_5max_6max()
        {
            var xmlDoc = new XDocument();
            xmlDoc.Add(
                new XElement("Layouts", 
                             LayoutManager.CreateXElementFor(Utils.GetOverlaySettings_5Max()),
                             LayoutManager.CreateXElementFor(Utils.GetOverlaySettings_6Max())));
            return xmlDoc;
        }
    }

    public class LayoutXDocumentHandlerMock : ILayoutXDocumentHandler
    {
        public void Save(XDocument xmlDoc)
        {
            DocumentWasSaved = true;
            SavedDocument = xmlDoc;
        }

        public XDocument SavedDocument { get; set; }

        public XDocument DocumentToLoad { get; set; }

        public XDocument Load()
        {
            DocumentWasLoaded = true;
            return DocumentToLoad;
        }

        public bool DocumentWasLoaded { get; set; }

        public bool DocumentWasSaved { get; set; }

        public string PokerSite { get; set; }
    }
}