/*
 * User: Thorsten Lorenz
 * Date: 8/14/2009
 * 
 */
using System;

namespace Tools.WPF.ViewModels
{
    /// <summary>
    /// ViewModel representing a command
    /// </summary>
    public class CommandViewModel : ViewModel
    {
        
        public CommandViewModel() { }
        
        public CommandViewModel(string displayName, SimpleCommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            base.Content = displayName;
            this.Command = command;
        }

        public SimpleCommand Command { get;  set; }

        
    }
}