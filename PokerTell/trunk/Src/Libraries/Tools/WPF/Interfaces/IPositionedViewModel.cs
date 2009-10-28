namespace Tools.WPF.Interfaces
{
    using System.ComponentModel;
    using System.Windows;

    public interface IPositionedViewModel<T> : INotifyPropertyChanged
    {
        double Left { get; set; }

        double Top { get; set; }

        bool Visible { get; set; }

        void UpdateWith(T data);

        void SetLocationTo(Point location);
    }
}