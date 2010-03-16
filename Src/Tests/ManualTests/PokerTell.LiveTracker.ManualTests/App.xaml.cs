namespace PokerTell.LiveTracker.ManualTests
{
    using System;
    using System.Reflection;
    using System.Windows;

    using log4net;

    using NewHandCreator;

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
           _bootStrapper.Container
               .Resolve<NewHandCreatorLauncher>()
               .Launch()
               .StartTracking();
            Current.MainWindow.Hide();
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
                _bootStrapper = new Bootstrapper();
                _bootStrapper.Run();
            }
            catch (Exception excep)
            {
                HandleException(excep);
            }
        }
    }
}