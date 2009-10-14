namespace Tools.WPF.ViewModels
{
    using System.Windows;

    using Interfaces;

    public abstract class PositionedViewModel<T> : ViewModel, IPositionedViewModel<T>
    {
        #region Constants and Fields

        double _left;

        double _top;

        bool _visible;

        #endregion

        #region Properties

        public double Left
        {
            get { return _left; }

            set
            {
                _left = value;
                RaisePropertyChanged(() => Left);
            }
        }

        public double Top
        {
            get { return _top; }

            set
            {
                _top = value;
                RaisePropertyChanged(() => Top);
            }
        }

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

        public abstract void UpdateWith(T data);

        #endregion

        #region Methods

        public void SetLocationTo(Point location)
        {
            Left = location.X;
            Top = location.Y;
        }

        #endregion
    }
}