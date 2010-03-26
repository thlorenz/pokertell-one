namespace PokerTell.Plugins.PlayerPeeker.ManualTests
{
    using System.Drawing;
    using System.Windows.Forms;

    using Infrastructure.Interfaces;

    using Moq;

    using NUnit.Framework;

    using PokerTell.PlayerPeeker;

    // Resharper disable InconsistentNaming
    public class PlayerPeekerFormTests
    {
        [Test]
        [RequiresSTA]
        public void LaunchPlayerPeekerForm_PassItSomeNewlyFoundPlayers_ShouldRetrieveTheirData()
        {
            // Login first, then move the form to start retrieving, reset to close
            bool wasClosed = false;

            var settings_Stub = new Mock<ISettings>();
            settings_Stub
                .Setup(s => s.RetrievePoint(It.IsAny<string>()))
                .Returns(new Point(0, 0));
            settings_Stub
                .Setup(s => s.RetrieveSize(It.IsAny<string>(), It.IsAny<Size>()))
                .Returns(new Size(400, 300));

            var playersStub = new[] { "salemorguy", "Greystoke-11" };

            var sut = new PlayerPeekerForm(settings_Stub.Object) { TopMost = true };
            sut.FormClosed += (s, e) => wasClosed = true;
            sut.SizeChanged += (s, e) => sut.NewPlayersFound(new[] { "renniweg" });
            sut.ResetRequested += () => wasClosed = true;
            sut.Show();

            sut.NewPlayersFound(playersStub);
            
            while (! wasClosed)
            {
                Application.DoEvents();
            }

        }
    }
}