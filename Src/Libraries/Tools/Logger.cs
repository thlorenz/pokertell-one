namespace Tools
{
    using System;

    using Interfaces;

    using log4net.Appender;
    using log4net.Config;
    using log4net.Core;
    using log4net.Layout;

    public class Logger : IApplicationLogger
    {
        readonly string _applicationName;

        public Logger(string applicationName)
        {
            _applicationName = applicationName;
        }

        public IApplicationLogger InitializeRollingFileAppender(string fullPath, int maxBackups, Level level)
        {
            var appender = new RollingFileAppender()
                {
                    File = fullPath,
                    AppendToFile = false,
                    
                    RollingStyle = RollingFileAppender.RollingMode.Once,
                    MaxSizeRollBackups = maxBackups,
                    Layout = new PatternLayout(@"%newline%date [%thread] %-5level %logger '%message'%newline")
                        {
                            Header = string.Format("[{0}.Log.Begin] {1}", _applicationName, DateTime.Now),
                            Footer = string.Format("[{0}.Log.End]", _applicationName)
                        },
                    Threshold = level
                };

            appender.ActivateOptions();

            BasicConfigurator.Configure(appender);

            return this;
        }

        public IApplicationLogger InitializeConsoleAppender(Level level)
        {
            var appender = new ConsoleAppender
                {
                    Layout = new PatternLayout(@"%newline%-5level[%thread] %logger '%message' ***%newline"), 
                    Threshold = level
                };

            BasicConfigurator.Configure(appender);

            return this;
        }
    }
}