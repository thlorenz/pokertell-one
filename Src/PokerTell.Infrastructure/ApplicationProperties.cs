namespace PokerTell.Infrastructure
{
    using System.Globalization;
    using System.Threading;
    using System.Windows.Media;

    /// <summary>
    /// Description of GlobalValues.
    /// Contains all global variables
    /// </summary>
    public static class ApplicationProperties
    {
        public const string ApplicationName = "PokerTell";

        public const string Author = "Thorsten Lorenz";

        public const string DatabaseSetupMappingAssemblyName = "PokerTell.DatabaseSetup";

        public const string HandHistoriesRegion = "HandHistoriesRegion";

        public const string PokerHandMappingAssemblyName = "PokerTell.PokerHand";

        public const string ShellDatabaseMenuRegion = "Shell.ShellDatabaseMenuRegion";

        public const string ShellMainMenuRegion = "Shell.MainMenuRegion";

        public const string ShellMainRegion = "Shell.MainRegion";

        public const string ShellStatusRegion = "Shell.StatusRegion";

        public const string TableOverlayToolBarRegion = "TableOverlay.ToolBarRegion";

        public const string Version = "0.7.1.0";

        public const double VersionNumber = 0.71;

        /// <summary>
        /// Keys used to normalize the true bet sizes
        /// in order to make a statistical analysis
        /// </summary>
        public static readonly double[] BetSizeKeys = new[] { 0.1, 0.3, 0.5, 0.7, 0.9, 1.5 };

        public static readonly Brush BorderedWindowBackgoundBrush = new SolidColorBrush(Colors.Black);

        /// <summary>
        /// Calling ratios in raised Pot: 0.5, 0.7, 0.8 
        /// </summary>
        public static readonly double[] RaisedPotCallingRatios = new[] { 0.5, 0.7, 0.8 };

        /// <summary>
        /// Raise Size Keys 2.0, 3.0, 5.0, 9.0 
        /// </summary>
        public static readonly double[] RaiseSizeKeys = new[] { 2.0, 3.0, 5.0, 9.0 };

        /// <summary>
        /// Calling ratios in unraised Pot: 0.2, 0.3, 0.4, 0.7
        /// </summary>
        public static readonly double[] UnraisedPotCallingRatios = new[] { 0.2, 0.3, 0.4, 0.7 };

        /// <summary>
        /// Sets the culture to "en-US"
        /// This needs to be called for all threads
        /// </summary>
        public static void SetCulture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US", false);
        }
    }
}