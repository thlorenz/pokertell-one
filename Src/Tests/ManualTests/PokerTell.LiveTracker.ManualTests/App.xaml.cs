namespace PokerTell.LiveTracker.ManualTests
{
    using System;
    using System.Reflection;
    using System.Windows;

    using Infrastructure;

    using log4net;
    using log4net.Core;

    using NewHandCreator;

    using Tools;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        Bootstrapper _bootStrapper;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            RunInDebugMode();

            ShutdownMode = ShutdownMode.OnMainWindowClose;

            RunNewHandLauncher();
        }

        void RunNewHandLauncher()
        {
            Current.MainWindow.Width = 400;
            Current.MainWindow.Height = 100;
            Current.MainWindow.Left = 0;
            Current.MainWindow.Top = 0;
           _bootStrapper.Container
               .Resolve<NewHandCreatorLauncher>()
               .Launch();
        }

        static void HandleException(Exception excep)
        {
            if (excep == null)
            {
                return;
            }

            Log.Error("An unhandled error occurred in Aplication: ", excep);

            Environment.Exit(1);
        }

        void RunInDebugMode()
        {
            AppDomain.CurrentDomain.UnhandledException +=
                (sender, e) => HandleException(e.ExceptionObject as Exception);

            try
            {
                new Logger(ApplicationProperties.ApplicationName)
                   .InitializeConsoleAppender(Level.Debug)
                   .InitializeRollingFileAppender(Files.LocalUserAppDataPath + @"\" + Files.LogFile, 5, Level.Debug);
                
                _bootStrapper = new Bootstrapper();
                _bootStrapper.Run();

                GlobalCommands.StartServicesCommand.Execute(null);
            }
            catch (Exception excep)
            {
                HandleException(excep);
            }
        }
    }
}