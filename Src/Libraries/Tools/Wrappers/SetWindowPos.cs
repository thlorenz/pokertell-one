namespace Tools.Wrappers
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Wrapper for SetWindowPos.
    /// </summary>
    public partial class User32
    {
        public enum SWP_Flags : uint
        {
            ASYNCWINDOWPOS = 0x4000, 
            DEFERERASE = 0x2000, 
            DRAWFRAME = 0x0020, 
            FRAMECHANGED = 0x0020, 
            HIDEWINDOW = 0x0080, 
            NOACTIVATE = 0x0010, 
            NOCOPYBITS = 0x0100, 
            NOMOVE = 0x0002, 
            NOOWNERZORDER = 0x0200, 
            NOREDRAW = 0x0008, 
            NOREPOSITION = 0x0200, 
            NOSENDCHANGING = 0x0400, 
            NOSIZE = 0x0001, 
            NOZORDER = 0x0004, 
            SHOWWINDOW = 0x0040
        }

        public enum SWP_Hwnd
        {
            TOP = 0, 
            BOTTOM = 1, 
            TOPMOST = -1, 
            NOTOPMOST = -2
        }

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP_Flags uFlags);
    }
}