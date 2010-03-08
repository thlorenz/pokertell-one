using System;
using System.Runtime.InteropServices;
namespace DllWrappers
{
	/// <summary>
	/// Wrapper for CallWindowProc.
	/// </summary>
	public partial class User32
	{
		public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
		
		public struct rect{
			public rect(int left,int top,int right,int bottom)
			{
				this.Left = left;
				this.Top = top;
				this.Right = right;
				this.Bottom = bottom;
			}
			int Left;
			int Top;
			int Right;
			int Bottom;
		}
		public enum MSG:int
		{
			MOVING = 534
		}
		
		[DllImport("user32.dll")]
		static extern IntPtr CallWindowProc(WndProcDelegate lpPrevWndFunc, IntPtr hWnd,
		                                    uint Msg, IntPtr wParam, IntPtr lParam);
		
		
	}
	
}
