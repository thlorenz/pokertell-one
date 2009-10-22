namespace PokerTell
{
    using System;
    using System.Reflection;
    using System.Windows;

    using log4net;

    using PokerHand.Views;

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

           // RunTestWindow();

           RunInDebugMode();

            Log.Info("Started PokerTell");

            ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        void RunTestWindow()
        {
            new TestWindow().Show();
        }

        static void RunInDebugMode()
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
            Log4NetAppenders.InitializeDebugAppender();

            try
            {
                new Bootstrapper().Run();
            }
            catch (Exception excep)
            {
                HandleException(excep);
            }  
        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
        }

        private static void HandleException(Exception excep)
        {
            if (excep == null)
            {
                return;
            }

            Log.Error("An unhandled error occurred", excep);
           
            // Environment.Exit(1);
        }
    }
}
