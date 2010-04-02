namespace PokerTell
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Windows;
    using System.Windows.Threading;

    using log4net;
    using log4net.Core;

    using Microsoft.Practices.Unity;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Events;
    using PokerTell.User;

    using Tools;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            RunInDebugMode();
           // RunInReleaseMode();

            ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        static void BootApplication()
        {
            new Bootstrapper().Run();

            if (!File.Exists(Files.LocalUserAppDataPath + @"\" + Files.UserConfigFile))
                ConfigureForTheFirstTimeAndStartServices();
            else
                StartServices();
        }

        static void ConfigureForTheFirstTimeAndStartServices()
        {
            InformUserThatConfigurationWillTakePlace();

            ConfigureServicesForTheFirstTime();
            StartServices();

            InformUserThatConfigurationWasCompleted();
        }

        static void ConfigureServicesForTheFirstTime()
        {
            GlobalCommands.ConfigureServicesForFirstTimeCommand.Execute(null);
        }

        static void HandleDevelopmentException(Exception excep)
        {
            if (excep == null) return;

            LogExceptionAndFullStackTrace(excep);

            Environment.Exit(1);
        }

        static void HandleAppDomainException(UnhandledExceptionEventArgs e)
        {
            var excep = e.ExceptionObject as Exception;
            if (excep == null) return;

            LogExceptionAndFullStackTrace(excep);

            UserService.PublishUnhandledException(excep, e.IsTerminating);
        }

        static void HandleDispatcherException(DispatcherUnhandledExceptionEventArgs e)
        {
            var excep = e.Exception;
            if (excep == null) return;

            LogExceptionAndFullStackTrace(excep);

            UserService.PublishUnhandledException(excep, false);

            e.Handled = true;
        }

        static void InformUserThatConfigurationWasCompleted()
        {
            string msg = PokerTell.Properties.Resources.Info_CompletedConfiguration;
            UserService.HandleUserMessageEvent(new UserMessageEventArgs(UserMessageTypes.Info, msg));
        }

        static void InformUserThatConfigurationWillTakePlace()
        {
            var msg = PokerTell.Properties.Resources.Info_ConfiguringForFirstTime;
            UserService.HandleUserMessageEvent(new UserMessageEventArgs(UserMessageTypes.Info, msg));
        }

        static void InitializeEnvironment()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);

            if (!Directory.Exists(Files.LocalUserAppDataPath))
                Directory.CreateDirectory(Files.LocalUserAppDataPath);
        }

        static void InitializeLogger()
        {
            new Logger(ApplicationProperties.ApplicationName)
                .InitializeConsoleAppender(Level.Debug)
                .InitializeRollingFileAppender(Files.LocalUserAppDataPath + @"\" + Files.LogFile, 5, Level.Debug);

            Log.InfoFormat("\n\n{0}", Utils.EnvironmentInformation);
        }

        static void LogExceptionAndFullStackTrace(Exception excep)
        {
            Log.Error(excep);
            Log.InfoFormat("Full StackTrace: \n{0}", Environment.StackTrace);
        }

        static void RunInDebugMode()
        {
            AppDomain.CurrentDomain.UnhandledException += (_, e) => HandleDevelopmentException(e.ExceptionObject as Exception);

            try
            {
                InitializeEnvironment();

                InitializeLogger();

                BootApplication();
            }
            catch (Exception excep)
            {
                HandleDevelopmentException(excep);
            }
        }

        static void RunInReleaseMode()
        {
            Current.DispatcherUnhandledException += (_, e) => HandleDispatcherException(e);
            AppDomain.CurrentDomain.UnhandledException += (_, e) => HandleAppDomainException(e);

            try
            {
                InitializeEnvironment();

                InitializeLogger();

                BootApplication();
            }
            catch (Exception excep)
            {
                HandleAppDomainException(new UnhandledExceptionEventArgs(excep, false));
            }
        }

        static void StartServices()
        {
            GlobalCommands.StartServicesCommand.Execute(null);
        }
    }
}