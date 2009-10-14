namespace Tools.WPF.Interfaces
{
    using System.Windows;

    public interface IPositionedViewModel<T> : IViewModel
    {
        double Left { get; set; }

        double Top { get; set; }

        bool Visible { get; set; }

        void UpdateWith(T data);

        void SetLocationTo(Point location);
    }
}