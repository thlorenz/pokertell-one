namespace PokerTell.SessionReview.ViewModels
{
    using Microsoft.Practices.Composite.Presentation.Commands;

    public interface ISessionReviewViewModel
    {
        DelegateCommand<object> SaveCommand { get; }
    }
}