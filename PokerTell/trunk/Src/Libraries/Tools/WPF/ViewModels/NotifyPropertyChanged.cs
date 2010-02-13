/*
 * User: Thorsten Lorenz
 * Date: 7/28/2009
 * 
*/
namespace Tools.WPF.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;

    using Tools.GenericUtilities;
    using Tools.Interfaces;

    /// <summary>
    /// Abstract ViewModel that implements property name check when RaisePropertyChanged is raised
    /// </summary>
    [Serializable]
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        #region Events
        [field: NonSerialized]       
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
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