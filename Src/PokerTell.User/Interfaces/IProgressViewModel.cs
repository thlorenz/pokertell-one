namespace PokerTell.User.Interfaces
{
    public interface IProgressViewModel
    {
        double PercentCompleted { get; set; }

        bool Visible { get; set; }
    }
}