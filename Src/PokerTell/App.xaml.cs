namespace PokerTell
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Windows;

    using DatabaseSetup.Views;

    using Infrastructure;
    using Infrastructure.Events;

    using log4net;
    using log4net.Core;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.PokerHand.Views;

    using Tools;

    using User;

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

//            Current.DispatcherUnhandledException += (sender, args) => {
//                Log.Error(args.Exception);
//                args.Handled = true;
//            };

            ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        static void HandleException(Exception excep)
        {
            if (excep == null)
            {
                return;
            }

            Log.Error("An unhandled error occurred", excep);

            Environment.Exit(1);
        }

        static void RunInDebugMode()
        {
            AppDomain.CurrentDomain.UnhandledException +=
                (sender, e) => HandleException(e.ExceptionObject as Exception);

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);

            try
            {
                new Logger(ApplicationProperties.ApplicationName)
                   .InitializeConsoleAppender(Level.Debug)
                   .InitializeRollingFileAppender(Files.LocalUserAppDataPath + @"\" + Files.LogFile, 5, Level.Debug);

                new Bootstrapper().Run();

                if (!File.Exists(Files.LocalUserAppDataPath + @"\" + Files.UserConfigFile))
                    ConfigureForTheFirstTimeAndStartServices();
                else
                    GlobalCommands.StartServicesCommand.Execute(null);
            }
            catch (Exception excep)
            {
                HandleException(excep);
            }
        }

        static void ConfigureForTheFirstTimeAndStartServices()
        {
            // Instead of raising an event, we talk directly to the UserService to make sure the ShowDialog blocks this thread so we wait for the user to respond
            var msg = PokerTell.Properties.Resources.Info_ConfiguringForFirstTime;
            UserService.HandleUserMessageEvent(new UserMessageEventArgs(UserMessageTypes.Info, msg));

            GlobalCommands.ConfigureServicesForFirstTimeCommand.Execute(null);
            GlobalCommands.StartServicesCommand.Execute(null);

            msg = PokerTell.Properties.Resources.Info_CompletedConfiguration;
            UserService.HandleUserMessageEvent(new UserMessageEventArgs(UserMessageTypes.Info, msg));
        }
    }
}