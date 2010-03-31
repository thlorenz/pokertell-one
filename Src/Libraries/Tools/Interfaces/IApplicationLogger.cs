namespace Tools.Interfaces
{
    using log4net.Core;

    public interface IApplicationLogger
    {
        IApplicationLogger InitializeRollingFileAppender(string fullPath, int maxBackups, Level level);

        IApplicationLogger InitializeConsoleAppender(Level level);
    }
}