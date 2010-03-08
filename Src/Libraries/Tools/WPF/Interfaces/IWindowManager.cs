namespace Tools.WPF.Interfaces
{
    using System;

    using Tools.Interfaces;

    public interface IWindowManager : IFluentInterface, IDisposable
    {
        object DataContext { get; set; }

        IWindowManager CreateWindow();

        IWindowManager Show();

        IWindowManager BringToFront();

        IWindowManager Hide();

        IntPtr Handle { get; }
    }
}