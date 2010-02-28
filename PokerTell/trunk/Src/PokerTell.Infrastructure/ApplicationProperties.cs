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
        public const string ApplicationName = "PokerTellWPF";

        /// <summary>
        /// Number of tables to be displayed in Tables Form
        /// </summary>
        public const int TablesPerPage = 5;

        /// <summary>
        /// Keys used to normalize the true bet sizes
        /// in order to make a statistical analysis
        /// </summary>
        public static double[] BetSizeKeys
        {
            get { return new[] { 0.1, 0.3, 0.5, 0.7, 0.9, 1.5 }; }
        }

        public static double[] RaiseSizeKeys
        {
            get { return new[] { 2.0, 3.0, 5.0, 9.0 }; }
        }

        /// <summary>
        /// Calling ratios in raised Pot: 0.5, 0.7, 0.8 
        /// </summary>
        public static double[] RaisedPotCallingRatios
        {
            get { return new[] { 0.5, 0.7, 0.8 }; }
        }

        /// <summary>
        /// Calling ratios in unraised Pot: 0.2, 0.3, 0.4, 0.7
        /// </summary>
        public static double[] UnraisedPotCallingRatios
        {
            get { return new[] { 0.2, 0.3, 0.4, 0.7 }; }
        }

        public static Brush BorderedWindowBackgoundBrush
        {
            // #FF 3C 3C 3C => Color.FromScRgb(255, 60, 60, 60)
            get { return new SolidColorBrush(Colors.DimGray); }
        }

        public const string MappingAssemblyName = "PokerTell.PokerHand";

        /// <summary>
        /// Sets the culture to "en-US"
        /// This needs to be called for all threads
        /// </summary>
        public static void SetCulture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US", false);
        }

        public const string ShellMainRegion = "Shell.MainRegion";
        public const string ShellMainMenuRegion = "Shell.MainMenuRegion";
        public const string ShellDatabaseMenuRegion = "Shell.ShellDatabaseMenuRegion";
        public const string ShellStatusRegion = "Shell.StatusRegion";

        public const string HandHistoriesRegion = "HandHistoriesRegion";
    }
}