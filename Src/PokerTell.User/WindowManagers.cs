namespace PokerTell.User
{
    using System;
    using System.Windows;

    using Interfaces;

    using Tools.WPF;

    using ViewModels;

    using Views;

    public class ReportWindowManager : WindowManager, IReportWindowManager
    {
        public ReportWindowManager(IReportViewModel reportViewModel)
            : base(() => new ReportView())
        {
            DataContext = reportViewModel;
        }

        public string Subject
        {
            get { return ViewModel.Subject; }
            set { ViewModel.Subject = value; }
        }

        protected IReportViewModel ViewModel
        {
             get { return (IReportViewModel)DataContext; }
        }

    }
}