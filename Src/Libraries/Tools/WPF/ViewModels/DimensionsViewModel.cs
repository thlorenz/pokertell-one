namespace Tools.WPF.ViewModels
{
    using System;
    using System.Drawing;

    using Interfaces;

    /// <summary>
    /// Adapts a plain drawing rectangle to be used in WPF databinding scenario.
    /// It also makes sure that the given location is found on on of the connected sceens.
    /// If not it will correct the location to 0, 0
    /// </summary>
    public class DimensionsViewModel : NotifyPropertyChanged, IDimensionsViewModel
    {
        Rectangle _rectangle;

        public Rectangle Rectangle
        {
            get { return _rectangle; }
        }

        public DimensionsViewModel()
        {
        }

        public DimensionsViewModel(Rectangle rectangle)
        {
            InitializeWith(rectangle);
        }

        public DimensionsViewModel(int left, int top, int width, int height)
        {
            InitializeWith(left, top, width, height);
        }

        public IDimensionsViewModel InitializeWith(Rectangle rectangle)
        {
            _rectangle = rectangle;
            ValidateAndCorrectLocation();
            return this;
        }

        public IDimensionsViewModel InitializeWith(int left, int top, int width, int height)
        {
            _rectangle = new Rectangle(left, top, width, height); 
            ValidateAndCorrectLocation();
            return this;
        }

        public int Left
        {
            get { return Rectangle.X; }
            set
            {
                _rectangle.X = value;
                RaisePropertyChanged(() => Left);
            }
        }

        public int Top
        {
            get { return Rectangle.Y; }
            set
            {
                _rectangle.Y = value;
                RaisePropertyChanged(() => Top);
            }
        }

        public int Width
        {
            get { return Rectangle.Width; }
            set
            {
                _rectangle.Width = value;
                RaisePropertyChanged(() => Width);
            }
        }

        public int Height
        {
            get { return Rectangle.Height; }
            set
            {
                _rectangle.Height = value;
                RaisePropertyChanged(() => Height);
            }
        }

        void ValidateAndCorrectLocation()
        {
            if (! Utils.ThisPointIsOnOneOfTheConnectedScreens(_rectangle.Location))
                _rectangle.Location = new Point(0, 0);
        }
    }
}