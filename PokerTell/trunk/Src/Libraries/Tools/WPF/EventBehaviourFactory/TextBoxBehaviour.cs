using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tools.WPF
{
	public static class TextBoxBehavior
	{
	    public static readonly DependencyProperty TextChangedCommand =
	        EventBehaviourFactory.CreateCommandExecutionEventBehaviour(
	            TextBox.TextChangedEvent, "TextChangedCommand", typeof (TextBoxBehavior));
	    
	    public static void SetTextChangedCommand(DependencyObject o, ICommand value)
	    {
	        o.SetValue(TextChangedCommand, value);
	    }
	    
	    public static ICommand GetTextChangedCommand(DependencyObject o)
	    {
	        return o.GetValue(TextChangedCommand) as ICommand;
	    }
	   
	}
	

}
