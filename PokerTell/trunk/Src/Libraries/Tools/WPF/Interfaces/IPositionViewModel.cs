namespace Tools.WPF.Interfaces
{
    using System.ComponentModel;

    using Tools.Interfaces;

    public interface IPositionViewModel : INotifyPropertyChanged, IFluentInterface
    {
        double Left { get; set; }

        double Top { get; set; }
    }
}