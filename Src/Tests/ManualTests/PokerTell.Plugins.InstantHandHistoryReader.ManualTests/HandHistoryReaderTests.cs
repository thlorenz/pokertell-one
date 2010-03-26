namespace PokerTell.Plugins.InstantHandHistoryReader.ManualTests
{
    using System;

    using NUnit.Framework;

    using Tools.FunctionalCSharp;

    /// <summary>
    /// These tests work only with the appropriate PokerClient running and at least one table open
    /// </summary>
    public class HandHistoryReaderTests 
    {
        [Test]
        public void FindNewInstantHandHistories_PokerStarsRunningAndConstructedWithPokerStarsStartingString_ReturnsReadHandHistories()
        {
            const string processName = "pokerstars";
            const string pokerStarsHandHistoryStartingValue = "PokerStars Game #";

            var sut = new HandHistoryReader().InitializeWith(processName, pokerStarsHandHistoryStartingValue);

            var readHandHistories = sut.FindNewInstantHandHistories();

            readHandHistories.ForEach(h => Console.WriteLine("{0}\n\n", h));
        }

        [Test]
        public void FindNewInstantHandHistories_FullTiltPokerRunningAndConstructedWithFullTiltStartingString_ReturnsNoReadHandHistories()
        {
            // Right now this test shows that we are unable to read Full Tilt instant HandHistories
            // This could be either because FullTilt encrypts their HandHistories in Memory or because the HandHistoryReader needs to be adapted
            // in some way to work for Full Tilt as well.
            const string processName = "fulltiltpoker";
            const string pokerStarsHandHistoryStartingValue = "Full Tilt Poker Game #";

            var sut = new HandHistoryReader().InitializeWith(processName, pokerStarsHandHistoryStartingValue);

            var readHandHistories = sut.FindNewInstantHandHistories();

            readHandHistories.ForEach(h => Console.WriteLine("{0}\n\n", h));
        }
    }
}