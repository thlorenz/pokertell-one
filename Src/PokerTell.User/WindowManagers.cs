namespace PokerTell.User
{
    using System;
    using System.Windows;

    using Interfaces;

    using Tools.WPF;

    using Views;

    public class ReportWindowManager : WindowManager, IReportWindowManager
    {
        public ReportWindowManager(IReportViewModel reportViewModel)
            : base(() => new ReportView())
        {
            DataContext = reportViewModel;
        }
    }
}