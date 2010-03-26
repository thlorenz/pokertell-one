namespace PokerTell.Plugins.InstantHandHistoryReader
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// ProcessMemoryReader is a class that enables direct reading a process memory
    /// </summary>
    public class Kernel32
    {
        // constants information can be found in <winnt.h>
        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000, 
            Reserve = 0x2000, 
            Decommit = 0x4000, 
            Release = 0x8000, 
            Reset = 0x80000, 
            Physical = 0x400000, 
            TopDown = 0x100000
        }

        [Flags]
        public enum ProcessAccessType
        {
            PROCESS_TERMINATE = 0x0001, 
            PROCESS_CREATE_THREAD = 0x0002, 
            PROCESS_SET_SESSIONID = 0x0004, 
            PROCESS_VM_OPERATION = 0x0008, 
            PROCESS_VM_READ = 0x0010, 
            PROCESS_VM_WRITE = 0x0020, 
            PROCESS_DUP_HANDLE = 0x0040, 
            PROCESS_CREATE_PROCESS = 0x0080, 
            PROCESS_SET_QUOTA = 0x0100, 
            PROCESS_SET_INFORMATION = 0x0200, 
            PROCESS_QUERY_INFORMATION = 0x0400
        }

        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(IntPtr hObject);

        // function declarations are found in the MSDN and in <winbase.h>
        [DllImport("kernel32.dll")]
        public static extern void GetSystemInfo([MarshalAs(UnmanagedType.Struct)] ref SYSTEM_INFO lpSystemInfo);

        // 		HANDLE OpenProcess(
        // 			DWORD dwDesiredAccess,  // access flag
        // 			BOOL bInheritHandle,    // handle inheritance option
        // 			DWORD dwProcessId       // process identifier
        // 			);
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

        // 		BOOL CloseHandle(
        // 			HANDLE hObject   // handle to object
        // 			);

        // 		BOOL ReadProcessMemory(
        // 			HANDLE hProcess,              // handle to the process
        // 			LPCVOID lpBaseAddress,        // base of memory area
        // 			LPVOID lpBuffer,              // data buffer
        // 			SIZE_T nSize,                 // number of bytes to read
        // 			SIZE_T * lpNumberOfBytesRead  // number of bytes read
        // 			);
        [DllImport("kernel32.dll")]
        public static extern int ReadProcessMemory(
            IntPtr hProcess, IntPtr lpBaseAddress, [In] [Out] byte[] buffer, uint size, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern unsafe bool VirtualFreeEx(
            IntPtr hProcess, 
            byte* pAddress, 
            UIntPtr size, 
            AllocationType freeType);

        [DllImport("kernel32.dll")]
        public static extern uint VirtualQueryEx(
            IntPtr hProcess, 
            IntPtr lpAddress, 
            out MEMORY_BASIC_INFORMATION lpBuffer, 
            UIntPtr dwLength);

        // 		BOOL WriteProcessMemory(
        // 			HANDLE hProcess,                // handle to process
        // 			LPVOID lpBaseAddress,           // base of memory area
        // 			LPCVOID lpBuffer,               // data buffer
        // 			SIZE_T nSize,                   // count of bytes to write
        // 			SIZE_T * lpNumberOfBytesWritten // count of bytes written
        // 			);
        [DllImport("kernel32.dll")]
        public static extern int WriteProcessMemory(
            IntPtr hProcess, IntPtr lpBaseAddress, [In] [Out] byte[] buffer, uint size, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(
            IntPtr hProcess, 
            IntPtr lpAddress, 
            UIntPtr dwSize, 
            uint flAllocationType, 
            uint flProtect);

        [StructLayout(LayoutKind.Explicit)]
        public struct _PROCESSOR_INFO_UNION
        {
            [FieldOffset(0)]
            internal uint dwOemId;

            [FieldOffset(0)]
            internal ushort wProcessorArchitecture;

            [FieldOffset(2)]
            internal ushort wReserved;
        }

        /* 
		 * SIZE_T WINAPI VirtualQueryEx(
		 *	  __in      HANDLE hProcess,
		 *	  __in_opt  LPCVOID lpAddress,
		 *	  __out     PMEMORY_BASIC_INFORMATION lpBuffer,
		 *	  __in      SIZE_T dwLength
		 *	);
		 *
		 *	hProcess [in]
		 *	    A handle to the process whose memory information is queried. 
		 *		The handle must have been opened with the PROCESS_QUERY_INFORMATION access right,
		 *  which enables using the handle to read information from the process object. 
		 * For more information, see Process Security and Access Rights.
		 * 
		 * 	lpAddress [in, optional]
		 *
		 *	    A pointer to the base address of the region of pages to be queried.
		 *		This value is rounded down to the next page boundary. 
		 * To determine the size of a page on the host computer, use the GetSystemInfo function.
		 *	
		 * lpBuffer [out]
		 *
		 *	    A pointer to a MEMORY_BASIC_INFORMATION structure in which 
		 * information about the specified page range is returned.
		 *	
		 * dwLength [in]
		 *
		 *	    The size of the buffer pointed to by the lpBuffer parameter, in bytes.
		 *
		 *	Return Value
		 *
		 *	The return value is the actual number of bytes returned in the information buffer.
		 */

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public uint BaseAddress;

            public uint AllocationBase;

            public uint AllocationProtect;

            public uint RegionSize;

            public uint State;

            public uint Protect;
            public uint Type;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO
        {
            internal _PROCESSOR_INFO_UNION uProcessorInfo;

            public uint dwPageSize;

            public uint lpMinimumApplicationAddress;

            public uint lpMaximumApplicationAddress;

            public uint dwActiveProcessorMask;

            public uint dwNumberOfProcessors;

            public uint dwProcessorType;

            public uint dwAllocationGranularity;

            public uint dwProcessorLevel;

            public uint dwProcessorRevision;
        }
    }
}