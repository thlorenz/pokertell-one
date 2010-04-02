namespace PokerTell.User.Interfaces
{
    using Tools.WPF.Interfaces;

    public interface IReportWindowManager : IWindowManager
    {
        string Subject { get; set; }
    }
}