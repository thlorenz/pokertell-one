/*
 * User: Thorsten Lorenz
 * Date: 7/25/2009
 * 
*/
using System;
using System.Windows.Input;

namespace Tools.WPF
{
    /// <summary>
    /// Description of WPFCommand.
    /// </summary>
    public abstract class IWPFCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
	        remove { CommandManager.RequerySuggested -= value; }
        }
        
        public abstract void Execute(object parameter);
        
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }
    }
    

}
