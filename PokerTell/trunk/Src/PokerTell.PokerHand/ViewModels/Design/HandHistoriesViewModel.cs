namespace PokerTell.PokerHand.ViewModels.Design
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Services;

    public class HandHistoriesViewModel : ViewModels.HandHistoriesViewModel
    {
        public HandHistoriesViewModel()
            : this(new Constructor<IHandHistoryViewModel>(() => new HandHistoryViewModel()))
        {
        }
        
        public HandHistoriesViewModel(IConstructor<IHandHistoryViewModel> handHistoryViewModelMake)
            : base(handHistoryViewModelMake)
        {
            var designHelper = new DesignHelper();
            var hands = new List<IConvertedPokerHand>();

            for (int i = 0; i < 15; i++)
            {
                hands.Add(designHelper.CreateSamplePokerHand(i));
            }

            InitializeWith(hands);

            HeaderInfo = "SessionReview";
        }
    }
}