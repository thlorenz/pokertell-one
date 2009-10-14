/*
 * User: Thorsten Lorenz
 * Date: 8/15/2009
 * 
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Tools.WPF.Controls
{
    /// <summary>
    /// Interaction logic for CommandButton.xaml
    /// </summary>
    public partial class CommandButton : UserControl
    {
        public CommandButton()
        {
            InitializeComponent();
           
            
            this.butCommand.MouseDown += delegate { Debug.WriteLine("Clicked"); };
        }
    }
}