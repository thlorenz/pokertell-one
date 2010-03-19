namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.LiveTracker.Interfaces;

    using Tools.Interfaces;
    using Tools.WPF.ViewModels;


    public class GameHistoryViewModel : NotifyPropertyChanged, IGameHistoryViewModel
    {
        int _currentHandIndex;

        readonly IList<IConvertedPokerHand> _convertedHands;

        public GameHistoryViewModel(IHandHistoryViewModel handHistoryViewModel, ICollectionValidator collectionValidator)
        {
            CurrentHandHistory = handHistoryViewModel;
            _collectionValidator = collectionValidator;

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
                _currentHandIndex = _collectionValidator.GetValidIndexForCollection(value, HandCount);
                
                if (_currentHandIndex < HandCount)
                {
                    CurrentHandHistory.UpdateWith(_convertedHands[_currentHandIndex]);
                    RaisePropertyChanged(() => CurrentHandIndex);
                }
            }
        }

        string _tableName;

        readonly ICollectionValidator _collectionValidator;

        public string TableName
        {
            get { return _tableName; }
            protected set
            {
                _tableName = value;
                RaisePropertyChanged(() => TableName);
            }
        }

        public IGameHistoryViewModel AddNewHand(IConvertedPokerHand convertedPokerHand)
        {
            if (!_convertedHands.Contains(convertedPokerHand))
            {
                bool lastHandIsDisplayed = HandCount == 0 || CurrentHandIndex == HandCount - 1;

                _convertedHands.Add(convertedPokerHand);

                TableName = convertedPokerHand.TableName;

                RaisePropertyChanged(() => HandCount);

                // only update the display if the user is not currently browsing the hands
                if (lastHandIsDisplayed)
                    CurrentHandIndex = HandCount - 1;
            }

            return this;
        }
    }
}