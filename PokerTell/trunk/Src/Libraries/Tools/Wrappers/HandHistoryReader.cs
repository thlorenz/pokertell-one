/*
Created by SharpDevelop.
 * Thorsten Lorenz
 * Date: 1/15/2009
 * Time: 4:54 PM
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using DllWrappers;

namespace Tools.Wrappers
{
    /// <summary>
    /// Finds all Handhistories from Pokerroom's memory
    /// </summary>
    public class HandHistoryReader
    {
        #region Fields
        Process myProcess;
		
        Kernel32.SYSTEM_INFO SysInfo;
        Kernel32.MEMORY_BASIC_INFORMATION MemBasicInfo;
		
        ProcessMemoryReader ProcMemReader;
		
		
        List <long> ReadGameIds;
		
        string strValueStartsWith;
        int TourneyId;
		
        int RegionBufferSize;
        #endregion
		
        #region Constructor
        public HandHistoryReader(string strProcName, string strValueStartsWith){
            if(!FindProcess(strProcName)){
                string msg = "Couldn't find a running " + strProcName;
                throw new ApplicationException(msg);
            }
            else{
                this.strValueStartsWith = strValueStartsWith;
                if(!Int32.TryParse(strValueStartsWith,out this.TourneyId)){
                    this.TourneyId = 0;
                }
				
                CreateMemoryReaderAndGetSystemInfo();
                ReadGameIds = new List<long>();
            }
        }
        #endregion
		
        #region Methods
		
        #region Find Process
        private bool FindProcess(string strProcName)
        {
			
            Process[] pArray;
			
            // Find a buffer with the name in it.
            // Search the Hearts Process
            pArray = Process.GetProcessesByName(strProcName);
			
            if (pArray.Length == 0)
            {
                return false;
            }
            else{
                this.myProcess = pArray[0];
            }

			
			
            return true;
        }
        #endregion
		
        #region CreateMemoryReaderAndGetSystemInfo
        private void CreateMemoryReaderAndGetSystemInfo(){
            this.SysInfo = new Kernel32.SYSTEM_INFO();
            Kernel32.GetSystemInfo(ref this.SysInfo);

            // Create memory reader
            this.ProcMemReader = new ProcessMemoryReader();

            //Point Reader at PokerStars
            this.ProcMemReader.ReadProcess = this.myProcess;

            this.ProcMemReader.OpenProcess();
			
            this.MemBasicInfo = new Kernel32.MEMORY_BASIC_INFORMATION();
        }
        #endregion
		
        #region FindInstantHandHistories
        public List<string> FindNewInstantHandHistories()
        {
            return ReadAllNewGames();
        }
        #endregion
		
        #region ReadAllNewGames
        private unsafe List<string> ReadAllNewGames()
        {
            byte* lpMem;
            byte [] MemoryBuffer;
            int NextValueMemAddress;
            int BufferIndex;
			
            string strGameId;
            long GameId;
			
            int StartOfGameId = "PokerStars Game #".Length; 
            int LengthOfGameId = "23769815844".Length;
			
            List<string> ReadHands;
			
            lpMem = null;
            MemoryBuffer = null;
            this.RegionBufferSize = 0;
            ReadHands =  new List<string>();
			
            while (lpMem < (byte*)(void*)this.SysInfo.lpMaximumApplicationAddress)
            {
                Kernel32.VirtualQueryEx(myProcess.Handle,
                                        (IntPtr)(void*)lpMem,
                                        out this.MemBasicInfo,
                                        (System.UIntPtr)sizeof(Kernel32.MEMORY_BASIC_INFORMATION));
				
                //Make sure memory is readable
                if (this.MemBasicInfo.Protect != 1)
                {
                    //Read entire Process Memory into buffer
                    MemoryBuffer = this.ProcMemReader.ReadProcessMemory((IntPtr)(void*)lpMem,
                                                                        this.MemBasicInfo.RegionSize,
                                                                        out this.RegionBufferSize);
					

                    NextValueMemAddress = 1;
                    BufferIndex = 0;
					
                    //Do until no more values are found in memory
                    while (NextValueMemAddress > 0)
                    {
                        NextValueMemAddress = FindStartValueInMemory(this.strValueStartsWith,
                                                                     MemoryBuffer,
                                                                     this.RegionBufferSize,
                                                                     BufferIndex);
						
                        //No more Values found
                        if (NextValueMemAddress <= 0) break;
						
                        strGameId = string.Empty;
                        //Read HandId and determine if we already delivered it
                        for(int iBuf = NextValueMemAddress + StartOfGameId;
                            iBuf < NextValueMemAddress + StartOfGameId + LengthOfGameId;
                            iBuf++)
                        {
                            strGameId+= (char) MemoryBuffer[iBuf];
                        }
						
                        if(long.TryParse(strGameId,out GameId) &&
                           ! this.ReadGameIds.Contains(GameId))
                        {
                            //Add Value if it hasn't been delivered before
                            ReadHands.Add(ReadValueAt(NextValueMemAddress, MemoryBuffer));
                            this.ReadGameIds.Add(GameId);
                        }
                        BufferIndex= NextValueMemAddress + 	StartOfGameId + LengthOfGameId;
                    }
                }

                /* increment lpMem to next region of memory */
                lpMem = (byte*)(void*)this.MemBasicInfo.BaseAddress;
                lpMem += this.MemBasicInfo.RegionSize;
            }
			
            return ReadHands;
        }
        #endregion
		
        #region Read Value
        private string ReadValueAt(int StartMemAddress, byte[] MemoryBuffer)
        {
            string strHH;
			
            //Read the text of the Value
            strHH = string.Empty;
            for(int i=StartMemAddress; i<MemoryBuffer.Length;i++){
                //If we found end of line and next byte is not readable
                //we are at the end of the Value
                if(MemoryBuffer[i] == 0x0a && MemoryBuffer[i+1] == 0){
                    break;
                }
                strHH+= (char) MemoryBuffer[i];
            }
            return strHH;
        }
        #endregion
		
        #region FindStartValueInMemory
        private int FindStartValueInMemory(string start_str, byte[] bufMemory, int bufMemorySize, int iMemoryStartAddress)
        {
            //Tries to find startstr in Memory (bufMemory) and returns the index of the buffer
            //at which it was found
			
            //Seems to be very inefficient
            //Maybe casting the bufMemory to string and using RegEx would help?
			
            //Finds Hand containing startstr in buffer
            char[] bufSearchValue = start_str.Trim(new char[] { '_' }).ToCharArray();
            int bufSearchValueSize = bufSearchValue.GetLength(0);
			
            bool found = false;

            try
            {
                //Steps thru memory by changing memory start address
                //then tries to match chars in memory with chars in SearchValue
                //returns MemoryStart Adress if it matched all the way
                while ((iMemoryStartAddress < bufMemorySize) && (!found))
                {
                    int iParseSearchValue = 0;
                    int iParseMemArea = iMemoryStartAddress;

                    while ((iParseSearchValue < bufSearchValueSize) && 
                           (iParseMemArea < bufMemorySize) && (!found))
                    {
                        // Skip the wildcards.
                        while ((iParseSearchValue < bufSearchValueSize) && 
                               (bufSearchValue[iParseSearchValue] == '_'))
                        {
                            iParseSearchValue++;
                        }
						
                        if (iParseSearchValue < bufSearchValueSize)
                        {
                            if (bufSearchValue[iParseSearchValue] == (char)bufMemory[iParseMemArea])
                            {
                                iParseSearchValue++;
                                iParseMemArea++;
                                //Get here if all chars in SearchValue so far matched the chars in Memory area
                                if (iParseSearchValue == bufSearchValueSize)
                                {
                                    found = true;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
					
                    //Not found yet -> start at next char in memory this time
                    //else return where it was found
                    if (found)
                    {
                        return iMemoryStartAddress;
                    }
                    else
                    {
                        iMemoryStartAddress++;
                    }
                }
            }
            catch (Exception excep)
            {
                System.Console.WriteLine(excep.ToString());
            }

            return 0;
        }
        #endregion
		
        #endregion
    }
}