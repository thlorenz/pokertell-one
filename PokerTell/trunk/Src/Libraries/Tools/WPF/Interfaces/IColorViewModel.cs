namespace Tools.WPF.Interfaces
{
    using System.Windows.Media;

    public interface IColorViewModel
    {
        Color Color { get; set; }

        Brush Brush { get; set; }

        string ColorString { get; set; }
    }
}