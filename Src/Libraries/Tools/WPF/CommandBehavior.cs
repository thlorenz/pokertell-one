namespace Tools.WPF
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;

    public static class CommandBehavior
    {
        #region Constants and Fields

        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.RegisterAttached("Parameter", 
                                                typeof(object), 
                                                typeof(CommandBehavior), 
                                                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Event : The event that should actually execute the
        /// ICommand
        /// </summary>
        public static readonly DependencyProperty EventProperty =
            DependencyProperty.RegisterAttached("Event", 
                                                typeof(String), 
                                                typeof(CommandBehavior), 
                                                new FrameworkPropertyMetadata(String.Empty, 
                                                                              new PropertyChangedCallback(
                                                                                  OnEventChanged)));

        /// <summary>
        /// Command : The actual ICommand to run
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", 
                                                typeof(ICommand), 
                                                typeof(CommandBehavior), 
                                                new FrameworkPropertyMetadata((ICommand)null));

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the Command property.  
        /// </summary>
        public static object GetParameter(DependencyObject d)
        {
            return d.GetValue(ParameterProperty);
        }

        /// <summary>
        /// Gets the Event property.  
        /// </summary>
        public static string GetEvent(DependencyObject d)
        {
            return (String)d.GetValue(EventProperty);
        }

        /// <summary>
        /// Gets the Command property.  
        /// </summary>
        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        /// <summary>
        /// Sets the Command property.  
        /// </summary>
        public static void SetParameter(DependencyObject d, object value)
        {
            d.SetValue(ParameterProperty, value);
        }

        /// <summary>
        /// Sets the Event property.  
        /// </summary>
        public static void SetEvent(DependencyObject d, string value)
        {
            d.SetValue(EventProperty, value);
        }

        /// <summary>
        /// Sets the Command property.  
        /// </summary>
        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Hooks up a Dynamically created EventHandler (by using the 
        /// <see cref="EventHooker">EventHooker</see> class) that when
        /// run will run the associated ICommand
        /// </summary>
        static void OnEventChanged(
            DependencyObject d, 
            DependencyPropertyChangedEventArgs e)
        {
            string routedEvent = (String)e.NewValue;

            // If the RoutedEvent string is not null, create a new
            // dynamically created EventHandler that when run will execute
            // the actual bound ICommand instance (usually in the ViewModel)
            if (!String.IsNullOrEmpty(routedEvent))
            {
                EventHooker eventHooker = new EventHooker();
                eventHooker.ObjectWithAttachedCommand = d;

                EventInfo eventInfo = d.GetType().GetEvent(routedEvent, 
                                                           BindingFlags.Public | BindingFlags.Instance);

                // Hook up Dynamically created event handler
                if (eventInfo != null)
                {
                    eventInfo.AddEventHandler(d, 
                                              eventHooker.GetNewEventHandlerToRunCommand(eventInfo));
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Contains the event that is hooked into the source RoutedEvent
    /// that was specified to run the ICommand
    /// </summary>
    internal sealed class EventHooker
    {
        #region Properties

        /// <summary>
        /// The DependencyObject, that holds a binding to the actual
        /// ICommand to execute
        /// </summary>
        public DependencyObject ObjectWithAttachedCommand { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a Dynamic EventHandler that will be run the ICommand
        /// when the user specified RoutedEvent fires
        /// </summary>
        /// <param name="eventInfo">The specified RoutedEvent EventInfo</param>
        /// <returns>An Delegate that points to a new EventHandler
        /// that will be run the ICommand</returns>
        public Delegate GetNewEventHandlerToRunCommand(EventInfo eventInfo)
        {
            Delegate del = null;

            if (eventInfo == null)
            {
                throw new ArgumentNullException("eventInfo");
            }

            if (eventInfo.EventHandlerType == null)
            {
                throw new ArgumentException("EventHandlerType is null");
            }

            if (del == null)
            {
                del = Delegate.CreateDelegate(eventInfo.EventHandlerType, 
                                              this, 
                                              GetType().GetMethod("OnEventRaised", 
                                                                  BindingFlags.NonPublic |
                                                                  BindingFlags.Instance));
            }

            return del;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Runs the ICommand when the requested RoutedEvent fires
        /// </summary>
        void OnEventRaised(object sender, EventArgs e)
        {
            ICommand command = (ICommand)(sender as DependencyObject).
                                             GetValue(CommandBehavior.CommandProperty);

            object parameter = (sender as DependencyObject).GetValue(CommandBehavior.ParameterProperty);

            if (command != null)
            {
                command.Execute(parameter);
            }
        }

        #endregion
    }
}