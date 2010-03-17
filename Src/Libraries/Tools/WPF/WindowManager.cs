namespace Tools.WPF
{
    using System;
    using System.Windows;
    using System.Windows.Interop;

    using Interfaces;

    public class WindowManager : IWindowManager
    {
        readonly Func<Window> _createWindow;

        public WindowManager(Func<Window> createWindow)
        {
            _createWindow = createWindow;
        }

        protected Window _window;

        public Window Window
        {
            get
            {
                if (_window == null)
                    _window = _createWindow();
               
                _window.Closed += (s, e) => Dispose();
                return _window;
            }
        }

        public void Dispose()
        {
            if (_window != null)
            {
                _window.Close();

                _window = null;
            }
        }

        public object DataContext
        {
            get { return Window.DataContext; }
            set { Window.DataContext = value; }
        }

        public IWindowManager Show()
        {
            Window.Show();
            return this;
        }

        public IWindowManager ShowDialog()
        {
            Window.ShowDialog();
            return this;
        }

        public IWindowManager BringIntoView()
        {
            Window.BringIntoView();
            return this;
        }

        public IWindowManager Activate()
        {
            Window.Activate();
            return this;
        }

        public IWindowManager Hide()
        {
            Window.Hide();
            return this;
        }

        public IntPtr Handle
        {
            get { return new WindowInteropHelper(Window).Handle; }
        }

        public double Left
        {
            get { return Window.Left; }
            set { Window.Left = value; }
        }

        public double Top
        {
            get { return Window.Top; }
            set { Window.Top = value; }
        }

        public double Width
        {
            get { return Window.Width; }
            set { Window.Width = value; }
        }

        public double Height
        {
            get { return Window.Height; }
            set { Window.Height = value; }
        }
    }
}