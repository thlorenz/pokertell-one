namespace Tools.WPF.Views
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;

    using log4net;

    public class DialogView : Window
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public DialogView()
        {
            // Cannot set owner to itself so catch this exception during testing when there may not be a maain window
            try
            {
                Owner = Application.Current.MainWindow;
            }
            catch (Exception excep)
            {
                Log.Error(excep);
            }

            ResizeMode = ResizeMode.NoResize;
        }

        protected void WindowBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        protected void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}