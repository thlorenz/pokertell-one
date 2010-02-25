namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Interfaces;

    using Tools.FunctionalCSharp;
    using Tools.WPF.ViewModels;

    public class HarringtonMViewModel : NotifyPropertyChanged, IHarringtonMViewModel
    {
        int _value;

        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged(() => Value);
                RaisePropertyChanged(() => Zone);
            }
        }

        IList<Point> _harringtonMPositions;

        int _seatNumber;

        public double Left
        {
            get { return _harringtonMPositions[_seatNumber].X; }

            set
            {
                _harringtonMPositions[_seatNumber] = new Point(value, Top);
                RaisePropertyChanged(() => Left);
            }
        }

        public double Top
        {
            get { return _harringtonMPositions[_seatNumber].Y; }

            set
            {
                _harringtonMPositions[_seatNumber] = new Point(Left, value);
                RaisePropertyChanged(() => Top);
            }
        }

        public IHarringtonMViewModel InitializeWith(IList<Point> harringtonMPositions, int seatNumber)
        {
            _seatNumber = seatNumber;
            _harringtonMPositions = harringtonMPositions;
            return this;
        }

        public string Zone
        {
            get
            {
                return Value.Match()
                    .With(v => v >= 20, _ => "Green")
                    .With(v => v > 10, _ => "Yellow")
                    .With(v => v > 6, _ => "Orange")
                    .Else(_ => "Red")
                    .Do();
            }
        }
    }
}