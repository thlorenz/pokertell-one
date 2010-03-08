namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Interfaces;

    using Tools.FunctionalCSharp;
    using Tools.WPF.Interfaces;
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

        public IPositionViewModel Position { get; protected set; }

        public IHarringtonMViewModel InitializeWith(IPositionViewModel position)
        {
            Position = position;
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