namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System.Collections.Generic;
    using System.Drawing;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.LiveTracker.Interfaces;

    using Tools.Interfaces;
    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    public class GameHistoryViewModel : NotifyPropertyChanged, IGameHistoryViewModel
    {
        public const string DimensionsKey = "PokerTell.LiveTracker.GameHistory.Dimensions";

        readonly ICollectionValidator _collectionValidator;

        readonly IList<IConvertedPokerHand> _convertedHands;

        readonly IDimensionsViewModel _dimensions;

        readonly ISettings _settings;

        int _currentHandIndex;

        string _tableName;

        public GameHistoryViewModel(
            ISettings settings, IDimensionsViewModel dimensions, IHandHistoryViewModel handHistoryViewModel, ICollectionValidator collectionValidator)
        {
            _settings = settings;
            _dimensions = dimensions;
            CurrentHandHistory = handHistoryViewModel;
            _collectionValidator = collectionValidator;

            Dimensions = dimensions.InitializeWith(settings.RetrieveRectangle(DimensionsKey, new Rectangle(0, 0, 600, 200)));

            _convertedHands = new List<IConvertedPokerHand>();
        }

        public IHandHistoryViewModel CurrentHandHistory { get; protected set; }

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

        public IDimensionsViewModel Dimensions { get; protected set; }

        public int HandCount
        {
            get { return _convertedHands.Count; }
        }

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

        public IGameHistoryViewModel SaveDimensions()
        {
            _settings.Set(DimensionsKey, Dimensions.Rectangle);

            return this;
        }
    }
}