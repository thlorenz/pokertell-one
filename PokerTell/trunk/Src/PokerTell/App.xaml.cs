namespace PokerTell
{
    using System;
    using System.Reflection;
    using System.Windows;

    using log4net;

    using PokerTell.PokerHand.Views;

    using Tools;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Methods

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // RunTestWindow();
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

        void RunTestWindow()
        {
            new TestWindow().Show();
        }

        #endregion
    }
}