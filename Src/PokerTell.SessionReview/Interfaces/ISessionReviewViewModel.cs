namespace PokerTell.SessionReview.Interfaces
{
    using Infrastructure.Interfaces.PokerHand;

    using Microsoft.Practices.Composite.Presentation.Commands;

    using Tools.WPF.Interfaces;

    public interface ISessionReviewViewModel : IItemsRegionViewModel
    {
        DelegateCommand<object> SaveCommand { get; }

        IHandHistoriesViewModel HandHistoriesViewModel { get; }
    }
}