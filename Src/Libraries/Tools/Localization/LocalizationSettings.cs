// © 2009 Rick Strahl. All rights reserved. 
// See http://wpflocalization.codeplex.com for related whitepaper and updates
// See http://wpfclientguidance.codeplex.com for other WPF resources

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows;

namespace Tools.Localization
{
    /// <summary>
    /// This class holds static configuration values that should
    /// be set once (or be auto-loaded once) for the application
    /// 
    /// This object becomes a static property on the ResExtension
    /// instance.
    /// </summary>
    public class LocalizationSettings : INotifyPropertyChanged
    {
        private static LocalizationSettings _current = new LocalizationSettings();
        internal static Hashtable AssemblyList = new Hashtable();
        private CultureInfo _currentCulture = CultureInfo.CurrentUICulture;
        private FlowDirection _flowDirection = FlowDirection.LeftToRight;

        /// <summary>
        /// Cached ResourceManagers for each ResourceSet supported requested.       
        /// </summary>
        internal Dictionary<string, ResourceManager> ResourceManagers = new Dictionary<string, ResourceManager>();

        private LocalizationSettings()
        {
            CheckForCultureChange = true;
        }

        /// <summary>
        /// Global singleton instance of configuration settings
        /// </summary>
        public static LocalizationSettings Current
        {
            get { return _current; }
            set { _current = value; }
        }


        /// <summary>
        /// The default resource manager used.
        /// </summary>        
        public ResourceManager DefaultResourceManager { get; set; }


        /// <summary>
        /// The Assembly from which resources are loaded
        /// </summary>
        public Assembly DefaultResourceAssembly { get; set; }


        public bool CheckForCultureChange { get; set; }

        /// <summary>
        /// Hold flow direction that can be bound to dynamically:
        /// FlowDirection="{Binding Source={x:Static res:LocalizationSettings.Current},Path=FlowDirection}"  
        /// </summary>
        public FlowDirection FlowDirection
        {
            get { return _flowDirection; }
            set
            {
                _flowDirection = value;
                RaisePropertyChanged("FlowDirection");
            }
        }

        /// <summary>
        /// Allows triggering of culture changes to rebind any active
        /// bindings.
        /// </summary>
        public CultureInfo CurrentCulture
        {
            get { return _currentCulture; }
            set
            {
                _currentCulture = value;
                FlowDirection = _currentCulture.TextInfo.IsRightToLeft
                                    ? FlowDirection.RightToLeft
                                    : FlowDirection.LeftToRight;
                OnCultureChanged();
                RaisePropertyChanged("CurrentCulture");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Allows adding an assembly to the internal lookup list of assemblies 
        /// that can be accessed for binding. 
        /// 
        /// This method allows preloading of the assembly as opposed to when
        /// the markup extension tries to find it.
        /// </summary>
        /// <param name="assemblyName"></param>
        public static Assembly AddAssembly(string assemblyName)
        {
            return AddAssembly(assemblyName, assemblyName);
        }

        /// <summary>
        /// Allows you to directly add an assembly to the internal assembly
        /// lookup list.
        /// This method allows preloading of the assembly as opposed to when
        /// the markup extension tries to find it.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Assembly AddAssembly(string assemblyName, Assembly assembly)
        {
            AssemblyList[assemblyName] = assembly;
            return assembly;
        }

        /// <summary>
        /// Allows adding an assembly to the list of assemblies that is
        /// used for lookup up resources. This version allows 
        /// </summary>
        /// <param name="assemblyId"></param>
        /// <param name="assemblyName"></param>
        public static Assembly AddAssembly(string assemblyId, string assemblyName)
        {
            Assembly assembly =
                AppDomain.CurrentDomain.GetAssemblies().Where(
                    asm => asm.GetName().Name == assemblyName || asm.FullName == assemblyName).FirstOrDefault();

            if (assembly == null)
            {
                throw new ArgumentException("Invalid assembly passed or assembly is not loaded.");
            }


            AssemblyList[assemblyId] = assembly;
            return assembly;
        }


        public static void Initialize(Assembly resourceAssembly, ResourceManager manager)
        {
            Current.DefaultResourceManager = manager;
            Current.DefaultResourceAssembly = resourceAssembly;
        }

        public event Action CultureChanged;

        protected void OnCultureChanged()
        {
            if (CultureChanged != null)
            {
                CultureChanged();
            }
        }


        /// <summary>
        /// Retrieves a resource manager for the appropriate ResourceSet
        /// By default the 'global' Resource
        /// </summary>
        /// <param name="resourceSet"></param>
        /// <param name="assembly">Assembly Name</param>
        /// <returns></returns>
        public static ResourceManager GetResourceManager(string resourceSet, string assembly)
        {
            // If we passed an assembly on the extension we have to look it up/load it
            // if the default resource assembly is not set - try to guess where to load it
            // from - this matters primarily at design time, otherwise
            // LocalizationSettings.Initialize() should be called from App.xaml.cs
            //if (assembly != null || LocalizationSettings.Current.DefaultResourceAssembly == null)
            //this.FindDefaultResourceAssembly();

            if (string.IsNullOrEmpty(resourceSet))
            {
                return Current.DefaultResourceManager;
            }

            if (Current.ResourceManagers.ContainsKey(resourceSet))
            {
                return Current.ResourceManagers[resourceSet];
            }

            // Can't load without a resource assembly
            if (Current.DefaultResourceAssembly == null)
            {
                return null;
            }

            var man = new ResourceManager(resourceSet, Current.DefaultResourceAssembly);
            Current.ResourceManagers.Add(resourceSet, man);
            man.GetString("");
            return man;
        }

        public static ResourceManager GetResourceManager(string resourceSet, Assembly assembly)
        {
            if (string.IsNullOrEmpty(resourceSet))
            {
                return Current.DefaultResourceManager;
            }

            if (Current.ResourceManagers.ContainsKey(resourceSet))
            {
                return Current.ResourceManagers[resourceSet];
            }

            var man = new ResourceManager(resourceSet, assembly);
            Current.ResourceManagers.Add(resourceSet, man);
            man.GetString("");
            return man;
        }


        protected virtual void RaisePropertyChanged(String propertyName)
        {
            if ((PropertyChanged != null))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}