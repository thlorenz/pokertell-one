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
    public abstract class TestWithLog
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
            _appender = new ConsoleAppender
                {
                    Layout = new PatternLayout(
                        "%newline%date [%thread] %level %logger - %message%newline")
                };

            EnableLogger();

            BasicConfigurator.Configure(_appender);
        }

        #endregion

        #region Methods

        protected void DisableLogger()
        {
            _appender.Threshold = Level.Off;
        }

        protected void EnableLogger()
        {
            _appender.Threshold = Level.All;
        }

        protected void NotLogged(Action unloggedCodeBlock)
        {
            DisableLogger();

            unloggedCodeBlock.Invoke();

            EnableLogger();
        }

        #endregion
    }
}