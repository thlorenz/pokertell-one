namespace PokerTell.LiveTracker.ViewModels.Overlay
{
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

        double _left;

        public double Left
        {
            get { return _left; }
            set
            {
                _left = value;
                RaisePropertyChanged(() => Left);
            }
        }

        double _top;

        public double Top
        {
            get { return _top; }
            set
            {
                _top = value;
                RaisePropertyChanged(() => Top);
            }
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