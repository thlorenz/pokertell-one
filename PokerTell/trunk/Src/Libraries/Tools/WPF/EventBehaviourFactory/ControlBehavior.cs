using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tools.WPF
{
	public static class ControlBehavior
	{
	    #region MouseEnterCommand

	    public static readonly DependencyProperty MouseEnterCommand =
	        EventBehaviourFactory.CreateCommandExecutionEventBehaviour(
	            Control.MouseEnterEvent, "MouseEnterCommand", typeof (ControlBehavior));
	    
	    public static void SetMouseEnterCommand(DependencyObject o, ICommand value)
	    {
	        o.SetValue(MouseEnterCommand, value);
	    }
	    
	    public static ICommand GetMouseEnterCommand(DependencyObject o)
	    {
	        return o.GetValue(MouseEnterCommand) as ICommand;
	    }
	    
	    #endregion
	    
	    #region MouseLeaveCommand

	    public static readonly DependencyProperty MouseLeaveCommand =
	        EventBehaviourFactory.CreateCommandExecutionEventBehaviour(
	            Control.MouseLeaveEvent, "MouseLeaveCommand", typeof (ControlBehavior));
	    
	    public static void SetMouseLeaveCommand(DependencyObject o, ICommand value)
	    {
	        o.SetValue(MouseLeaveCommand, value);
	    }
	    
	    public static ICommand GetMouseLeaveCommand(DependencyObject o)
	    {
	        return o.GetValue(MouseLeaveCommand) as ICommand;
	    }
	    
	    #endregion
	    
	    
	    #region MouseDoubleClickCommand
	    
	    public static readonly DependencyProperty MouseDoubleClickCommand = 
	        EventBehaviourFactory.CreateCommandExecutionEventBehaviour(
            Control.MouseDoubleClickEvent, "MouseDoubleClickCommand", typeof (ControlBehavior));

        public static void SetMouseDoubleClickCommand(Control o, ICommand command)
        {
            o.SetValue(MouseDoubleClickCommand, command);
        }

        public static void GetMouseDoubleClickCommand(Control o)
        {
            o.GetValue(MouseDoubleClickCommand);
        }
        
        #endregion
        
        #region MouseLeftButtonDownCommand

        public static readonly DependencyProperty MouseLeftButtonDownCommand =
            EventBehaviourFactory.CreateCommandExecutionEventBehaviour(
                Control.MouseLeftButtonDownEvent, "MouseLeftButtonDownCommand", typeof (ControlBehavior));
        
        public static void SetMouseLeftButtonDownCommand(DependencyObject o, ICommand value)
        {
            o.SetValue(MouseLeftButtonDownCommand, value);
        }
        
        public static ICommand GetMouseLeftButtonDownCommand(DependencyObject o)
        {
            return o.GetValue(MouseLeftButtonDownCommand) as ICommand;
        }
        
        #endregion
       
	}
}
