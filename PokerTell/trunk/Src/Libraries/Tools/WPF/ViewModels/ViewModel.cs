/*
 * User: Thorsten Lorenz
 * Date: 7/28/2009
 * 
*/
namespace Tools.WPF.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq.Expressions;

    using Tools.GenericUtilities;
    using Tools.WPF.Interfaces;

    /// <summary>
    /// Abstract ViewModel that implements property name check when RaisePropertyChanged is raised
    /// </summary>
    public abstract class ViewModel : IViewModel
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public object Content { get; set; }

        public bool ThrowOnInvalidPropertyName { get; set; }

        #endregion

        #region Public Methods

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (ThrowOnInvalidPropertyName)
                {
                    throw new Exception(msg);
                }
                else
                {
                    Debug.Fail(msg);
                }
            }
        }

        #endregion

        #region Methods

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        /// <summary>
        /// Verifies passed in property name and then raises PropertyChanged Event
        /// </summary>
        protected void RaisePropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);

            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Use Static reflection to determine property name and thus no Verification is needed
        /// </summary>
        protected void RaisePropertyChanged<T>(Expression<Func<T>> expression)
        {
            OnPropertyChanged(Reflect.GetProperty(expression).Name);
        }

        #endregion
    }
}