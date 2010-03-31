namespace PokerTell.UnitTests
{
    using System;

    using log4net.Appender;
    using log4net.Config;
    using log4net.Core;
    using log4net.Layout;

    using NUnit.Framework;

    /// <summary>
    /// Description of TestWithLog.
    /// </summary>
    [TestFixture]
    public class TestWithLog
    {
        ConsoleAppender _appender;

        [TestFixtureTearDown]
        public void CleanupLog()
        {
            DisableLogger();
        }

        public void DisableLogger()
        {
            SetThreshold(Level.Off);
        }

        public void EnableLogger()
        {
            SetThreshold(Level.All);
        }

        [TestFixtureSetUp]
        public void InitLog()
        {
            InitAppender();

            EnableLogger();
        }

        public void NotLogged(Action unloggedCodeBlock)
        {
            DisableLogger();

            unloggedCodeBlock.Invoke();

            EnableLogger();
        }

        public void SetThreshold(Level level)
        {
            if (_appender == null)
            {
                InitAppender();
            }

            _appender.Threshold = level;
        }

        void InitAppender()
        {
            _appender = new ConsoleAppender
                {
                    Layout = new PatternLayout(
                        "%newline%date [%thread] %level %logger - %message%newline")
                };
            BasicConfigurator.Configure(_appender);
        }
    }
}