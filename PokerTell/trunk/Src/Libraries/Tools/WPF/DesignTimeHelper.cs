namespace Tools.WPF
{
    using System;
    using System.ComponentModel;
    using System.Windows;

    // From http://weblogs.asp.net/thomaslebrun/archive/2009/05/04/wpf-mvvm-how-to-get-data-in-design-time.aspx
    public class DesignTimeHelper
    {
        #region Constants and Fields

        /// <summary>
        /// DependencyProperty used to store the DesignTime property.
        /// </summary>
        public static readonly DependencyProperty DesignTimeDataProperty =
            DependencyProperty.RegisterAttached(
                "DesignTimeData", 
                typeof(Type), 
                typeof(DesignTimeHelper), 
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDesignTimeDataChanged)));

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the design time data.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static Type GetDesignTimeData(DependencyObject obj)
        {
            return (Type)obj.GetValue(DesignTimeDataProperty);
        }

        /// <summary>
        /// Sets the design time data.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetDesignTimeData(DependencyObject obj, Type value)
        {
            obj.SetValue(DesignTimeDataProperty, value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when DesignTimeData changed.
        /// </summary>
        /// <param name="d">The source object.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnDesignTimeDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool isOnDesignMode = DesignerProperties.GetIsInDesignMode(new DependencyObject());

            if (isOnDesignMode)
            {
                var element = d as FrameworkElement;

                if (element == null)
                {
                    throw new NullReferenceException("element must not be null and must be an UIElement.");
                }

                var designTimeDataType = e.NewValue as Type;

                if (designTimeDataType == null)
                {
                    throw new NullReferenceException("designTimeDataType must not be null.");
                }

                element.DataContext = Activator.CreateInstance(designTimeDataType);
            }
        }

        #endregion
    }
}