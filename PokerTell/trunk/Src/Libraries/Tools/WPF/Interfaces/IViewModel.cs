namespace Tools.WPF.Interfaces
{
    using System.ComponentModel;

    public interface IViewModel : INotifyPropertyChanged
    {
        object Content { get; set; }

        bool ThrowOnInvalidPropertyName { get; set; }

    }
}