namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.LiveTracker.Interfaces;

    using Tools.WPF.ViewModels;

    public class GameHistoryViewModel : NotifyPropertyChanged, IGameHistoryViewModel
    {
        int _currentHandIndex;

        readonly IList<IConvertedPokerHand> _convertedHands;

        public GameHistoryViewModel(IHandHistoryViewModel handHistoryViewModel)
        {
            CurrentHandHistory = handHistoryViewModel;
            _convertedHands = new List<IConvertedPokerHand>();
        }

        public IHandHistoryViewModel CurrentHandHistory { get; protected set; }

        public int HandCount
        {
            get { return _convertedHands.Count; }
        }            

        public int CurrentHandIndex
        {
            get { return _currentHandIndex; }
            set
            {
                _currentHandIndex = value;
                
                CurrentHandHistory.UpdateWith(_convertedHands[_currentHandIndex]);
                RaisePropertyChanged(() => CurrentHandIndex);
            }
        }

        public IGameHistoryViewModel AddNewHand(IConvertedPokerHand convertedPokerHand)
        {
            if (!_convertedHands.Any(hand => hand.Equals(convertedPokerHand)))
            {
                bool lastHandIsDisplayed = HandCount == 0 || CurrentHandIndex == HandCount - 1;

                _convertedHands.Add(convertedPokerHand);

                RaisePropertyChanged(() => HandCount);

                // only update the display if the user is not currently browsing the hands
                if (lastHandIsDisplayed)
                    CurrentHandIndex = HandCount - 1;
            }

            return this;
        }
    }
}