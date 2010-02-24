namespace Tools.WPF.ViewModels
{
    using System.Windows;

    using Interfaces;

    public abstract class PositionedViewModel<T> : NotifyPropertyChanged, IPositionedViewModel<T>
    {
        double _left;

        double _top;

        bool _visible;

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

        public abstract void UpdateWith(T data);

        public void SetLocationTo(Point location)
        {
            Left = location.X;
            Top = location.Y;
        }
    }
}