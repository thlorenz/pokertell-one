namespace Tools.GenericUtilities
{
    using System;
    using System.Collections.Generic;

    public class CompositeAction<T>
    {
        #region Constants and Fields

        readonly IList<Action<T>> _registeredActions;

        #endregion

        #region Constructors and Destructors

        public CompositeAction()
        {
            _registeredActions = new List<Action<T>>();
        }

        #endregion

        #region Properties

        public IEnumerable<Action<T>> RegisteredActions
        {
            get { return _registeredActions; }
        }

        #endregion

        #region Public Methods

        public void Execute(T parameter)
        {
            foreach (Action<T> action in RegisteredActions)
            {
                action.Invoke(parameter);
            }
        }

        public CompositeAction<T> RegisterAction(Action<T> action)
        {
            return RegisterAction(action, false);
        }

        public CompositeAction<T> RegisterAction(Action<T> action, bool asFirst)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (_registeredActions.Contains(action))
            {
                _registeredActions.Remove(action);
            }

            if (asFirst)
            {
                _registeredActions.Insert(0, action);
            }
            else
            {
                _registeredActions.Add(action);
            }

            return this;
        }

        public CompositeAction<T> RegisterActions(IList<Action<T>> actionCollection)
        {
            foreach (Action<T> action in actionCollection)
            {
                RegisterAction(action);
            }

            return this;
        }

        public CompositeAction<T> UnregisterAction(Action<T> action)
        {
            _registeredActions.Remove(action);
            return this;
        }

        #endregion

        // ICommand implementation

        // /// <summary>
        // /// Always returns true.
        // /// Only added to conform to ICommand -> don't use.
        // /// </summary>
        // /// <param name="parameter">Irrelevant parameter</param>
        // /// <returns>True</returns>
        // public bool CanExecute(object parameter)
        // {
        // return true;
        // }
        // /// <summary>
        // /// Only added to conform to ICommand, use strongly typed
        // /// <code> <![CDATA[Execute(T parameter)]]></code> instead
        // /// </summary>
        // /// <param name="parameter">Parameter to pass, must be of type T</param>
        // public void Execute(object parameter)
        // {
        // if (parameter.GetType() != typeof(T))
        // {
        // throw new ArgumentException("parameter must be of type " + typeof(T));
        // }
        // Execute((T)parameter);
        // }

        // /// <summary>
        // /// Only added to conform to ICommand -> don't use
        // /// </summary>
        // public event EventHandler CanExecuteChanged;
    }
}