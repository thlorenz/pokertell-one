namespace PokerTell.User.ViewModels
{
    public interface IProgressViewModel
    {
        double PercentCompleted { get; set; }

        bool Visible { get; set; }
    }
}