namespace PokerTell.LiveTracker.DesignWithDatabase
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Windows;

    internal sealed class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US", false);
            Application.Current.Run(
                new TableStatisticsViewWithDatabaseTests().ConnectToDataBase_LoadSomePLayers_ShowTheirStatistics_InTheLiveStats());
        }
    }
}