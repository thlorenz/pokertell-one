namespace PokerTell.User
{
    using System;
    using System.Windows;

    using Interfaces;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Events;
    using Microsoft.Practices.Unity;

    using PokerTell.Infrastructure.Events;
    using PokerTell.User.ViewModels;
    using PokerTell.User.Views;

    using Properties;

    public class UserService
    {
        static IUnityContainer _container;

        public UserService(IUnityContainer container, IEventAggregator eventAggregator)
        {
            _container = container;

            const bool keepMeAlive = true;
            eventAggregator
                .GetEvent<UserMessageEvent>()
                .Subscribe(HandleUserMessageEvent, ThreadOption.UIThread, keepMeAlive);
           
            eventAggregator
                .GetEvent<UserConfirmActionEvent>()
                .Subscribe(HandleUserConfirmActionEvent, keepMeAlive);
        }

        static void HandleUserConfirmActionEvent(UserConfirmActionEventArgs userConfirmActionEventArgs)
        {
            var viewModel = new ConfirmActionViewModel(
                userConfirmActionEventArgs.ActionToConfirm, userConfirmActionEventArgs.Message);
            var userConfirmActionView = new ConfirmActionView(viewModel) { Owner = Application.Current.MainWindow };
            userConfirmActionView.ShowDialog();
        }

        public static void HandleUserMessageEvent(UserMessageEventArgs userMessageEventArgs)
        {
            var viewModel = new UserMessageViewModel(userMessageEventArgs);
            var userMessageView = new UserMessageView(viewModel) { Owner = Application.Current.MainWindow };
            userMessageView.ShowDialog();
        }

        public static void PublishUnhandledException(Exception excep, bool willTerminate)
        {
            var msg = willTerminate ? Resources.TerminatingUnhandledException_Message : Resources.NonTerminatingUnhandledException_Message;

            HandleUserMessageEvent(new UserMessageEventArgs(UserMessageTypes.Error, msg, excep));
            
            GiveUserChanceToSendProblemReport(excep);
        }

        static void GiveUserChanceToSendProblemReport(Exception excep)
        {
            var reportWindow = _container.Resolve<IReportWindowManager>();
            reportWindow.Subject = string.Format("{0} [{1}] ", excep.Message, Environment.OSVersion);
            reportWindow.ShowDialog();
        }
    }
}