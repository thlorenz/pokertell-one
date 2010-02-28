namespace Tools.WPF.ViewModels
{
    using System.Windows.Media;

    using Controls;

    using Interfaces;

    /// <summary>
    /// Provides different formats of a given Color.
    /// Right now it is used with the <see cref="ColorPicker"/>
    /// </summary>
    public class ColorViewModel : NotifyPropertyChanged, IColorViewModel
    {
        public ColorViewModel(string colorString)
        {
            ColorString = colorString;
        }
        
        Color _color;
     
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                RaisePropertyChangedForAllProperties();
            }
        }

        public void RaisePropertyChangedForAllProperties()
        {
            RaisePropertyChanged(() => Color);
            RaisePropertyChanged(() => Brush);
            RaisePropertyChanged(() => ColorString);
        }

        public Brush Brush
        {
            get { return new SolidColorBrush(Color); }
            set { Color = ((SolidColorBrush) value).Color; }
        }

        public string ColorString
        {
            get { return new ColorConverter().ConvertToString(Color); }
            set { Color = (Color)ColorConverter.ConvertFromString(value); }
        }
        public override string ToString()
        {
            return ColorString;
        }
    }
}