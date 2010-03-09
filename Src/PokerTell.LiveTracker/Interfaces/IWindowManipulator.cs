namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Windows;

    using Tools.WPF.Interfaces;

    public interface IWindowManipulator
    {
        IntPtr WindowHandle { get; set; }

        bool CheckWindowLocationAndSize(Action<Point, Size> onChangeDetected);

        string GetInfo();

        string GetText();

        /// <summary>
        /// Finds the Window handle right on top of the controlled window in the Z order
        /// </summary>
        /// <returns>The handle to the window or Zero if Pokertable wasn't found</returns>
        IntPtr GetTheHandleOfWindowOnTopOfYours();

       void PlaceThisWindowDirectlyOnTopOfYours(IWindowManager topWindow, Action onBottomWindowWasNotFound);

        bool SetTextTo(string windowText);

        /// <summary>
        /// Tries to find the subText in the Window text.
        /// If it is found and is not equal to the subText, the fullText will be set to the
        /// Window Text and then the Window Text is set to the subText.
        /// In case the subText is not found, the onSubTextNotContainedInWindowText command is invoked.
        /// </summary>
        /// <param name="subText">Text to be found in window text</param>
        /// <param name="onSubTextNotContainedInWindowText">Command invoked e.g. if Table changed</param>
        /// <returns>The current fulltext of the window, regardless if it changed or not.</returns>
        string SetTextTo(string subText, Action<string> onSubTextNotContainedInWindowText);
    }
}