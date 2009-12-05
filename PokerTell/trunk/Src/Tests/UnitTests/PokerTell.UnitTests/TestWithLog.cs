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
        #region Constants and Fields

        ConsoleAppender _appender;

        #endregion

        #region Public Methods

        [TestFixtureTearDown]
        public void CleanupLog()
        {
            DisableLogger();
        }

        [TestFixtureSetUp]
        public void InitLog()
        {
            InitAppender();

            EnableLogger();
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

        #endregion

        #region Methods

        public void DisableLogger()
        {
            SetThreshold(Level.Off);
        }

        public void EnableLogger()
        {
            SetThreshold(Level.All);
        }

        public void SetThreshold(Level level)
        {
            if (_appender == null)
            {
                InitAppender();
            }

            _appender.Threshold = level;
        }

        public void NotLogged(Action unloggedCodeBlock)
        {
            DisableLogger();

            unloggedCodeBlock.Invoke();

            EnableLogger();
        }

        #endregion
    }
}