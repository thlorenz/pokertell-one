namespace Tools.WPF
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Microsoft.Expression.Interactivity;

    /// <summary>
    /// The CommandAction allows the user to route a FrameworkElement's routed event to a Command.
    /// For instance this makes it possible to specify--in Xaml--that right-clicking on a Border
    /// element should execute the Application.Close command (this example may not make much sense,
    /// but it does illustrate what's possible).
    /// 
    /// CommandParameter and CommandTarget properties are provided for consistency with the Wpf
    /// Command pattern.
    /// 
    /// The action's IsEnabled property will be updated according to the Command's CanExecute value.
    /// 
    /// In addition a SyncOwnerIsEnabled property allows the user to specify that the owner element
    /// should be enabled/disabled whenever the action is enabled/disabled.
    ///
    /// Found at http://jacokarsten.wordpress.com/2009/03/27/applying-command-binding-to-any-control-and-any-event/
    /// </summary>
    public class CommandAction : TargetedTriggerAction<FrameworkElement>, ICommandSource
    {
        #region DPs

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandAction), new PropertyMetadata((ICommand)null, OnCommandChanged));
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(CommandAction), new PropertyMetadata());
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(CommandAction), new PropertyMetadata());
        public static readonly DependencyProperty SyncOwnerIsEnabledProperty = DependencyProperty.Register("SyncOwnerIsEnabled", typeof(bool), typeof(CommandAction), new PropertyMetadata());

        #endregion

        #region Properties

        [Category("Command Properties")]
        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }
        [Category("Command Properties")]
        public object CommandParameter
        {
            get
            {
                return (object)GetValue(CommandParameterProperty);
            }
            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }
        [Category("Command Properties")]
        public IInputElement CommandTarget
        {
            get
            {
                return (IInputElement)GetValue(CommandTargetProperty);
            }
            set
            {
                SetValue(CommandTargetProperty, value);
            }
        }
        [Category("Command Properties")]
        public bool SyncOwnerIsEnabled
        {
            get
            {
                return (bool)GetValue(SyncOwnerIsEnabledProperty);
            }
            set
            {
                SetValue(SyncOwnerIsEnabledProperty, value);
            }
        }

        #endregion

        #region Event Declaration

        private EventHandler CanExecuteChanged;

        #endregion

        #region Event Handlers

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            UpdateCanExecute();
        }

        #region DP Event Handlers

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CommandAction)d).OnCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue);
        }

        #endregion

        #endregion

        #region Overrides

        protected override void Invoke(object o)
        {
            if (this.Command != null)
            {
                RoutedCommand comRouted = this.Command as RoutedCommand;
                if (comRouted != null)
                {
                    //Is RoutedCommand
                    comRouted.Execute(this.CommandParameter, this.CommandTarget);
                }
                else
                {
                    //Is NOT RoutedCommand
                    this.Command.Execute(this.CommandParameter);
                }
            }
        }

        #endregion

        #region Helper functions

        private void OnCommandChanged(ICommand comOld, ICommand comNew)
        {
            if (comOld != null)
            {
                UnhookCommandCanExecuteChangedEventHandler(comOld);
            }
            if (comNew != null)
            {
                HookupCommandCanExecuteChangedEventHandler(comNew);
            }
        }
        private void HookupCommandCanExecuteChangedEventHandler(ICommand command)
        {
            this.CanExecuteChanged = new EventHandler(OnCanExecuteChanged);
            command.CanExecuteChanged += CanExecuteChanged;
            UpdateCanExecute();
        }
        private void UnhookCommandCanExecuteChangedEventHandler(ICommand command)
        {
            command.CanExecuteChanged -= CanExecuteChanged;
            UpdateCanExecute();
        }
        private void UpdateCanExecute()
        {
            if (this.Command != null)
            {
                RoutedCommand comRouted = this.Command as RoutedCommand;
                if (comRouted != null)
                {
                    //Is RoutedCommand
                    this.IsEnabled = comRouted.CanExecute(this.CommandParameter, this.CommandTarget);
                }
                else
                {
                    //Is NOT RoutedCommand
                    this.IsEnabled = this.Command.CanExecute(this.CommandParameter);
                }
                if (this.Target != null && this.SyncOwnerIsEnabled)
                {
                    this.Target.IsEnabled = IsEnabled;
                }
            }
        }

        #endregion
    }
}

// Not needed since specifiying interactivity dll explicitly as in :
// xmlns:i="clr-namespace:Microsoft.Expression.Interactivity;assembly=Microsoft.Expression.Interactivity" 
// instead of:
// xmlns:i="http://schemas.microsoft.com/expression/2009/interactivity" 
//namespace Microsoft.Expression.Interactivity.Layout
//{
//    /// <summary>
//    /// Needed to compile without the g.cs build failing b/c it adds Microsoft.Expression.Interactivity.Layout
//    /// in its using section
//    /// </summary>
//    public class Layout
//    {
//    }
//}

