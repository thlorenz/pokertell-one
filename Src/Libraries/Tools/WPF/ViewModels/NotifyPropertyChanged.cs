/*
 * User: Thorsten Lorenz
 * Date: 7/28/2009
 * 
*/
namespace Tools.WPF.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;

    using Tools.GenericUtilities;

    /// <summary>
    /// Abstract ViewModel that implements property name check when RaisePropertyChanged is raised
    /// </summary>
    [Serializable]
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

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
    }
}