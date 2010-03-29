namespace PokerTell.Plugins.InstantHandHistoryReader
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    using Interfaces;

    using log4net;

    /// <summary>
    /// Finds all Handhistories from Pokerroom's memory
    /// </summary>
    public class HandHistoryReader : IHandHistoryReader
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        Kernel32.MEMORY_BASIC_INFORMATION _memBasicInfo;

        Process _pokerClientProcess;

        ProcessMemoryReader _procMemReader;

        IList<long> _readGameIds;

        int _regionBufferSize;

        Kernel32.SYSTEM_INFO _sysInfo;

        string _processName;

        string _valueThatHandHistoryStartsWith;

        public IHandHistoryReader InitializeWith(string processName, string valueThatHandHistoryStartsWith)
        {
            _processName = processName;
            _valueThatHandHistoryStartsWith = valueThatHandHistoryStartsWith;
            _readGameIds = new List<long>();

            WasSuccessfullyInitialized = true;

            return this;
        }

        void PrepareReader()
        {
            if (!FindProcess(_processName))
            {
                string msg = "Couldn't find a running " + _processName;
                throw new ApplicationException(msg);
            }

            CreateMemoryReaderAndGetSystemInfo();
        }

        public bool WasSuccessfullyInitialized { get; protected set; }

        public IList<string> FindNewInstantHandHistories()
        {
            PrepareReader();
            return ReadAllNewGames();
        }

        void CreateMemoryReaderAndGetSystemInfo()
        {
            _sysInfo = new Kernel32.SYSTEM_INFO();
            Kernel32.GetSystemInfo(ref _sysInfo);

            // Create memory reader
            _procMemReader = new ProcessMemoryReader { ReadProcess = _pokerClientProcess };

            // Point Reader at PokerClient
            _procMemReader.OpenProcess();

            _memBasicInfo = new Kernel32.MEMORY_BASIC_INFORMATION();
        }

        bool FindProcess(string processName)
        {
            // Find a buffer with the name in it.
            // Search the Hearts Process
            Process[] pArray = Process.GetProcessesByName(processName);

            if (pArray.Length == 0)
                return false;
            
            _pokerClientProcess = pArray[0];

            return true;
        }

        static int FindStartValueInMemory(string startStr, byte[] bufMemory, int bufMemorySize, int iMemoryStartAddress)
        {
            // Tries to find startstr in Memory (bufMemory) and returns the index of the buffer
            // at which it was found

            // Seems to be very inefficient
            // Maybe casting the bufMemory to string and using RegEx would help?

            // Finds Hand containing startstr in buffer
            char[] bufSearchValue = startStr.Trim(new[] { '_' }).ToCharArray();
            int bufSearchValueSize = bufSearchValue.GetLength(0);

            bool found = false;

            try
            {
                // Steps thru memory by changing memory start address
                // then tries to match chars in memory with chars in SearchValue
                // returns MemoryStart Adress if it matched all the way
                while (iMemoryStartAddress < bufMemorySize)
                {
                    int iParseSearchValue = 0;
                    int iParseMemArea = iMemoryStartAddress;

                    while ((iParseSearchValue < bufSearchValueSize) && (iParseMemArea < bufMemorySize) && (! found))
                    {
                        // Skip the wildcards.
                        while ((iParseSearchValue < bufSearchValueSize) && (bufSearchValue[iParseSearchValue] == '_'))
                        {
                            iParseSearchValue++;
                        }

                        if (iParseSearchValue < bufSearchValueSize)
                        {
                            if (bufSearchValue[iParseSearchValue] == (char)bufMemory[iParseMemArea])
                            {
                                iParseSearchValue++;
                                iParseMemArea++;

                                // Get here if all chars in SearchValue so far matched the chars in Memory area
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

                    // Not found yet -> start at next char in memory this time
                    // else return where it was found
                    if (found)
                        return iMemoryStartAddress;

                    iMemoryStartAddress++;
                }
            }
            catch (Exception excep)
            {
                Log.Error(excep);
            }

            return 0;
        }

        unsafe IList<string> ReadAllNewGames()
        {
            int startOfGameId = _valueThatHandHistoryStartsWith.Length;
            int lengthOfGameId = "23769815844".Length;

            byte* lpMem = null;
            _regionBufferSize = 0;
            var readHands = new List<string>();

            while (lpMem < (byte*)(void*)_sysInfo.lpMaximumApplicationAddress)
            {
                Kernel32.VirtualQueryEx(_pokerClientProcess.Handle, 
                                        (IntPtr)(void*)lpMem,
                                        out _memBasicInfo,
                                        (UIntPtr) sizeof(Kernel32.MEMORY_BASIC_INFORMATION));

                // Make sure memory is readable
                if (_memBasicInfo.Protect != 1)
                {
                    // Read entire Process Memory into buffer
                    byte[] memoryBuffer = _procMemReader.ReadProcessMemory((IntPtr)(void*)lpMem, 
                                                                           _memBasicInfo.RegionSize, 
                                                                           out _regionBufferSize);

                    int nextValueMemAddress = 1;
                    int bufferIndex = 0;

                    // Do until no more values are found in memory
                    while (nextValueMemAddress > 0)
                    {
                        nextValueMemAddress = FindStartValueInMemory(_valueThatHandHistoryStartsWith, 
                                                                     memoryBuffer, 
                                                                     _regionBufferSize, 
                                                                     bufferIndex);

                        // No more Values found
                        if (nextValueMemAddress <= 0) break;

                        string strGameId = string.Empty;

                        // Read HandId and determine if we already delivered it
                        for (int iBuf = nextValueMemAddress + startOfGameId; iBuf < nextValueMemAddress + startOfGameId + lengthOfGameId; iBuf++)
                        {
                            strGameId += (char)memoryBuffer[iBuf];
                        }

                        long gameId;
                        if (long.TryParse(strGameId, out gameId) && ! _readGameIds.Contains(gameId))
                        {
                            // Add Value if it hasn't been delivered before
                            readHands.Add(ReadValueAt(nextValueMemAddress, memoryBuffer));
                            _readGameIds.Add(gameId);
                        }

                        bufferIndex = nextValueMemAddress + startOfGameId + lengthOfGameId;
                    }
                }

                /* increment lpMem to next region of memory */
                lpMem = (byte*)(void*)_memBasicInfo.BaseAddress;
                lpMem += _memBasicInfo.RegionSize;
            }

            return readHands;
        }

        static string ReadValueAt(int startMemAddress, byte[] memoryBuffer)
        {
            // Read the text of the Value
            string handHistory = string.Empty;
            for (int i = startMemAddress; i < memoryBuffer.Length; i++)
            {
                // If we found end of line and next byte is not readable
                // we are at the end of the Value
                if (memoryBuffer[i] == 0x0a && memoryBuffer[i + 1] == 0)
                {
                    break;
                }

                handHistory += (char)memoryBuffer[i];
            }

            return handHistory;
        }
    }
}