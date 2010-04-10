namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using System.Drawing;

    using Infrastructure.Interfaces.PokerHand;

    using Moq;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.LiveTracker.Design.PokerHand;
    using PokerTell.LiveTracker.ViewModels.Overlay;

    using Tools.Interfaces;
    using Tools.Validation;
    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    public class GameHistoryDesignModel : GameHistoryViewModel
    {
        public GameHistoryDesignModel()
            : base(SettingsStub, new DimensionsViewModel(), HandHistoryDesign.Model, new DispatcherTimerAdapter(), new CollectionValidator())
        {
        }

        static ISettings SettingsStub
        {
          get
          {
            var settingsStub = new Mock<ISettings>();  
            settingsStub
                .Setup(s => s.RetrieveRectangle(It.IsAny<string>(), It.IsAny<Rectangle>()))
                .Returns(new Rectangle(0, 0, 600, 400));

            return settingsStub.Object;
          } 
        } 
    }
}