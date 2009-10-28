/*
 * User: Thorsten Lorenz
 * Date: 7/28/2009
 * 
*/
using System.Windows.Media;

namespace Tools.WPF.ViewModels
{
    public class ColorViewModel : NotifyPropertyChanged
    {
        private Color _color;
        public ColorViewModel() : this("UnNamed") {}

        public ColorViewModel(string Name)
        {
            this.Name = Name;
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                RaisePropertyChanged(() => Color);
                RaisePropertyChanged(() => SolidColorBrush);
            }
        }

        public Brush SolidColorBrush
        {
            get { return new SolidColorBrush(Color); }
            set { Color = ((SolidColorBrush) value).Color; }
        }

        public string Name { get; private set; }
    }
}