using System;
using System.Diagnostics;

namespace DllWrappers
{
	public class ProcessMemoryReader
	{
	
	    public ProcessMemoryReader()
	    {
	    }
	
	    /// <summary>	
	    /// Process from which to read		
	    /// </summary>
	    public Process ReadProcess
	    {
	        get
	        {
	            return m_ReadProcess;
	        }
	        set
	        {
	            m_ReadProcess = value;
	        }
	    }
	
	    private Process m_ReadProcess = null;
	
	    private IntPtr m_hProcess = IntPtr.Zero;
	
	    public void OpenProcess()
	    {
	        Kernel32.ProcessAccessType access;
	        access = Kernel32.ProcessAccessType.PROCESS_VM_READ
	            | Kernel32.ProcessAccessType.PROCESS_VM_WRITE
	            | Kernel32.ProcessAccessType.PROCESS_VM_OPERATION;
	        m_hProcess = Kernel32.OpenProcess((uint)access, 1, (uint)m_ReadProcess.Id);
	    }
	
	    public void CloseHandle()
	    {
	        int iRetValue;
	        iRetValue = Kernel32.CloseHandle(m_hProcess);
	        if (iRetValue == 0)
	            throw new Exception("CloseHandle failed");
	    }
	
	    public byte[] ReadProcessMemory(IntPtr MemoryAddress, uint bytesToRead, out int bytesRead)
	    {
	        byte[] buffer = new byte[bytesToRead];
	
	        IntPtr ptrBytesRead;
	        Kernel32.ReadProcessMemory(m_hProcess, MemoryAddress, buffer, bytesToRead, out ptrBytesRead);
	
	        bytesRead = ptrBytesRead.ToInt32();
	
	        return buffer;
	    }
	
	    public void WriteProcessMemory(IntPtr MemoryAddress, byte[] bytesToWrite, out int bytesWritten)
	    {
	        IntPtr ptrBytesWritten;
	        Kernel32.WriteProcessMemory(m_hProcess, MemoryAddress, bytesToWrite, (uint)bytesToWrite.Length, out ptrBytesWritten);
	
	        bytesWritten = ptrBytesWritten.ToInt32();
	    }
	
	}
}
