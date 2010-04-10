namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System;
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

        readonly IDispatcherTimer _scrollToNewestHandTimer;

        readonly ISettings _settings;

        int _currentHandIndex;

        string _tableName;

        public GameHistoryViewModel(
            ISettings settings, 
            IDimensionsViewModel dimensions, 
            IHandHistoryViewModel handHistoryViewModel, 
            IDispatcherTimer scrollToNewestHandTimer, 
            ICollectionValidator collectionValidator)
        {
            _settings = settings;
            CurrentHandHistory = handHistoryViewModel;
            _scrollToNewestHandTimer = scrollToNewestHandTimer;
            _collectionValidator = collectionValidator;

            Dimensions = dimensions.InitializeWith(settings.RetrieveRectangle(DimensionsKey, new Rectangle(0, 0, 600, 200)));

            _convertedHands = new List<IConvertedPokerHand>();

            RegisterEvents();
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

                    MakeSureWeScrollToTheNewestHandShortlyIfNecessary();
                }
            }
        }

        public IDimensionsViewModel Dimensions { get; protected set; }

        public int HandCount
        {
            get { return _convertedHands.Count; }
        }

        public bool NewestHandIsDisplayed
        {
            get { return HandCount == 0 || CurrentHandIndex == HandCount - 1; }
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
                bool newestHandWasDisplayed = NewestHandIsDisplayed;

                _convertedHands.Add(convertedPokerHand);

                TableName = convertedPokerHand.TableName;

                RaisePropertyChanged(() => HandCount);

                // only update the display if the user is not currently browsing the hands
                if (newestHandWasDisplayed)
                    CurrentHandIndex = HandCount - 1;
            }

            return this;
        }

        public IGameHistoryViewModel SaveDimensions()
        {
            _settings.Set(DimensionsKey, Dimensions.Rectangle);

            return this;
        }

        void MakeSureWeScrollToTheNewestHandShortlyIfNecessary()
        {
            if (NewestHandIsDisplayed)
                _scrollToNewestHandTimer.Stop();
            else
                _scrollToNewestHandTimer.Start();
        }

        void RegisterEvents()
        {
            _scrollToNewestHandTimer.Interval = TimeSpan.FromSeconds(30.0);
            _scrollToNewestHandTimer.Tick += (s, e) => {
                CurrentHandIndex = HandCount - 1;
                _scrollToNewestHandTimer.Stop();
            };
        }
    }
}