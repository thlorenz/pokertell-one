namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Text.RegularExpressions;

    using Overlay;

    public interface IWindowFinder
    {
        /// <summary>
        /// Tries to find a window for the given criteria
        /// </summary>
        /// <param name="parentHandle">Handle of the parent that owns the window</param>
        /// <param name="className"><see cref="WindowFinder._className"></see></param>
        /// <param name="windowText"><see cref="WindowFinder._windowText"></see></param>
        /// <param name="process"><see cref="WindowFinder._process"></see></param>
        /// <param name="foundWindowCallback"><see cref="WindowFinder.FoundWindow"></see></param>
        void FindWindows(IntPtr parentHandle, Regex className, Regex windowText, Regex process, Func<IntPtr, bool> foundWindowCallback);

        void FindWindowMatching(Regex windowText, Regex process, Func<IntPtr, bool> foundWindowCallback);
    }
}