/*
 * User: Thorsten Lorenz
 * Date: 7/6/2009
 * 
*/
namespace Tools
{
    using log4net.Appender;
    using log4net.Config;
    using log4net.Core;
    using log4net.Layout;

    public static class Log4NetAppenders
    {
        #region Public Methods

        public static IAppender InitializeConsoleAppender()
        {
            var appender = new ConsoleAppender
                {
                    Layout = new PatternLayout(
                        "%newline%date [%thread] %level %logger - %message%newline"), 
                    Threshold = Level.All
                };

            BasicConfigurator.Configure(appender);

            return appender;
        }

        public static IAppender InitializeDebugAppender()
        {
            var appender = new DebugAppender
                {
                    Layout = new PatternLayout(
                        "%newline%date [%thread] %level %logger - %message%newline"), 
                    Threshold = Level.All
                };

            BasicConfigurator.Configure(appender);

            return appender;
        }

        #endregion
    }
}