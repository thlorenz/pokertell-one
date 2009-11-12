#region Using Directives
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

#endregion


namespace Tools
{
	/// <summary>
	/// Contains various static methods
	/// </summary>
	public class Static
	{
		#region FindNewestFileInDirectory

		/// <summary>
		/// FInds the newest file with a given extension  in a given directory
		/// </summary>
		/// <param name="str_dir">Full directory path</param>
		/// <param name="extension">File estension</param>
		/// <returns>Short Filename of newest file</returns>
		public static string FindNewestFileInDirectory(string str_dir, string extension)
		{
			
			DateTime  record_time;
			
			string filename=string.Empty ;
			
			DirectoryInfo dir = new DirectoryInfo(@str_dir);
			FileInfo[] files = dir.GetFiles("*." + extension);
			
			record_time=DateTime.Now.AddYears(-5); //preset record to 5 years old
			
			foreach(FileInfo file in files){
				if(file.CreationTime.CompareTo(record_time)>0)
				{
					record_time = file.CreationTime;
					filename = file.Name;
				}
				
			}
			return filename;
		}
		
		#endregion
		
		#region ThisPointIsOnOneOfTheConnectedScreens
		public static bool ThisPointIsOnOneOfTheConnectedScreens(Point thePoint)
		{
			bool foundScreenThatContainsThePoint = false;

			for(int i = 0; i < Screen.AllScreens.Length; i++)
			{
				if(Screen.AllScreens[i].Bounds.Contains(thePoint))
					foundScreenThatContainsThePoint = true;
			}
			return foundScreenThatContainsThePoint;
			
		}
		#endregion
		
		public static string RemoveDirectoryAndExtensionFromFile(string fileName)
        {
            int extensionLength = new FileInfo(fileName).Extension.Length;
            string databaseNameWithExtension = new FileInfo(fileName).Name;
            
            if (extensionLength > 0)
            {
                //Make sure "." is also gone after removal
                return databaseNameWithExtension.Substring(0, databaseNameWithExtension.Length - extensionLength);
            }
            else
            {
                return databaseNameWithExtension;
            }
		}
		
		public static string GetUserDataPath(string applicationName)
		{
		    string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		    dir = System.IO.Path.Combine(dir, applicationName);
		    if (! Directory.Exists(dir))
		        Directory.CreateDirectory(dir);
		    return dir;
		}
		
		public static string GetCommonDataPath(string applicationName)
		{
		    string dir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
		    dir = System.IO.Path.Combine(dir, applicationName);
		    if (! Directory.Exists(dir))
		        Directory.CreateDirectory(dir);
		    return dir;
		}
		
		public static void CopyDirectory(string source, string destination)
		{
		    if (destination[destination.Length - 1] != Path.DirectorySeparatorChar) {
		        destination += Path.DirectorySeparatorChar;
		    }
		    
		    if (!Directory.Exists(destination)) {
		        Directory.CreateDirectory(destination);
		    }
		    
		    String[] entries = Directory.GetFileSystemEntries(source);
		    foreach(string item in entries)
		    {
		        if (Directory.Exists(item)) {
		            CopyDirectory(item, destination + Path.GetFileName(item));
		        }
		        else {
		            File.Copy(item,destination+Path.GetFileName(item),true);
		        }
		    }
		}


		
		public static Bitmap TakeScreenShotAndSaveJpegAs(string fullPath)
		{
		    int screenWidth = Screen.GetBounds(new Point(0, 0)).Width;
		    int screenHeight = Screen.GetBounds(new Point(0, 0)).Height;
		    
		    Bitmap bmpScreenShot = new Bitmap(screenWidth, screenHeight);
		    
		    Graphics gfx = Graphics.FromImage((Image) bmpScreenShot);
		    
		    gfx.CopyFromScreen(0, 0, 0, 0, new Size(screenWidth, screenHeight));
		    
		    if ( File.Exists(fullPath))
		    {
		        File.Delete(fullPath);
		    }
		    
		    bmpScreenShot.Save(fullPath, ImageFormat.Jpeg);
		    
		    return bmpScreenShot;
		}
		
		public static void NameThread(string name)
		{
			Thread currentThread = Thread.CurrentThread;
			if (string.IsNullOrEmpty(currentThread.Name)) {
				currentThread.Name = name + " [" + currentThread.ManagedThreadId + "]";
			}
		}
		
