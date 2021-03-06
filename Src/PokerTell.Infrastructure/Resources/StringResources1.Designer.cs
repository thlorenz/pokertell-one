﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PokerTell.Infrastructure.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class StringResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal StringResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PokerTell.Infrastructure.Resources.StringResources", typeof(StringResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to connect to the Database.
        /// </summary>
        internal static string Error_Database_UnableToConnect {
            get {
                return ResourceManager.GetString("Error.Database.UnableToConnect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to access registry.
        /// </summary>
        internal static string Error_UnableToAccessRegistry {
            get {
                return ResourceManager.GetString("Error.UnableToAccessRegistry", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PokerTell encountered an unexpected error.
        /// </summary>
        internal static string Error_Unexpected {
            get {
                return ResourceManager.GetString("Error.Unexpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connected to the specified Poker Office Server.
        ///Now you can choose and connect to a Poker Office Database.        ///
        ///.
        /// </summary>
        internal static string Info_ConnectPokerOffice_Connected {
            get {
                return ResourceManager.GetString("Info.ConnectPokerOffice.Connected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Successfully created the {0} database..
        /// </summary>
        internal static string Info_Database_DatabaseCreated {
            get {
                return ResourceManager.GetString("Info.Database.DatabaseCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connect to the MySql Server first by using the Database Settings. .
        /// </summary>
        internal static string Solution_Database_InvalidOperationException {
            get {
                return ResourceManager.GetString("Solution.Database.InvalidOperationException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reinstall the application and/or make sure, that the directory exists..
        /// </summary>
        internal static string Solution_DirectoryNotFoundException {
            get {
                return ResourceManager.GetString("Solution.DirectoryNotFoundException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please reinstall or move the application to a path not as long for example: &quot;C:\Program Files\PokerTell\ &quot;..
        /// </summary>
        internal static string Solution_ExceptionPathTooLongException {
            get {
                return ResourceManager.GetString("Solution.ExceptionPathTooLongException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please check the Poker Office database settings and make sure the MySQL Database in Poker Office is functioning properly.
        /// .
        /// </summary>
        internal static string Solution_ImportPokerOffice_InvalidOperationException {
            get {
                return ResourceManager.GetString("Solution.ImportPokerOffice.InvalidOperationException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The PokerOffice Database is very large. 
        ///PokerTell will now import as many HandHistoriess as possible. 
        ///You may run this import again to import the remaining HandHistoriess .
        /// </summary>
        internal static string Solution_ImportPokerOffice_OutOfMemoryException {
            get {
                return ResourceManager.GetString("Solution.ImportPokerOffice.OutOfMemoryException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Make sure you have access to the drive and directory you are trying to access and that the file you are trying to write to or delete is not currently in use..
        /// </summary>
        internal static string Solution_IOException {
            get {
                return ResourceManager.GetString("Solution.IOException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Make sure that the Hand History for the AutoTracker still exists and/or check the Hand History Folder in the Poker Room Settings..
        /// </summary>
        internal static string Solution_LiveTracker_FileWatcher_ArgumentException {
            get {
                return ResourceManager.GetString("Solution.LiveTracker.FileWatcher.ArgumentException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Add the PokerRoom and enter its settings again via the Manage Rooms option..
        /// </summary>
        internal static string Solution_Main_PokerRooms_FileNotFoundException {
            get {
                return ResourceManager.GetString("Solution.Main.PokerRooms.FileNotFoundException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PokerTell was unable to connect to the host specified in the settings. 
        ///Please make sure the database server is running and check the Database Settings.
        ///Check that the hostname, username and possibly password were entered correctly..
        /// </summary>
        internal static string Solution_MySqlException_ACCESS_DENIED_ERROR {
            get {
                return ResourceManager.GetString("Solution.MySqlException.ACCESS_DENIED_ERROR", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The database you were trying to access doesn&apos;t exist.
        ///Please create and/or choose another database to use with PokerTell..
        /// </summary>
        internal static string Solution_MySqlException_BAD_DB_ERROR {
            get {
                return ResourceManager.GetString("Solution.MySqlException.BAD_DB_ERROR", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Make sure, that the database server is running. 
        ///Another reason for this problem could be, that the host was set incorrectly in the database settings.
        ///In that case you should correct this, generally &apos;localhost&apos; will work..
        /// </summary>
        internal static string Solution_MySqlException_BAD_HOST_ERROR {
            get {
                return ResourceManager.GetString("Solution.MySqlException.BAD_HOST_ERROR", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Make sure you have enough access privileges. Try logging into windows using a different account or talk to your administrator..
        /// </summary>
        internal static string Solution_System_Security_SecurityException {
            get {
                return ResourceManager.GetString("Solution.System.Security.SecurityException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Make sure you have enough access privileges. Try logging into windows using a different account or talk to your administrator..
        /// </summary>
        internal static string Solution_UnauthorizedAccessException {
            get {
                return ResourceManager.GetString("Solution.UnauthorizedAccessException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PokerTell was unable to connect to the PokerOffice Database specified in the Database settings. 
        ///Please make sure the MySql Server is running an/or reconnect to the PokerOffice database using the Database Menu..
        /// </summary>
        internal static string Warning_Database_UnableToConnectToPokerOfficeDatabase {
            get {
                return ResourceManager.GetString("Warning.Database.UnableToConnectToPokerOfficeDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PokerTell was unable to connect to the PokerOffice Database Server using the username and password specified in the settings. Please correct this first before trying to connect to the PokerOffice Database..
        /// </summary>
        internal static string Warning_Database_UnableToConnectToPokerOfficeServer {
            get {
                return ResourceManager.GetString("Warning.Database.UnableToConnectToPokerOfficeServer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a server and username in order to test the connection..
        /// </summary>
        internal static string Warning_DatabaseSettings_InvalidForServerConnect {
            get {
                return ResourceManager.GetString("Warning.DatabaseSettings.InvalidForServerConnect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Couldn&apos;t find a MySQl installation. If you are sure it is installed carefully enter the settings..
        /// </summary>
        internal static string Warning_DatabaseSettings_MySQLNotFound {
            get {
                return ResourceManager.GetString("Warning.DatabaseSettings.MySQLNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to connect. Please make sure the database server is running and check the Database Settings..
        /// </summary>
        internal static string Warning_DatabaseSettings_UnableToConnect {
            get {
                return ResourceManager.GetString("Warning.DatabaseSettings.UnableToConnect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Couldn&apos;t find the specified directory. Please enter a correct one, preferrably using the Browse option..
        /// </summary>
        internal static string Warning_ImportDirectory_DirectoryNotFound {
            get {
                return ResourceManager.GetString("Warning.ImportDirectory.DirectoryNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while trying to import the Hand Histories. No hands were imported..
        /// </summary>
        internal static string Warning_ImportDirectory_ErrorWhileImporting {
            get {
                return ResourceManager.GetString("Warning.ImportDirectory.ErrorWhileImporting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Didn&apos;t find any files with the specified extension in the specified directory and therefore cannot import anything..
        /// </summary>
        internal static string Warning_ImportDirectory_FilesNotFound {
            get {
                return ResourceManager.GetString("Warning.ImportDirectory.FilesNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please select a PokerRoom from the Rooms Menu or if needed add Rooms via the Manage Rooms option first..
        /// </summary>
        internal static string Warning_NoPokerRoomSelected {
            get {
                return ResourceManager.GetString("Warning.NoPokerRoomSelected", resourceCulture);
            }
        }
    }
}
