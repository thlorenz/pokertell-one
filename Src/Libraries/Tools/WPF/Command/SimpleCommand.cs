/*
 * User: Thorsten Lorenz
 * Date: 7/25/2009
 * 
*/
namespace Tools.WPF
{
    using System;

    /// <summary>
    /// Implements the ICommand and wraps up all the verbose stuff so that you can just pass 2 delegates 1 for the CanExecute and one for the Execute
    /// </summary>
    public class SimpleCommand : IWPFCommand
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Predicate to execute when the CanExecute of the command gets called
        /// </summary>
        public Predicate<object> CanExecuteDelegate { get; set; }

        /// <summary>
        /// Gets or sets the action to be called when the Execute method of the command gets called
        /// </summary>
        public Action<object> ExecuteDelegate { get; set; }

        public Boolean ExecuteSucceeded { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if the command Execute method can run
        /// </summary>
        /// <param name="parameter">The command parameter to be passed</param>
        /// <returns>Returns true if the command can execute. By default true is returned so that if the user of SimpleCommand does not specify a CanExecuteCommand delegate the command still executes.</returns>
        public override bool CanExecute(object parameter)
        {
            if (CanExecuteDelegate != null)
            {
                return CanExecuteDelegate(parameter);
            }
            return true; // if there is no can execute default to true
        }

        /// <summary>
        /// Executes the actual command
        /// </summary>
        /// <param name="parameter">THe command parameter to be passed</param>
        public override void Execute(object parameter)
        {
            if (ExecuteDelegate != null)
            {
                ExecuteDelegate(parameter);
                ExecuteSucceeded = true;
            }
        }

        #endregion
    }
}