		#region BuildRegExToMatchRange
		/// <summary>
		/// Builds a Regex Pattern that only matches values for the given range
		/// </summary>
		/// <param name="strMin">Lower end of Range of format ##.#</param>
		/// <param name="strMax">Upper end of Range of format ##.#</param>
		/// <param name="TwoDigitsAfterPeriod">
		/// If true it produces a pattern matching ##.## and will
		/// include numbers that would round up or down to the desired ranges.
		/// e.g. 0.3 - 0.5 will produce patterns matching numbers between 0.25 and 0.54.
		/// If false it produces a pattern matching ##.# (no rounding required)
		/// </param>
		/// <returns>Regex Pattern matching values for the range</returns>
		public static string BuildRegExToMatchRange(string strMin,string strMax, bool TwoDigitsAfterPeriod)
		{
		    //Comments above patterns apply to one digit after period patterns
		    
		    Match m;
		    string patOneDigitAfterPeriodDouble = @"(?<d1>\d){0,1}(?<d2>\d)\.(?<d3>\d)";
		    
		    //Min [I1][I2].[I3]
		    int I1,I2,I3;
		    
		    //Max [A1][A2].[A3]
		    int A1,A2,A3;
		    
		    string patD2,patD1; //digit Expressions
		    
		    if(strMin==string.Empty){
		        return string.Empty;
		    }
		    
		    m=Regex.Match(strMin,patOneDigitAfterPeriodDouble);
		    I3=int.Parse(m.Groups["d3"].ToString());
		    I2=int.Parse(m.Groups["d2"].ToString());
		    
		    if(m.Groups["d1"].ToString()!=string.Empty)
		        I1=int.Parse(m.Groups["d1"].ToString());
		    else
		        I1=-1;
		    
		    m=Regex.Match(strMax,patOneDigitAfterPeriodDouble);
		    A3=int.Parse(m.Groups["d3"].ToString());
		    A2=int.Parse(m.Groups["d2"].ToString());
		    
		    if(m.Groups["d1"].ToString()!=string.Empty)
		        A1=int.Parse(m.Groups["d1"].ToString());
		    else
		        A1=-1;
		    
		    string patLimitUpperRange;
		    //Digit2
		    if(I2 == A2)
		    {
		        //LastDigit(3)
		        if(! TwoDigitsAfterPeriod)
		        {
		            patLimitUpperRange = string.Format("[{0}-{1}]",I3, A3);
		        }
		        else
		        {
		            patLimitUpperRange = A3 > I3
		                ? string.Format("(([{0}-{1}][0-9])|({2}[0-4]))",I3 ,A3 -1, A3)
		                : string.Format("{0}[0-4]",A3);
		        }
		        
		        patD2 = string.Format("{0}[.]{1}",I2,patLimitUpperRange);
		    }
		    else if(I2+1 < A2)
		    {
		        if(! TwoDigitsAfterPeriod)
		        {
		            patLimitUpperRange = string.Format("[0-{0}]",A3);
		        }
		        else
		        {
		            patLimitUpperRange = A3 > 0
		                ? string.Format("(([0-{0}][0-9])|({1}[0-4]))",A3 -1, A3)
		                : string.Format("{0}[0-4]",A3);
		        }
		        
		        //((I2.[I3-9])  | ([(I2+1)-(A2-1)].[0-9]) |  (A2.[0-A3] ))
		        patD2=string.Format("(({0}[.][{1}-9])|([{2}-{3}][.][0-9])|({4}[.]{5}))",I2,I3,I2+1,A2-1,A2,patLimitUpperRange);
		    }
		    
		    else
		    {
		        if(! TwoDigitsAfterPeriod)
		        {
		            patLimitUpperRange = string.Format("[0-{0}]",A3);
		        }
		        else
		        {
		            patLimitUpperRange = A3 > 0
		                ? string.Format("(([0-{0}][0-9])|({1}[0-4]))",A3 -1, A3)
		                : string.Format("{0}[0-4]",A3);
		        }
		        
		        //((I2.I3-9]) |  (A2.[0-A3] ))
		        patD2=string.Format("(({0}[.][{1}-9])|({2}[.]{3}))",I2,I3,A2,patLimitUpperRange);
		    }
		    
		    //Digit3
		    if(A1>-1)
		    {
		        if(I1>-1)
		        {
		            if(I1==A1)
		            {
		                patD1=string.Format("{0}{1}",I1,patD2);
		            }else if(I1+1<A1){
		                //((I1[I2-9].[0-9])  | ([(I1+1)-(A1-1)][0-9].[0-9]) |  (A1[0-(A2-1)].[0-9])|(A1A2.[0-A3]))
		                patD1=string.Format("(({0}[{1}-9][.][0-9])|([{2}-{3}][0-9][.][0-9])|({4}[0-{5}][.][0-9])|({4}{6}[.]{7}))",
		                                    I1,I2,I1+1,A1-1,A1,A2-1,A2,patLimitUpperRange);
		            }
		            
		            else{
		                //((I1[I2-9].[0-9]]) |  (A1[0-(A2-1)].[0-9])|(A1A2.[0-A3]))
		                patD1=string.Format("(({0}[{1}-9][.][0-9])|({2}[0-{4}][.][0-9])|({2}{3}[.]{5}))"
		                                    ,I1,I2,A1,A2,A2-1,patLimitUpperRange);
		            }
		            
		            
		        }
		        else if(A1>1){//Min is 2 digit and Max is 3 digit
		            if(A2>0){
		                //(((I2.[I3-9])|([(I2+1)-9].[0-9]) | ([1-(A1-1)][0-9].[0-9]) |  (A1[0-(A2-1)].[0-9])|(A1A2.[0-A3]))
		                patD1=string.Format("(({0}[.][{1}-9])|([{2}-9][.][0-9])|([1-{3}][0-9][.][0-9])|({4}[0-{5}][.][0-9])|({4}{6}[.]{7}))",
		                                    I2,I3,I2+1,A1-1,A1,A2-1,A2,patLimitUpperRange);
		            }else{
		                //(((I2.[I3-9])|([(I2+1)-9].[0-9]) | ([1-(A1-1)][0-9].[0-9]) | (A1A2.[0-A3]))
		                patD1=string.Format("(({0}[.][{1}-9])|([{2}-9][.][0-9])|([1-{3}][0-9][.][0-9])|({4}{5}[.]{6}))",
		                                    I2,I3,I2+1,A1-1,A1,A2,patLimitUpperRange);
		            }
		        }
		        else{
		            if(A2>0){
		                //(((I2.[I3-9])|([(I2+1)-9].[0-9])|  (1[0-(A2-1)].[0-9])|(1A2.[0-A3]))
		                patD1=string.Format("(({0}[.][{1}-9])|([{2}-9][.][0-9])|(1[0-{4}][.][0-9])|(1{3}[.]{5}))"
		                                    ,I2,I3,I2+1,A2,A2-1,patLimitUpperRange);
		            }else{
		                //((I2.[I3-9])|([(I2+1)-9].[0-9]) | (1A2.[0-A3]))
		                patD1=string.Format("(({0}[.][{1}-9])|([{2}-9][.][0-9])|(1{3}[.]{4}))"
		                                    ,I2,I3,I2+1,A2,patLimitUpperRange);
		            }
		        }
		    }else{
		        patD1 = patD2;
		    }
		    
		    if(TwoDigitsAfterPeriod)
		    {
		        //Reproduce number as x.y
		        int x = I1 > -1
		            ? I1 * 10 + I2
		            : I2;
		        
		        int y = I3;
		        
		        string patIncludeLowerEnd;
		        if(y > 0)
		        {
		            patIncludeLowerEnd = string.Format("({0}[.]{1}[5-9])|",x,y -1);
		        }
		        else
		        {
		            patIncludeLowerEnd = x > 0
		                ? string.Format("({0}.9[5-9])|",x - 1)
		                : string.Empty;
		        }
		        
		        patD1 = string.Format("{0}{1}",patIncludeLowerEnd, patD1);
		    }
		    return patD1;
		    
		}
		#endregion

        public static bool OperatingSystemIsWindowsXPOrOlder()
        {
            return Environment.OSVersion.Platform.Equals(PlatformID.Win32NT)
                   && Environment.OSVersion.Version.Major <= 5;
        }
	}
}


