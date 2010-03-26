namespace PokerTell.Plugins.InstantHandHistoryReader
{
    using System;
    using System.Diagnostics;

    public class ProcessMemoryReader
    {
        IntPtr _hProcess = IntPtr.Zero;

        public ProcessMemoryReader()
        {
            ReadProcess = null;
        }

        /// <summary>	
        /// Process from which to read		
        /// </summary>
        public Process ReadProcess { get; set; }

        public void CloseHandle()
        {
            int returnValue = Kernel32.CloseHandle(_hProcess);
            if (returnValue == 0)
                throw new Exception("CloseHandle failed");
        }

        public void OpenProcess()
        {
            const Kernel32.ProcessAccessType access = Kernel32.ProcessAccessType.PROCESS_VM_READ
                                                      | Kernel32.ProcessAccessType.PROCESS_VM_WRITE
                                                      | Kernel32.ProcessAccessType.PROCESS_VM_OPERATION;

            _hProcess = Kernel32.OpenProcess((uint)access, 1, (uint)ReadProcess.Id);
        }

        public byte[] ReadProcessMemory(IntPtr memoryAddress, uint bytesToRead, out int bytesRead)
        {
            var buffer = new byte[bytesToRead];

            IntPtr ptrBytesRead;
            Kernel32.ReadProcessMemory(_hProcess, memoryAddress, buffer, bytesToRead, out ptrBytesRead);

            bytesRead = ptrBytesRead.ToInt32();

            return buffer;
        }

        public void WriteProcessMemory(IntPtr memoryAddress, byte[] bytesToWrite, out int bytesWritten)
        {
            IntPtr ptrBytesWritten;
            Kernel32.WriteProcessMemory(_hProcess, memoryAddress, bytesToWrite, (uint)bytesToWrite.Length, out ptrBytesWritten);

            bytesWritten = ptrBytesWritten.ToInt32();
        }
    }
}