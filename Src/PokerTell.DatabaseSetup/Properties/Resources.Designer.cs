﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4005
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PokerTell.DatabaseSetup.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PokerTell.DatabaseSetup.Properties.Resources", typeof(Resources).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Choose Database.
        /// </summary>
        public static string ChooseDatabaseViewModel_Title {
            get {
                return ResourceManager.GetString("ChooseDatabaseViewModel_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Choose Dataprovider.
        /// </summary>
        public static string ChooseDataProviderViewModel_Title {
            get {
                return ResourceManager.GetString("ChooseDataProviderViewModel_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Clear Database.
        /// </summary>
        public static string ClearDatabaseViewModel_Title {
            get {
                return ResourceManager.GetString("ClearDatabaseViewModel_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connection Settings.
        /// </summary>
        public static string ConfigureProviderView_ConnectionSettings {
            get {
                return ResourceManager.GetString("ConfigureProviderView_ConnectionSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password: .
        /// </summary>
        public static string ConfigureProviderView_Password {
            get {
                return ResourceManager.GetString("ConfigureProviderView_Password", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Server: .
        /// </summary>
        public static string ConfigureProviderView_Server {
            get {
                return ResourceManager.GetString("ConfigureProviderView_Server", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Test Connection.
        /// </summary>
        public static string ConfigureProviderView_TestConnection {
            get {
                return ResourceManager.GetString("ConfigureProviderView_TestConnection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use Defaults.
        /// </summary>
        public static string ConfigureProviderView_UseDefaults {
            get {
                return ResourceManager.GetString("ConfigureProviderView_UseDefaults", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use PokerOffice Settings.
        /// </summary>
        public static string ConfigureProviderView_UsePokerOfficeSettings {
            get {
                return ResourceManager.GetString("ConfigureProviderView_UsePokerOfficeSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User Name: .
        /// </summary>
        public static string ConfigureProviderView_UserName {
            get {
                return ResourceManager.GetString("ConfigureProviderView_UserName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Create Database.
        /// </summary>
        public static string CreateDatabaseViewModel_Title {
            get {
                return ResourceManager.GetString("CreateDatabaseViewModel_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Configure MySql Server.
        /// </summary>
        public static string DatabaseSetupMenu_ConfigureMySqlServer_Header {
            get {
                return ResourceManager.GetString("DatabaseSetupMenu_ConfigureMySqlServer_Header", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database Setup.
        /// </summary>
        public static string DatabaseSetupMenu_Header {
            get {
                return ResourceManager.GetString("DatabaseSetupMenu_Header", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Delete Database.
        /// </summary>
        public static string DeleteDatabaseViewModel_Title {
            get {
                return ResourceManager.GetString("DeleteDatabaseViewModel_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The {0} data provider was not found in the settings.
        ///Please reconfigure it..
        /// </summary>
        public static string Error_ProviderNotFoundInSettings {
            get {
                return ResourceManager.GetString("Error_ProviderNotFoundInSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server connection for {0} defined in the settings is invalid.
        ///Please reconfigure it.
        ///.
        /// </summary>
        public static string Error_SettingsContainInvalidServerConnectString {
            get {
                return ResourceManager.GetString("Error_SettingsContainInvalidServerConnectString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PokerTell was unable to connect to the database specified in the settings.
        ///Please choose/create another database..
        /// </summary>
        public static string Error_UnableToConnectToDatabaseSpecifiedInTheSettings {
            get {
                return ResourceManager.GetString("Error_UnableToConnectToDatabaseSpecifiedInTheSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PokerTell was unable to connect to the {0} server  as specified in the settings. 
        ///Please make sure the {0} server is running and check the Database Settings. 
        ///Ensure that the servername, username and possibly password were entered correctly..
        /// </summary>
        public static string Error_UnableToConnectToServer {
            get {
                return ResourceManager.GetString("Error_UnableToConnectToServer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occured when trying to create &apos;{0}&apos;.
        ///Choosing a simpler name may solve the problem..
        /// </summary>
        public static string Error_UnableToCreateDatabase {
            get {
                return ResourceManager.GetString("Error_UnableToCreateDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred when trying to delete &apos;{0}&apos;.
        /// Please restart PokerTell and try again..
        /// </summary>
        public static string Error_UnableToDeleteDatabase {
            get {
                return ResourceManager.GetString("Error_UnableToDeleteDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Since your data provider is {0}, make sure, that its server is running..
        /// </summary>
        public static string Hint_EnsureExternalServerIsRunning {
            get {
                return ResourceManager.GetString("Hint_EnsureExternalServerIsRunning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database {0} was chosen successfully..
        /// </summary>
        public static string Info_DatabaseChosen {
            get {
                return ResourceManager.GetString("Info_DatabaseChosen", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database &quot;{0}&quot; has been cleared successfully..
        /// </summary>
        public static string Info_DatabaseCleared {
            get {
                return ResourceManager.GetString("Info_DatabaseCleared", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database {0} was created successfully.
        ///In order to use it as your PokerTell database make sure to choose it via the {1} Menu..
        /// </summary>
        public static string Info_DatabaseCreated {
            get {
                return ResourceManager.GetString("Info_DatabaseCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database &quot;{0}&quot; has been deleted successfully..
        /// </summary>
        public static string Info_DatabaseDeleted {
            get {
                return ResourceManager.GetString("Info_DatabaseDeleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The {0} server was chosen as your data provider and the settings saved successfully..
        /// </summary>
        public static string Info_DataProviderChosen {
            get {
                return ResourceManager.GetString("Info_DataProviderChosen", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Found MySql {0}  installed on your computer..
        /// </summary>
        public static string Info_FoundMySqInstallation {
            get {
                return ResourceManager.GetString("Info_FoundMySqInstallation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No MySql installation found..
        /// </summary>
        public static string Info_MySqlInstallationNotFound {
            get {
                return ResourceManager.GetString("Info_MySqlInstallationNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The settings were saved successfully..
        /// </summary>
        public static string Info_SettingsSaved {
            get {
                return ResourceManager.GetString("Info_SettingsSaved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Successfully connected to the {0} server.
        ///Don&apos;t forget to save these settings and finally you should create and/or choose a database to use with it..
        /// </summary>
        public static string Info_SuccessfullyConnectedToServer {
            get {
                return ResourceManager.GetString("Info_SuccessfullyConnectedToServer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP TABLE IF EXISTS actionhhd;
        ///
        ///CREATE TABLE actionhhd (
        ///  playerid int(11) unsigned NOT NULL,
        ///  handid bigint(20) unsigned NOT NULL,
        ///  m int(10) unsigned DEFAULT NULL,
        ///  cards varchar(6) DEFAULT NULL,
        ///  position tinyint(2) DEFAULT NULL COMMENT &apos;Order of players acting on the hand (0-SB to TotalPlayers-1)&apos;,
        ///  strategicposition tinyint(2) DEFAULT NULL COMMENT &apos;Stratecic Position of player SB to BU&apos;,
        ///  inposflop tinyint(1) unsigned NOT NULL DEFAULT &apos;2&apos; COMMENT &apos;1=IP, 0=OOP 2=N/A(no action on Round)&apos; [rest of string was truncated]&quot;;.
        /// </summary>
        public static string MySql_Queries_CreateTables {
            get {
                return ResourceManager.GetString("MySql_Queries_CreateTables", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE DATABASE IF NOT EXISTS `{0}`;.
        /// </summary>
        public static string Sql_Queries_CreateDatabase {
            get {
                return ResourceManager.GetString("Sql_Queries_CreateDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP DATABASE IF EXISTS `{0}`;.
        /// </summary>
        public static string Sql_Queries_DropDatabase {
            get {
                return ResourceManager.GetString("Sql_Queries_DropDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SHOW DATABASES;.
        /// </summary>
        public static string Sql_Queries_ShowDatabases {
            get {
                return ResourceManager.GetString("Sql_Queries_ShowDatabases", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP TABLE IF EXISTS actionhhd; 
        ///
        ///CREATE TABLE actionhhd(
        ///       playerid int(11) NOT NULL,
        ///       handid bigint(20) NOT NULL,
        ///       m int(10) DEFAULT NULL,
        ///       cards varchar(6) DEFAULT NULL,
        ///       position tinyint(2) DEFAULT NULL,
        ///       strategicposition tinyint(2) DEFAULT NULL,
        ///       inposflop tinyint(1) NOT NULL  default &apos;2&apos;,
        ///       inposturn tinyint(1) NOT NULL default &apos;2&apos;,
        ///       inposriver tinyint(1) NOT NULL default &apos;2&apos;,
        ///       raiseinfrontpreflopfrompos tinyint(2) NOT NULL default [rest of string was truncated]&quot;;.
        /// </summary>
        public static string SQLite_Queries_CreateTables {
            get {
                return ResourceManager.GetString("SQLite_Queries_CreateTables", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connected to {0} using {1}.
        /// </summary>
        public static string Status_ConnectedTo {
            get {
                return ResourceManager.GetString("Status_ConnectedTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to not connected to any database.
        /// </summary>
        public static string Status_NotConnectedToDatabase {
            get {
                return ResourceManager.GetString("Status_NotConnectedToDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Warning.
        /// </summary>
        public static string Title_Warning {
            get {
                return ResourceManager.GetString("Title_Warning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All the data in the database &quot;{0}&quot; will be lost!
        ///Are you sure?.
        /// </summary>
        public static string Warning_AllDataInDatabaseWillBeLost {
            get {
                return ResourceManager.GetString("Warning_AllDataInDatabaseWillBeLost", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The database you are trying to create already exists.
        ///Therefore it could not be created.
        ///If you were trying to delete all its data, use Clear Database instead..
        /// </summary>
        public static string Warning_DatabaseExistsException {
            get {
                return ResourceManager.GetString("Warning_DatabaseExistsException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PokerTell was unable to find a MySql installatin on your computer.\nIf you are sure it is installed enter the appropriate settings and test the connection. If still unsuccessful consider reinstalling the MySql server..
        /// </summary>
        public static string Warning_MySqlInstallationNotFound {
            get {
                return ResourceManager.GetString("Warning_MySqlInstallationNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are not using a MySQL Database in PokerOffice.Follow the help file instructions to install the MySQL Server and change to MySQL Database in PokerOffice..
        /// </summary>
        public static string Warning_MySqlNotUsedInPokerOffice {
            get {
                return ResourceManager.GetString("Warning_MySqlNotUsedInPokerOffice", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PokerTell couldn&apos;t find any configured dataprovider.
        ///To remedy this please first configure a server to be used as your data provider..
        /// </summary>
        public static string Warning_NoConfiguredDataProvidersFoundInSettings {
            get {
                return ResourceManager.GetString("Warning_NoConfiguredDataProvidersFoundInSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No database has been chosen for the current data provider ({0}).
        ///Either create/choose a database to be used with {0 } or choose another data provider..
        /// </summary>
        public static string Warning_NoDatabaseHasBeenChosenForCurrentProvider {
            get {
                return ResourceManager.GetString("Warning_NoDatabaseHasBeenChosenForCurrentProvider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No data provider has been found.
        ///Please define it by using Choose Data Provider form the Database Menu..
        /// </summary>
        public static string Warning_NoDataProviderDefinedInSettings {
            get {
                return ResourceManager.GetString("Warning_NoDataProviderDefinedInSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Couldn&apos;t find a PokerOffice installation. 
        ///If you are sure it is installed carefully enter the values that you find in PokerOffice/Database/NetworkSettings..
        /// </summary>
        public static string Warning_PokerOfficeNotFound {
            get {
                return ResourceManager.GetString("Warning_PokerOfficeNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PokerTell was unable to connect to the Database specified in the settings. 
        ///Please make sure the MySql Server is running and check the Database Settings using the Database Menu.
        ///Check that the hostname, username, password were entered correctly and that you chose an existing database or created one..
        /// </summary>
        public static string Warning_UnableToConnectToDatabase {
            get {
                return ResourceManager.GetString("Warning_UnableToConnectToDatabase", resourceCulture);
            }
        }
    }
}
