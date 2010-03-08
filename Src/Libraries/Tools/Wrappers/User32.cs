using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DllWrappers
{

	public partial class User32
	{

		public enum WS_EX
		{
			Transparent = 0x20,
			Layered = 0x80000
		}

		public enum LWA
		{
			ColorKey = 0x1,
			Alpha = 0x2
		}
		
		public enum GWL
		{
			ExStyle = -20,
			WndProc = (-4)
				
		}
		[DllImport("user32", EntryPoint = "GetWindowLong")]
		public static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

		[DllImport("user32", EntryPoint = "SetWindowLong")]
		public static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, int dsNewLong);

		[DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
		public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, int crKey, byte alpha, LWA dwFlags);
		
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr parentHandle,
		                                         IntPtr childAfter, string className,  IntPtr windowTitle);
		// Win32 constants
		public const int WM_GETTEXT = 0x000D;
		public const int WM_GETTEXTLENGTH = 0x000E;
		
		[DllImport("User32.Dll")]
		public static extern void GetClassName(IntPtr hWnd, StringBuilder s, int nMaxCount);

		 [DllImport("User32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, StringBuilder lParam);

        [DllImport("User32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		
		[DllImport("user32.dll")]
		public static extern long GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);
		
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern int GetClassName(int hWnd, StringBuilder lpClassName,int nMaxCount);
		
		[DllImport("User32.dll")]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
		
		[DllImport("user32.dll")]
		public static extern bool SetWindowText(IntPtr hWnd, string lpString);
		
		[DllImport("user32")]
		public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

		// EnumChildWindows works just like EnumWindows, except we can provide a parameter that specifies the parent
		// window handle. If this is NULL or zero, it works just like EnumWindows. Otherwise it'll only return windows
		// whose parent window handle matches the hWndParent parameter.
		[DllImport("user32.Dll")]
		public static extern Boolean EnumChildWindows(IntPtr hWndParent, PChildCallBack lpEnumFunc, int lParam);
		
		// The PChildCallBack delegate that we used with EnumWindows.
		public delegate bool PChildCallBack(IntPtr hWnd, int lParam);
		
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool IsWindowVisible(IntPtr hWnd);
		
		public const int GW_HWNDNEXT = 2;
		public const int GW_HWNDLAST = 1;
		public const int GW_HWNDFIRST = 0;
		
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
		
		[DllImport("user32.dll")]
		public static extern IntPtr GetTopWindow(int hWnd);
		
		[DllImport("user32.dll")]
		public static extern bool SetCursorPos(int X, int Y);

		public enum MouseEventFlags
		{
			LEFTDOWN   = 0x00000002,
			LEFTUP     = 0x00000004,
			MIDDLEDOWN = 0x00000020,
			MIDDLEUP   = 0x00000040,
			MOVE       = 0x00000001,
			ABSOLUTE   = 0x00008000,
			RIGHTDOWN  = 0x00000008,
			RIGHTUP    = 0x00000010
		}
		[DllImport("user32.dll")]
		static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData,  int dwExtraInfo);
		
		//Set cursor position
		//SetCursorPos(10, 50);
		////Mouse Right Down and Mouse Right Up
		//mouse_event((uint)MouseEventFlags.RIGHTDOWN,0,0,0,0);
		//mouse_event((uint)MouseEventFlags.RIGHTUP,0,0,0,0);
		
		//KeyCodes @ http://msdn.microsoft.com/en-us/library/ms645540.aspx
		[DllImport("user32.dll")]
		static extern void keybd_event(byte bVk, byte bScan, uint dwFlags,
		                               UIntPtr dwExtraInfo);

//				public const byte VK_LSHIFT= 0xA0; // left shift key
		//    public const byte VK_TAB = 0x09;
		//    public const int KEYEVENTF_EXTENDEDKEY = 0x01;
		//    public const int KEYEVENTF_KEYUP = 0x02;

		//    //press the shift key
		//    keybd_event(VK_LSHIFT, 0x45, 0, 0);
//
		//    //press the tab key
		//    keybd_event(VK_TAB, 0x45, 0, 0);
//
		//    //release the tab key
		//    keybd_event(VK_TAB, 0x45, KEYEVENTF_KEYUP, 0);
//
		//    //release the shift key
		//    keybd_event(VK_LSHIFT, 0x45, KEYEVENTF_KEYUP, 0);
		
		

	}
}
