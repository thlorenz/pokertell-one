namespace Tools.WPF.Interfaces
{
    using System.ComponentModel;

    using Tools.Interfaces;

    public interface IPositionViewModel : INotifyPropertyChanged
    {
        double Left { get; set; }

        double Top { get; set; }
    }
}