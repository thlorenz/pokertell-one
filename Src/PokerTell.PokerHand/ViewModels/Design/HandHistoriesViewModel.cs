namespace PokerTell.PokerHand.ViewModels.Design
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Services;

    using log4net;

    using Services;

    using Tools;
    using Tools.Interfaces;
    
    [Serializable]
    public class HandHistoriesViewModel : ViewModels.HandHistoriesViewModel
    {
        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public HandHistoriesViewModel()
            : this(
                new Constructor<IHandHistoryViewModel>(() => new HandHistoryViewModel()),
                new ItemsPagesManager<IHandHistoryViewModel>(),
            new HandHistoriesFilter())
        {
        }
        
        public HandHistoriesViewModel(
            IConstructor<IHandHistoryViewModel> handHistoryViewModelMake,
            IItemsPagesManager<IHandHistoryViewModel> itemsPageManager,
            IHandHistoriesFilter handHistoriesFilter)
            : base(handHistoryViewModelMake, itemsPageManager, handHistoriesFilter)
        {
            var designHelper = new DesignHelper();
            var hands = new List<IConvertedPokerHand>();

            for (int i = 0; i < 100; i++)
            {
                var hand = designHelper.CreateSamplePokerHand(i);
                if (i % 3 == 0)
                {
                    hand[0].Name = "hero";
                }

                hands.Add(hand);
            }
            
            InitializeWith(hands, 10);
        }
    }
}