﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PokerTell.Repository.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PokerTell.Repository.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Application.
        /// </summary>
        public static string DatabaseImportView_Application {
            get {
                return ResourceManager.GetString("DatabaseImportView_Application", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database.
        /// </summary>
        public static string DatabaseImportView_Database {
            get {
                return ResourceManager.GetString("DatabaseImportView_Database", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Data Provider.
        /// </summary>
        public static string DatabaseImportView_Provider {
            get {
                return ResourceManager.GetString("DatabaseImportView_Provider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Import Database.
        /// </summary>
        public static string DatabaseImportView_Title {
            get {
                return ResourceManager.GetString("DatabaseImportView_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Import Hand Histories.
        /// </summary>
        public static string ImportHandHistoriesViewModel_Title {
            get {
                return ResourceManager.GetString("ImportHandHistoriesViewModel_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Successfully imported {0} hands from the {1} database..
        /// </summary>
        public static string Info_DatabaseImportCompleted {
            get {
                return ResourceManager.GetString("Info_DatabaseImportCompleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Successfully imported {0} hands..
        /// </summary>
        public static string Info_HandHistoriesDirectoryImportCompleted {
            get {
                return ResourceManager.GetString("Info_HandHistoriesDirectoryImportCompleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The {0} data provider has not been connected yet.
        ///You may do this via the Database Menu.
        ///This is the first step in order to import databases from {1}.
        ///
        ///Additionally, in order to import a database from {1} you also need to make sure to export it to a {0} data provider..
        /// </summary>
        public static string Warning_DataProviderUnavailable {
            get {
                return ResourceManager.GetString("Warning_DataProviderUnavailable", resourceCulture);
            }
        }
    }
}
