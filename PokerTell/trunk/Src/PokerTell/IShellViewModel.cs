namespace PokerTell
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public interface IShellViewModel
    {
        ICommand DevelopmentCommand { get; }

        ICommand MaximizeWindowCommand { get; }

        ICommand MinimizeWindowCommand { get; }

        ICommand NormalizeWindowCommand { get; }

        bool ShowMaximize { get; }

        bool ShowNormalize { get; }

        WindowState WindowState { get; set; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}