namespace PokerTell.LiveTracker.ManualTests.WindowFinder
{
    using System;
    using System.Text.RegularExpressions;

    using NUnit.Framework;

    using Overlay;

    using PokerRooms;

    public class FindFullTiltWindowTests
    {
        WindowFinder _windowFinder;

        [SetUp]
        public void _Init()
        {
            _windowFinder = new WindowFinder();
        }

        [Test]
        public void FindWindow_ProcessNameIsGeneral_FindsIt()
        {
            const string tableName = ".COM Play 1666 (6 max)";
            string processName = new FullTiltPokerInfo().ProcessName;

            _windowFinder.FindWindowMatching(
                                      new Regex(Regex.Escape(tableName)),
                                      new Regex(processName), 
                                      ptr => {
                                          Console.WriteLine("FoundWindow with Handle: {0}", ptr);
                                          return true;
                                      });
        }
    }
}