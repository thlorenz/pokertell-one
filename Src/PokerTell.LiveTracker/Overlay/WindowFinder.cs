namespace PokerTell.LiveTracker.Overlay
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Text.RegularExpressions;

    using DllWrappers;

    using PokerTell.LiveTracker.Interfaces;

    /// <summary>
    /// Description of WindowHandler.
    /// </summary>
    public class WindowFinder : IWindowFinder
    {
        /// <summary>
        /// This is an event that is run each time a window was found 
        /// that matches the search criterias. 
        /// The boolean return value of the delegate matches the functionality of the 
        /// PChildCallBack delegate function.
        /// </summary>
        event Func<IntPtr, bool> FoundWindow;

        /// <summary>
        /// Name of the class of the window, Can be null.
        /// </summary>
        Regex _className;

        /// <summary>
        /// Name of the process, optional
        /// </summary>
        Regex _process;

        /// <summary>
        /// Regex object that will be matched to the window text
        /// </summary>
        Regex _windowText;

        /// <summary>
        /// Tries to find a window for the given criteria
        /// </summary>
        /// <param name="parentHandle">Handle of the parent that owns the window</param>
        /// <param name="className"><see cref="_className"></see></param>
        /// <param name="windowText"><see cref="_windowText"></see></param>
        /// <param name="process"><see cref="_process"></see></param>
        /// <param name="foundWindowCallback"><see cref="FoundWindow"></see></param>
        public void FindWindows(IntPtr parentHandle, Regex className, Regex windowText, Regex process, Func<IntPtr, bool> foundWindowCallback)
        {
            _className = className;
            _windowText = windowText;
            _process = process;

            // Add the FoundWindowCallback to the FoundWindow event.
            FoundWindow = foundWindowCallback;

            // Invoke the EnumChildWindows function.
            User32.EnumChildWindows(parentHandle, EnumChildWindowsCallback, 0);
        }

        // This function gets called each time a window is found by the EnumChildWindows function. The foun windows here
        // are NOT the final found windows as the only filtering done by EnumChildWindows is on the parent window handle.
        bool EnumChildWindowsCallback(IntPtr handle, int lParam)
        {
            // If a class name was provided, check to see if it matches the window.
            if (_className != null)
            {
                var sbClass = new StringBuilder(256);
                User32.GetClassName(handle, sbClass, sbClass.Capacity);

                // If it does not match, return true so we can continue on with the next window.
                if (!_className.IsMatch(sbClass.ToString()))
                {
                    return true;
                }
            }

            // If a window text was provided, check to see if it matches the window.
            if (_windowText != null)
            {
                var txtLength = (int)User32.SendMessage(handle, User32.WM_GETTEXTLENGTH, 0, null);
                var sbText = new StringBuilder(txtLength + 1);
                User32.SendMessage(handle, User32.WM_GETTEXT, sbText.Capacity, sbText);

                // If it does not match, return true so we can continue on with the next window.
                if (!_windowText.IsMatch(sbText.ToString()))
                {
                    return true;
                }
            }

            // If a process name was provided, check to see if it matches the window.
            if (_process != null)
            {
                int processId;
                User32.GetWindowThreadProcessId(handle, out processId);

                // Now that we have the process ID, we can use the built in .NET function to obtain a process object.
                Process p = Process.GetProcessById(processId);

                // If it does not match, return true so we can continue on with the next window.
                if (!_process.IsMatch(p.ProcessName))
                {
                    return true;
                }
            }

            // If we get to this point, the window is a match. Now invoke the FoundWindow event and based upon
            // the return value, whether we should continue to search for windows.
            return FoundWindow(handle);
        }

        public void FindWindowMatching(Regex windowText, Regex process, Func<IntPtr, bool> foundWindowCallback)
        {
            FindWindows((IntPtr)0, null, windowText, process, foundWindowCallback);
        }
    }
}