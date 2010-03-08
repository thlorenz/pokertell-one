namespace PokerTell.Statistics.ViewModels.Filters
{
    using System;

    using PokerTell.Infrastructure.Interfaces.Statistics;

    using Tools.WPF.ViewModels;

    public class ValueViewModel<T> : NotifyPropertyChanged, IValueViewModel<T>
        where T : IComparable
    {
        #region Constants and Fields

        readonly Func<T, string> _convertValueToDisplay;

        #endregion

        #region Constructors and Destructors

        public ValueViewModel(T value, Func<T, string> convertValueToDisplay)
        {
            Value = value;
            _convertValueToDisplay = convertValueToDisplay;
        }

        #endregion

        #region Properties

        public T Value { get; set; }

        bool _visible;

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                RaisePropertyChanged(() => Visible);
            }
        }

        #endregion

        #region Public Methods

        public int CompareTo(IValueViewModel<T> other)
        {
            return Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return _convertValueToDisplay(Value);
        }

        #endregion
    }
}