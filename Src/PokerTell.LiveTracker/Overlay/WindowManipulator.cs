namespace PokerTell.LiveTracker.Overlay
{
    using System;
    using System.Drawing;
    using System.Text;

    using DllWrappers;

    using PokerTell.LiveTracker.Interfaces;

    using Tools.WPF.Interfaces;

    using Point = System.Windows.Point;
    using Size = System.Windows.Size;

    public class WindowManipulator : IWindowManipulator
    {
        IntPtr _windowHandle;

        /// <summary>
        /// Remembers previous condition of Pokertable to determine if it changed and
        /// any adjustments are necessary
        /// </summary>
        Rectangle _windowOnLastCheck;

        public WindowManipulator()
        {
        }

        public WindowManipulator(IntPtr windowHandle)
        {
            WindowHandle = windowHandle;
        }

        public IntPtr WindowHandle
        {
            get { return _windowHandle; }

            set
            {
                _windowHandle = value;
                _windowOnLastCheck = GetCurrentWindowRectangle();
            }
        }

        public bool CheckWindowLocationAndSize(Action<Point, Size> onChangeDetected)
        {
            Rectangle currentWindow = GetCurrentWindowRectangle();

            // sizes have location added to them
            // We simply have to subtract the rect.X value from the rect.Width value to obtain the "real" width
            currentWindow.Width = currentWindow.Width - currentWindow.X;
            currentWindow.Height = currentWindow.Height - currentWindow.Y;

            if (_windowOnLastCheck != currentWindow)
            {
                _windowOnLastCheck = currentWindow;

                onChangeDetected.Invoke(
                    new Point(currentWindow.X, currentWindow.Y), 
                    new Size(currentWindow.Width, currentWindow.Height));
            }

            return true;
        }

        public string GetInfo()
        {
            var sbClass = new StringBuilder(256);
            User32.GetClassName(_windowHandle, sbClass, sbClass.Capacity);

            var sbInfo = new StringBuilder();
            sbInfo.AppendLine("\nHandle: " + _windowHandle);
            sbInfo.AppendLine("Class : " + sbClass);
            sbInfo.AppendLine("Text  : " + GetText());

            return sbInfo.ToString();
        }

        public string GetText()
        {
            var txtLength = (int)User32.SendMessage(_windowHandle, User32.WM_GETTEXTLENGTH, 0, null);

            var sbText = new StringBuilder(txtLength + 1);
            User32.SendMessage(_windowHandle, User32.WM_GETTEXT, sbText.Capacity, sbText);
            return sbText.ToString();
        }

        /// <summary>
        /// Finds the Window handle right on top of the controlled window in the Z order
        /// </summary>
        /// <returns>The handle to the window or Zero if Pokertable wasn't found</returns>
        public IntPtr GetTheHandleOfWindowOnTopOfYours()
        {
            if (_windowHandle != IntPtr.Zero)
            {
                IntPtr topMostHandle = User32.GetTopWindow(0);
                IntPtr nextHandle = topMostHandle;
                do
                {
                    // Remember this Window
                    IntPtr onTopOfTableHandle = nextHandle;

                    // Get Next Window
                    nextHandle = User32.GetWindow(nextHandle, User32.GW_HWNDNEXT);
                    if (nextHandle == _windowHandle)
                    {
                        // Found PokerTable so previous one is on Top of it
                        return onTopOfTableHandle;
                    }
                }
                while ((nextHandle != topMostHandle) && (nextHandle != IntPtr.Zero));
            }

            // Did'nt find Pokertable
            return IntPtr.Zero;
        }

        public void PlaceThisWindowDirectlyOnTopOfYours(IWindowManager topWindow, Action onBottomWindowWasNotFound)
        {
            IntPtr onTopOfThisWindow = GetTheHandleOfWindowOnTopOfYours();

            // Does PokerTable Window still exist??
            if (onTopOfThisWindow == IntPtr.Zero)
            {
                onBottomWindowWasNotFound.Invoke();
            }
            else
            {
                IntPtr topWindowHandle = topWindow.Handle;

                if (topWindowHandle != onTopOfThisWindow)
                {
                    User32.SetWindowPos(
                        topWindowHandle, 
                        onTopOfThisWindow, 
                        (int) topWindow.Left, 
                        (int) topWindow.Top, 
                        (int) topWindow.Width, 
                        (int) topWindow.Height, 
                        User32.SWP_Flags.NOACTIVATE);
                }
            }
        }

        public bool SetTextTo(string windowText)
        {
            return User32.SetWindowText(_windowHandle, windowText);
        }

        /// <summary>
        /// Tries to find the subText in the Window text.
        /// If it is found and is not equal to the subText, the fullText will be set to the
        /// Window Text and then the Window Text is set to the subText.
        /// In case the subText is not found, the onSubTextNotContainedInWindowText command is invoked.
        /// </summary>
        /// <param name="subText">Text to be found in window text</param>
        /// <param name="onSubTextNotContainedInWindowText">Command invoked e.g. if Table changed</param>
        /// <returns>The current fulltext of the window, regardless if it changed or not.</returns>
        public string SetTextTo(string subText, Action onSubTextNotContainedInWindowText)
        {
            string currentWindowText = GetText();

            if (! string.IsNullOrEmpty(subText) && !currentWindowText.Equals(subText))
            {
                if (currentWindowText.Contains(subText))
                {
                    SetTextTo(subText);
                }
                else if (onSubTextNotContainedInWindowText != null)
                {
                    onSubTextNotContainedInWindowText.Invoke();
                }
            }

            return currentWindowText;
        }

        #region Methods

        Rectangle GetCurrentWindowRectangle()
        {
            var lpRect = new Rectangle();
            User32.GetWindowRect(_windowHandle, ref lpRect);

            return lpRect;
        }

        #endregion
    }
}