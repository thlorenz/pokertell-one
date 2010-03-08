// © 2009 Rick Strahl. All rights reserved. 
// See http://wpflocalization.codeplex.com for related whitepaper and updates
// See http://wpfclientguidance.codeplex.com for other WPF resources

namespace Tools.Extensions
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Resources;
    using System.Windows;

    using Tools.Localization;

    /// <summary>
    /// Provides a few attached properties for use in localization
    ///     
    /// </summary>
    public class TranslationExtension : DependencyObject
    {
        #region Constants and Fields

        public static readonly DependencyProperty TranslateProperty =
            DependencyProperty.RegisterAttached(
                "Translate", 
                typeof(bool), 
                typeof(
                    FrameworkElement
                    ), 
                new FrameworkPropertyMetadata
                    (
                    false, 
                    FrameworkPropertyMetadataOptions
                        .
                        AffectsRender, 
                    OnTranslateChanged));

        public static readonly DependencyProperty TranslateResourceAssemblyProperty =
            DependencyProperty.RegisterAttached(
                "TranslateResourceAssembly", 
                typeof(string), 
                typeof(TranslationExtension), 
                new FrameworkPropertyMetadata(
                    string.Empty, 
                    FrameworkPropertyMetadataOptions.
                        AffectsRender));

        public static readonly DependencyProperty TranslateResourceSetProperty =
            DependencyProperty.RegisterAttached(
                "TranslateResourceSet", 
                typeof(string), 
                typeof(TranslationExtension), 
                new FrameworkPropertyMetadata(
                    string.Empty, 
                    FrameworkPropertyMetadataOptions.
                        AffectsRender));

        #endregion

        #region Public Methods

        public static bool GetTranslate(UIElement element)
        {
            return (bool)element.GetValue(TranslateProperty);
        }

        public static string GetTranslateResourceAssembly(UIElement element)
        {
            return element.GetValue(TranslateResourceAssemblyProperty) as string;
        }

        public static string GetTranslateResourceSet(UIElement element)
        {
            return element.GetValue(TranslateResourceSetProperty) as string;
        }

        public static void SetTranslate(UIElement element, bool value)
        {
            element.SetValue(TranslateProperty, value);
        }

        public static void SetTranslateResourceAssembly(UIElement element, string value)
        {
            element.SetValue(TranslateResourceAssemblyProperty, value);
        }

        public static void SetTranslateResourceSet(UIElement element, string value)
        {
            element.SetValue(TranslateResourceSetProperty, value);
        }

        #endregion

        #region Methods

        static void OnTranslateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                TranslateKeys(d as FrameworkElement);
            }
        }

        static void TranslateKeys(UIElement element)
        {
            if (DesignerProperties.GetIsInDesignMode(element))
            {
                return;
            }

            var root = WpfUtils.GetRootVisual(element) as FrameworkElement;
            if (root == null)
            {
                return; // must be framework element to find root
            }

            // Retrieve the resource set and assembly from the top level element
            var resourceset = root.GetValue(TranslateResourceSetProperty) as string;
            var resourceAssembly = root.GetValue(TranslateResourceAssemblyProperty) as string;

            // Error in sourcecode ?? TL
            // if (element is FrameworkElement && ((FrameworkElement) element).Name == "lblLocale")
            // root = root;
            ResourceManager manager = null;
            if (resourceAssembly == null)
            {
                manager = LocalizationSettings.GetResourceManager(resourceset, root.GetType().Assembly);
            }
            else
            {
                manager = LocalizationSettings.GetResourceManager(resourceset, resourceAssembly);
            }

            // find neutral culture so we can iterate over all keys
            ResourceSet set = manager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
            IDictionaryEnumerator enumerator = set.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var key = enumerator.Key as string;
                if (key.StartsWith(element.Uid + "."))
                {
                    string property = key.Split('.')[1];
                    object value = manager.GetObject(key); // enumerator.Value;

                    // Bind the value AFTER control has initialized or else the
                    // default will override what we bind here
                    root.Initialized += delegate {
                        try
                        {
                            PropertyInfo prop = element.GetType().GetProperty(
                                property, 
                                BindingFlags.Public |
                                BindingFlags.Instance |
                                BindingFlags.FlattenHierarchy |
                                BindingFlags.IgnoreCase);
                            prop.SetValue(element, value, null);
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(
                                string.Format(
                                    "TranslateExtension Resource Failure: {0}  - {1}", 
                                    key, 
                                    ex.Message));
                        }
                    };
                }
            }
        }

        #endregion
    }
}