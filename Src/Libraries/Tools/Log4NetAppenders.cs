namespace Tools
{
    using log4net.Appender;
    using log4net.Config;
    using log4net.Core;
    using log4net.Layout;

    public static class Log4NetAppenders
    {

        public static IAppender InitializeConsoleAppender()
        {
            return InitializeConsoleAppender(Level.All);
        }

        public static IAppender InitializeConsoleAppender(Level level)
        {
            var appender = new ConsoleAppender
                {
                    Layout = new PatternLayout(
                        "%newline%date [%thread] %level %logger - %message%newline"), 
                    Threshold = level
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

    }
}