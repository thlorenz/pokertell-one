namespace Tools.WPF
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Markup;

    /// <summary>
    /// This extension is used like a regular binding :
    /// More information: http://tomlev2.wordpress.com/2009/03/17/wpf-using-inputbindings-with-the-mvvm-pattern/ 
    /// </summary>
    /// <example>
    /// <UserControl.InputBindings>
    ///     <KeyBinding Modifiers="Control" Key="E" Command="{input:CommandBinding EditCommand}"/>
    /// </UserControl.InputBindings>
    /// </example>
    /// <remarks>
    /// Limitations: 
    ///     it works only for the DataContext of the XAML root. 
    ///     So you can’t use it, for instance, to define an InputBinding on a control whose DataContext is also redefined,
    ///     because the markup extension will access the root DataContext.
    /// </remarks>
    [MarkupExtensionReturnType(typeof(ICommand))]
    public class CommandBindingExtension : MarkupExtension
    {
        bool _dataContextChangeHandlerSet;

        object _targetObject;

        object _targetProperty;

        public CommandBindingExtension()
        {
        }

        public CommandBindingExtension(string commandName)
        {
            CommandName = commandName;
        }

        [ConstructorArgument("commandName")]
        public string CommandName { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (provideValueTarget != null)
            {
                _targetObject = provideValueTarget.TargetObject;
                _targetProperty = provideValueTarget.TargetProperty;
            }

            if (!string.IsNullOrEmpty(CommandName))
            {
                // The serviceProvider is actually a ProvideValueServiceProvider, which has a private field "_context" of type ParserContext
                var parserContext = GetPrivateFieldValue<ParserContext>(serviceProvider, "_context");
                if (parserContext != null)
                {
                    // A ParserContext has a private field "_rootElement", which returns the root element of the XAML file
                    var rootElement = GetPrivateFieldValue<FrameworkElement>(parserContext, "_rootElement");
                    if (rootElement != null)
                    {
                        // Now we can retrieve the DataContext
                        object dataContext = rootElement.DataContext;

                        // The DataContext may not be set yet when the FrameworkElement is first created, and it may change afterwards,
                        // so we handle the DataContextChanged event to update the Command when needed
                        if (!_dataContextChangeHandlerSet)
                        {
                            rootElement.DataContextChanged += RootElementDataContextChanged;
                            _dataContextChangeHandlerSet = true;
                        }

                        if (dataContext != null)
                        {
                            ICommand command = GetCommand(dataContext, CommandName);
                            if (command != null)
                                return command;
                        }
                    }
                }
            }

            // The Command property of an InputBinding cannot be null, so we return a dummy extension instead
            return DummyCommand.Instance;
        }

        void AssignCommand(ICommand command)
        {
            if (_targetObject != null && _targetProperty != null)
            {
                if (_targetProperty is DependencyProperty)
                {
                    var depObj = _targetObject as DependencyObject;
                    var depProp = _targetProperty as DependencyProperty;
                    depObj.SetValue(depProp, command);
                }
                else
                {
                    var prop = _targetProperty as PropertyInfo;
                    prop.SetValue(_targetObject, command, null);
                }
            }
        }

        ICommand GetCommand(object dataContext, string commandName)
        {
            PropertyInfo prop = dataContext.GetType().GetProperty(commandName);
            if (prop != null)
            {
                var command = prop.GetValue(dataContext, null) as ICommand;
                if (command != null)
                    return command;
            }

            return null;
        }

        T GetPrivateFieldValue<T>(object target, string fieldName)
        {
            FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null)
            {
                return (T)field.GetValue(target);
            }

            return default(T);
        }

        void RootElementDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var rootElement = sender as FrameworkElement;
            if (rootElement != null)
            {
                object dataContext = rootElement.DataContext;
                if (dataContext != null)
                {
                    ICommand command = GetCommand(dataContext, CommandName);
                    if (command != null)
                    {
                        AssignCommand(command);
                    }
                }
            }
        }

        // A dummy command that does nothing...
        class DummyCommand : ICommand
        {
            static DummyCommand _instance;

            DummyCommand()
            {
            }

            public event EventHandler CanExecuteChanged;

            public static DummyCommand Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = new DummyCommand();
                    }

                    return _instance;
                }
            }

            public bool CanExecute(object parameter)
            {
                return false;
            }

            public void Execute(object parameter)
            {
            }
        }
    }
}